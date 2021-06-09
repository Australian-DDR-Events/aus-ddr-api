using System;

namespace Api.Core.Entities
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; }
    }
}