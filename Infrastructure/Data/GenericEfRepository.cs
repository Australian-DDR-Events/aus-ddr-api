using Application.Core.Entities;
using Application.Core.Interface;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericEfRepository<T> : RepositoryBase<T>, IAsyncRepository<T> where T : BaseEntity
    {
        public GenericEfRepository(EFDatabaseContext dbContext) : base(dbContext)
        {
        }
    }
}