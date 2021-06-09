using System;

namespace Application.Core.Entities
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; }
    }
}