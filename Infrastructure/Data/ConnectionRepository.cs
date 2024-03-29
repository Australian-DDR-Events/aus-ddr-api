using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;

namespace Infrastructure.Data;

public class ConnectionRepository : IConnectionRepository
{
    private readonly EFDatabaseContext _context;
    
    public ConnectionRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateConnection(Connection connection, CancellationToken cancellationToken)
    {
        await _context
            .Connections
            .AddAsync(connection, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public IList<Connection> GetConnection(Guid userId, Connection.ConnectionType connectionType)
    {
        return _context
            .Connections
            .Where(c => c.DancerId.Equals(userId) && c.Type.Equals(connectionType))
            .ToList();
    }
}