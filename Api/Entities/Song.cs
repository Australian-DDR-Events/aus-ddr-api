using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AusDdrApi.GraphQL.Types;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AusDdrApi.Entities
{
    public class Song
    {
        [IsProjected(true)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        
        [NotMapped]
        public string Image32 => $"/songs/{Id}.32.png";
        [NotMapped]
        public string Image64 => $"/songs/{Id}.64.png";
        [NotMapped]
        public string Image128 => $"/songs/{Id}.128.png";
        [NotMapped]
        public string Image256 => $"/songs/{Id}.256.png";
        [NotMapped]
        public string Image512 => $"/songs/{Id}.512.png";
        
        [GraphQLType(typeof(NonNullType<ListType<NonNullType<SongDifficultyType>>>))]
        public ICollection<SongDifficulty> SongDifficulties { get; set; } = default!;

        public override bool Equals(object? comparator)
        {
            var comparatorAsSong = comparator as Song;
            if (comparatorAsSong == null) return false;
            return Equals(comparatorAsSong);
        }

        public bool Equals(Song comparator)
        {
            return (
                Id == comparator.Id &&
                Name == comparator.Name &&
                Artist == comparator.Artist);
        }

        public override int GetHashCode()
        {
            return (Id, Name, Artist).GetHashCode();
        }
    }
}