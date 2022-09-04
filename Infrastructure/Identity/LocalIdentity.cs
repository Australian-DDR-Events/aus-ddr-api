using System;
using System.Threading.Tasks;
using Application.Core.Interfaces;
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
    }
}