using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DancerEntity = AusDdrApi.Entities.Dancer;

namespace AusDdrApi.Services.Dancer
{
    public interface IDancer
    {
        public IEnumerable<DancerEntity> GetAll();
        public DancerEntity? Get(Guid dancerId);
        public DancerEntity? GetByAuthId(string authId);

        public Task<DancerEntity> Add(DancerEntity dancer);
        public DancerEntity? Update(DancerEntity dancer);
    }
}