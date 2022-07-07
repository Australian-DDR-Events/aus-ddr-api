using System.Collections.Generic;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IDancerRepository
{
    IEnumerable<Dancer> GetDancers(int skip, int limit);
}
