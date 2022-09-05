using System;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Identity;
using Application.Core.Models;

namespace Infrastructure.Identity
{
    public class LocalIdentity : IIdentity<string>
    {
        public bool IsAdmin(string source)
        {
            return source.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<UserInfo> GetUserInfo(string source)
        {
            var rand = new Random(source.GetHashCode());
            var guid = new byte[16];
            rand.NextBytes(guid);
            return new UserInfo
            {
                UserId = new Guid(guid).ToString(),
                Name = source
            };
        }

        public TokenType GetTokenType(string source)
        {
            return source.Contains("service")
                ? TokenType.SERVICE
                : TokenType.USER;
        }

        public bool HasScope(string source, string scope)
        {
            return true;
        }
    }
}