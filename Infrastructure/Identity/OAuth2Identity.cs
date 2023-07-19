using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Entities.Internal;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Identity;
using Application.Core.Interfaces.Repositories;
using Application.Core.Models;
using JetBrains.Annotations;

namespace Infrastructure.Identity
{
    public class OAuth2Identity : IIdentity<string>
    {
        private readonly ICache _cache;
        private readonly OAuth2IdentityConfig _config;
        private readonly HttpClient _client;
        private readonly ISessionRepository _sessionRepository;
        private readonly IDancerRepository _dancerRepository;
        private readonly ILogger _logger;
        
        public OAuth2Identity(ICache cache, OAuth2IdentityConfig config, HttpClient client, ISessionRepository sessionRepository, IDancerRepository dancerRepository, ILogger logger)
        {
            _cache = cache;
            _config = config;
            _client = client;
            _sessionRepository = sessionRepository;
            _dancerRepository = dancerRepository;
            _logger = logger;
        }
        
        public bool IsAdmin(string cookie)
        {
            var token = _cache.Fetch<string>(cookie);
            if (token == null) return false;
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var groupsClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals("cognito:groups", StringComparison.OrdinalIgnoreCase));
            if (groupsClaim == null) return false;
            var groupsArray = groupsClaim.Value.Split(" ");
            return groupsArray.Contains("administrators", StringComparer.OrdinalIgnoreCase);
        }

        public async Task<UserInfo> GetUserInfo(string cookie)
        {
            var token = _cache.Fetch<string>(cookie);
            using var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, _config.UserinfoEndpoint);
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    
            var response = await _client.SendAsync(requestMessage);
            return await response.Content.ReadFromJsonAsync<UserInfo>();
        }

        public TokenType GetTokenType(string source)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(source) as JwtSecurityToken;
            var sub = jsonToken?.Subject;
            var client =
                jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals("client_id", StringComparison.OrdinalIgnoreCase));
            if (sub == null || client == null) return TokenType.INVALID;
            return sub.Equals(client.Value)
                ? TokenType.SERVICE
                : TokenType.USER;
        }

        public bool HasScope(string source, string scope)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(source) as JwtSecurityToken;
            var scopes = jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals("scope", StringComparison.OrdinalIgnoreCase));
            return scopes != null && scopes.Value.Split(" ").Any($"AusDdrEventsApi/{scope}".Equals);
        }

        public bool IsSessionActive(string cookie)
        {
            var token = _cache.Fetch<string>(cookie);
            if (token == null || !token.Any()) return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var expString = jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals("exp", StringComparison.OrdinalIgnoreCase))?.Value;
            if (expString == null) return false;

            var exp = DateTime.UnixEpoch.AddSeconds(long.Parse(expString));
            return DateTime.Compare(exp, DateTime.Now.ToUniversalTime()) > 0;
        }

        public async Task<string> CreateSession(string code)
        {
            var formData = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "authorization_code"),
                new("code", code),
                new("redirect_uri", _config.RedirectUri)
            };

            var tokenData = await GetTokens(formData);
            if (tokenData == null)
            {
                return string.Empty;
            }
    
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(tokenData.AccessToken) as JwtSecurityToken;
            var sub = jsonToken?.Subject;
            if (sub == null || !sub.Any())
            {
                return string.Empty;
            }

            var dancer = _dancerRepository.GetDancerByAuthId(sub);
            if (dancer == null)
            {
                _logger.Warn($"No dancer found for subject {sub}");
                return string.Empty;
            }

            var newSession = new Session
            {
                Cookie = GenerateCookie(),
                DancerId = dancer.Id,
                Expiry = DateTime.Now.AddMonths(1).ToUniversalTime(),
                RefreshToken = tokenData?.RefreshToken ?? string.Empty
            };
            await _sessionRepository.CreateSession(newSession);
            _cache.Add(newSession.Cookie, tokenData.AccessToken);

            return newSession.Cookie;
        }

        public async Task<string> RefreshSession(string cookie)
        {
            var session = _sessionRepository.GetSessionByCookie(cookie);
            if (session == null) return string.Empty;

            var formData = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "refresh_token"),
                new("refresh_token", session.RefreshToken)
            };
            var tokenData = await GetTokens(formData);
            if (tokenData == null)
            {
                return string.Empty;
            }

            var newSession = new Session
            {
                Cookie = GenerateCookie(),
                DancerId = session.DancerId,
                Expiry = DateTime.Now.AddMonths(1).ToUniversalTime(),
                RefreshToken = tokenData?.RefreshToken ?? session.RefreshToken
            };
            await _sessionRepository.CreateSession(newSession);
            await _sessionRepository.DeleteSessionByCookie(cookie);
            
            _cache.Add(newSession.Cookie, tokenData.AccessToken);
            _cache.Delete(cookie);

            return newSession.Cookie;
        }

        public async Task ClearSession(string cookie)
        {
            _cache.Delete(cookie);
            await _sessionRepository.DeleteSessionByCookie(cookie);
        }

        private async Task<TokenEndpointResponse> GetTokens(List<KeyValuePair<string, string>> formData)
        {
            var authBytes = Encoding.UTF8.GetBytes($"{_config.ClientId}:{_config.ClientSecret}");
            var auth = Convert.ToBase64String(authBytes);
            var content = new HttpRequestMessage(HttpMethod.Post, _config.TokenEndpoint);
            content.Content = new FormUrlEncodedContent(formData);
            content.Headers.Add("Authorization", $"Basic {auth}");
            var response = await _client.SendAsync(content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warn($"Token endpoint failed: {await response.Content.ReadAsStringAsync()}");
                return null;
            }
            return await response.Content.ReadFromJsonAsync<TokenEndpointResponse>();
        }

        private static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_".ToCharArray();

        private string GenerateCookie()
        {
            var data = new byte[120];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }

            var result = new StringBuilder(30);
            for (var i = 0; i < 30; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;
                result.Append(chars[idx]);
            }

            return result.ToString();
        }
    }
}