using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Application.Core.Interfaces;

namespace Infrastructure.Identity
{
    public class OAuth2Identity : IIdentity<string>
    {
        private readonly ICache _cache;
        private readonly OAuth2IdentityConfig _config;
        
        public OAuth2Identity(ICache cache, OAuth2IdentityConfig config)
        {
            _cache = cache;
            _config = config;
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
    }
}