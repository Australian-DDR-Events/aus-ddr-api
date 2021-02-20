using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AusDdrApi.Entities;

namespace AusDdrApi.Services.Entities.DancerService
{
    public interface IDancerService
    {
        public IEnumerable<Dancer> GetAll();
        public Dancer? Get(Guid dancerId);
        public Dancer? GetByAuthId(string authId);

        public Task<Dancer> Add(Dancer dancer);
        public Task<Dancer?> Update(Dancer dancer);


    }
}