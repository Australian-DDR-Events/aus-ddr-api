using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.ExternalServices.Discord;

namespace Application.Core.Interfaces.ExternalServices;

public interface IDiscordApiService
{
    public Task<string> AuthCodeExchange(string code);
    public Task<AuthorizationMeResponse?> GetAuthorizationInfo(string token, CancellationToken cancellationToken);
}