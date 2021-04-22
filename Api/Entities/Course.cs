using System;
using System.Collections.Generic;
using AusDdrApi.GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;

namespace AusDdrApi.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        [GraphQLType(typeof(NonNullType<ListType<NonNullType<SongDifficultyType>>>))]
        public virtual ICollection<SongDifficulty> SongDifficulties { get; set; } = new HashSet<SongDifficulty>();
    }
}