using Application.Core.Entities;
using Ardalis.Specification;

namespace Application.Core.Specifications.SongSpecs
{
    public class List : PageableSpec<Song>
    {
        public List(int skip, int limit) : base(skip, limit)
        {
            Query
                .Include(s => s.SongDifficulties);
        }
    }
}