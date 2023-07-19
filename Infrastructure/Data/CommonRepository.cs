using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;

namespace Infrastructure.Data;

public class CommonRepository<T> where T : BaseEntity
{
    private readonly EFDatabaseContext _context;
    
    public CommonRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetN<TKey>(
        int skip,
        int limit,
        Func<T, T> selector,
        bool sortType,
        Func<T, TKey> sort
    )
    {
        var set = _context.Set<T>();
        var query = sortType ? set.OrderBy(sort) : set.OrderByDescending(sort);
        return query
            .Skip(skip)
            .Take(limit)
            .Select(selector);
    }
}