using System;

namespace Application.Core.Entities;

public class Connection : BaseEntity
{
    public string ConnectionData { get; set; }
    public ConnectionType Type { get; set; }
    
    public Guid DancerId { get; set; }
    public Dancer Dancer { get; set; } = default!;
    
    public enum ConnectionType
    {
        DISCORD
    }
}