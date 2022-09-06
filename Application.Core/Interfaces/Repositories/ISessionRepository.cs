using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Entities.Internal;

namespace Application.Core.Interfaces.Repositories;

public interface ISessionRepository
{
    public Task CreateSession(Session session);
    public Session? GetSessionByCookie(string cookie);
    public IList<Session> GetSessionsForUser(Guid userId);

    public Task DeleteSessionByCookie(string cookie);
    public Task DeleteSessionsForUser(Guid userId);

    public Task DeleteExpiredSessions();
}