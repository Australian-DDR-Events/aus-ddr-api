using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;

namespace Infrastructure.Data;

public class DancerRepository : IDancerRepository
{
    private readonly EFDatabaseContext _context;

    public DancerRepository(EFDatabaseContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Dancer> GetDancers(int skip, int limit)
    {
        return _context
            .Dancers
            .OrderBy(d => d.DdrName)
            .Skip(skip)
            .Take(limit)
            .Select(d => new Dancer
            {
                Id = d.Id,
                DdrName = d.DdrName,
                ProfilePictureTimestamp = d.ProfilePictureTimestamp
            })
            .ToList();
    }
}