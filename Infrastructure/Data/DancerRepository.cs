using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using JetBrains.Annotations;

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

    [CanBeNull]
    public Dancer GetDancerById(Guid id)
    {
        return _context
            .Dancers
            .FirstOrDefault(d => d.Id.Equals(id));
    }

    [CanBeNull]
    public Dancer GetDancerByAuthId(string id)
    {
        return _context
            .Dancers
            .FirstOrDefault(d => d.AuthenticationId.Equals(id));
    }

    public async Task CreateDancer(Dancer dancer, CancellationToken cancellationToken)
    {
        if (GetDancerByAuthId(dancer.AuthenticationId) != null) return;
        _context
            .Dancers
            .Add(dancer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDancer(Dancer dancer, CancellationToken cancellationToken)
    {
        var dbDancer = GetDancerById(dancer.Id);
        if (dbDancer == null) return;
        _context.Entry(dbDancer).CurrentValues.SetValues(dancer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}