using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IConnectionRepository
{
    public Task CreateConnection(Connection connection, CancellationToken cancellationToken);
}