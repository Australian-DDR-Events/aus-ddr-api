using System;

namespace Application.Core.Models.Badge;

public class GetBadgesResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? EventName { get; set; }
}