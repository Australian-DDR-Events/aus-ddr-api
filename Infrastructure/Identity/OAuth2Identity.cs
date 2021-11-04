using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Core.Interfaces;
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
            var groupsClaim = jsonToken.Claims.FirstOrDefault(c => c.Type.Equals("cognito:groups", StringComparison.OrdinalIgnoreCase));
            if (groupsClaim == null) return false;
            var groupsArray = groupsClaim.Value.Split(" ");
            return groupsArray.Contains("administrators", StringComparer.OrdinalIgnoreCase);
        }

        public async Task<UserInfo> GetUserInfo(string source)
        {
            using var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, _config.UserinfoEndpoint);
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", source);
    
            var response = await _client.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            return JsonSerializer.Deserialize<UserInfo>(body);
        }
    }
}