using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AusDdrApi.GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;

namespace AusDdrApi.Entities
{
    public class Badge
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public Guid EventId { get; set; }
        public Event? Event { get; set; }
        
        [NotMapped]
        public string Image32 => $"/badges/{Id}.32.png";
        [NotMapped]
        public string Image64 => $"/badges/{Id}.64.png";
        [NotMapped]
        public string Image128 => $"/badges/{Id}.128.png";
        [NotMapped]
        public string Image256 => $"/badges/{Id}.256.png";
        [NotMapped]
        public string Image512 => $"/badges/{Id}.512.png";

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<DancerType>>>))]
        public virtual ICollection<Dancer> Dancers { get; set; } = new HashSet<Dancer>();
    }
}