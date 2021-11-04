using System;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public static class InMemoryDatabaseRepository<T> where T : BaseEntity, IAggregateRoot
    {
        public static IAsyncRepository<T> CreateRepository() 
        {
            var options = new DbContextOptionsBuilder<EFDatabaseContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var ctx = new EFDatabaseContext(options);
            return new GenericEfRepository<T>(ctx);
        }
    }
}