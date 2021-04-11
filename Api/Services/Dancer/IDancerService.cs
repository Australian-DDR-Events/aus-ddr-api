using System;
using System.Linq;
using System.Threading.Tasks;
using DancerEntity = AusDdrApi.Entities.Dancer;

namespace AusDdrApi.Services.Dancer
{
    public interface IDancerService
    {
        public IQueryable<DancerEntity> GetAll();
        public DancerEntity? Get(Guid dancerId);
        public DancerEntity? GetByAuthId(string authId);

        public Task<DancerEntity> Add(DancerEntity dancer);
        public DancerEntity? Update(DancerEntity dancer);
    }
}