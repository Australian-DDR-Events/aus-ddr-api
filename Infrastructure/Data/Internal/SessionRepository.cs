using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Entities.Internal;
using Application.Core.Interfaces.Repositories;

namespace Infrastructure.Data.Internal;

public class SessionRepository : ISessionRepository
{
    private readonly EFDatabaseContext _context;
    
    public SessionRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateSession(Session session)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
    }

    public Session GetSessionByCookie(string cookie)
    {
        return _context
            .Sessions
            .FirstOrDefault(cookie.Equals);
    }

    public IList<Session> GetSessionsForUser(Guid userId)
    {
        return _context
            .Sessions
            .Where(s => s.DancerId.Equals(userId))
            .ToList();
    }

    public async Task DeleteSessionByCookie(string cookie)
    {
        var session = GetSessionByCookie(cookie);
        if (session == null) return;
        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSessionsForUser(Guid userId)
    {
        var sessions = GetSessionsForUser(userId);
        _context.Sessions.RemoveRange(sessions);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpiredSessions()
    {
        var sessions = _context
            .Sessions
            .Where(s => DateTime.Compare(s.Expiry, DateTime.Now.ToUniversalTime()) < 0)
            .Take(100)
            .ToList();
        _context.Sessions.RemoveRange(sessions);
        await _context.SaveChangesAsync();
    }
}