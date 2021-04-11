using System;
using System.Linq;
using System.Threading.Tasks;
using DancerEntity = AusDdrApi.Entities.Dancer;
using AusDdrApi.Persistence;

namespace AusDdrApi.Services.Dancer
{
    public class DancerService : IDancerService
    {
        private readonly DatabaseContext _context;

        public DancerService(DatabaseContext context)
        {
            _context = context;
        }

        public IQueryable<AusDdrApi.Entities.Dancer> GetAll()
        {
            return _context.Dancers;
        }

        public DancerEntity? Get(Guid dancerId)
        {
            return _context.Dancers.AsQueryable().SingleOrDefault(d => d.Id == dancerId);
        }

        public DancerEntity? GetByAuthId(string authId)
        {
            return _context.Dancers.AsQueryable().SingleOrDefault(d => d.AuthenticationId == authId);
        }

        public async Task<DancerEntity> Add(DancerEntity dancer)
        {
            var newDancer = await _context.Dancers.AddAsync(dancer);
            return newDancer.Entity;
        }

        public DancerEntity? Update(DancerEntity dancer)
        {
            return _context.Dancers.Update(dancer).Entity;
        }
    }
}