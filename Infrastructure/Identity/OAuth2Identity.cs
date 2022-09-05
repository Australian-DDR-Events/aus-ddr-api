using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Identity;
using Application.Core.Models;

namespace Infrastructure.Identity
{
    public class OAuth2Identity : IIdentity<string>
    {
        private readonly ICache _cache;
        private readonly OAuth2IdentityConfig _config;
        private readonly HttpClient _client;
        
        public OAuth2Identity(ICache cache, OAuth2IdentityConfig config, HttpClient client)
        {
            _cache = cache;
            _config = config;
            _client = client;
        }
        
        public bool IsAdmin(string source)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(source) as JwtSecurityToken;
            var groupsClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals("cognito:groups", StringComparison.OrdinalIgnoreCase));
            if (groupsClaim == null) return false;
            var groupsArray = groupsClaim.Value.Split(" ");
            return groupsArray.Contains("administrators", StringComparer.OrdinalIgnoreCase);
        }

        public async Task<UserInfo> GetUserInfo(string source)
        {
            if (!source.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase)) return new UserInfo();
            var token = source.Split(" ")[1];
            using var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, _config.UserinfoEndpoint);
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    
            var response = await _client.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            return JsonSerializer.Deserialize<UserInfo>(body);
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
    }
}