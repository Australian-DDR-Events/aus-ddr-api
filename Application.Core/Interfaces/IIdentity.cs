using System.Threading.Tasks;
using Application.Core.Interfaces.Identity;
using Application.Core.Models;

namespace Application.Core.Interfaces
{
    public interface IIdentity<in T>
    {
        public bool IsAdmin(T source);

        public Task<UserInfo> GetUserInfo(T source);

        public TokenType GetTokenType(string source);

        public bool HasScope(string source, string scope);

        public bool IsSessionActive(string cookie);
        public Task<string> CreateSession(string code);

        public Task<string?> RefreshSession(string cookie);
    }
}