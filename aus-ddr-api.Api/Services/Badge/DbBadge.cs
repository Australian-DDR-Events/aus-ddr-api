using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.Services.Badges
{
    public class DbBadge : IBadge
    {
        private readonly DatabaseContext _context;

        public DbBadge(DatabaseContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Badge> GetAll()
        {
            return _context
                .Badges
                .Include(b => b.Event)
                .AsSplitQuery()
                .ToList();
        }

        public IEnumerable<Badge> GetAssigned(Guid dancerId)
        {
            return _context
                .Badges
                .Include(b => b.Dancers)
                .Include(b => b.Event)
                .AsSplitQuery()
                .Where(b => b.Dancers.Any(d => d.Id == dancerId))
                .ToList();
        }

        public Badge? Get(Guid badgeId)
        {
            return _context
                .Badges
                .Include(b => b.Event)
                .FirstOrDefault(b => b.Id == badgeId);
        }

        public Badge? GetByName(string name)
        {
            return _context
                .Badges
                .Include(b => b.Event)
                .FirstOrDefault(b => b.Name == name);
        }

        public async Task<Badge?> Add(Badge badge)
        {
            var badgeResult = await _context.Badges.AddAsync(badge);
            return badgeResult?.Entity;
        }

        public Badge? Update(Badge badge)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid badgeId)
        {
            throw new NotImplementedException();
        }

        public bool AssignBadge(Guid badgeId, Guid dancerId)
        {
            var badge = _context.Badges.Include(b => b.Dancers).SingleOrDefault(b => b.Id == badgeId);
            var dancer = _context.Dancers.Find(dancerId);
            if (badge == null || dancer == null) return false;
            badge.Dancers.Add(dancer);
            return true;
        }

        public bool RevokeBadge(Guid badgeId, Guid dancerId)
        {
            var badge = _context.Badges.Include(b => b.Dancers).SingleOrDefault(b => b.Id == badgeId);
            var dancer = _context.Dancers.Find(dancerId);
            if (badge == null || dancer == null) return false;
            return badge.Dancers.Remove(dancer);
        }
    }
}