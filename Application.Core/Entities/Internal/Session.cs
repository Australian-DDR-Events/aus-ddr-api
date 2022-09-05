using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Core.Entities.Internal;

public class Session
{
    [Key]
    public string Cookie { get; set; }

    public string RefreshToken { get; set; }
    
    public DateTime Expiry { get; set; }

    public Guid DancerId { get; set; }
    public Dancer Dancer { get; set; } = default!;
}