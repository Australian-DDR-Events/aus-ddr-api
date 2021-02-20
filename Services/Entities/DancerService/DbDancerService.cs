using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Persistence;

namespace AusDdrApi.Services.Entities.DancerService
{
    public class DbDancerService : IDancerService
    {
        private readonly DatabaseContext _context;

        public DbDancerService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Dancer> GetAll()
        {
            return _context.Dancers.ToList();
        }

        public Dancer? Get(Guid dancerId)
        {
            return _context.Dancers.AsQueryable().SingleOrDefault(d => d.Id == dancerId);
        }

        public Dancer? GetByAuthId(string authId)
        {
            return _context.Dancers.AsQueryable().SingleOrDefault(d => d.AuthenticationId == authId);
        }

        public async Task<Dancer> Add(Dancer dancer)
        {
            var newDancer = await _context.Dancers.AddAsync(dancer);
            return newDancer.Entity;
        }

        public async Task<Dancer?> Update(Dancer dancer)
        {
            return _context.Dancers.Update(dancer).Entity;
        }
    }
}