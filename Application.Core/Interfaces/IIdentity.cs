using System.Threading.Tasks;
using Application.Core.Models;

namespace Application.Core.Interfaces
{
    public interface IIdentity<in T>
    {
        public bool IsAdmin(T source);

        public Task<UserInfo> GetUserInfo(T source);
    }
}