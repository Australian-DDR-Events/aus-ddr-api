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

        public Task<UserInfo> GetUserInfo(string source)
        {
            throw new NotImplementedException();
        }
    }
}