using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Interfaces.Services;

public interface IConnectionService<in T>
{
    public Task<bool> CreateConnection(string authId, T connectionData, CancellationToken cancellationToken);
}
