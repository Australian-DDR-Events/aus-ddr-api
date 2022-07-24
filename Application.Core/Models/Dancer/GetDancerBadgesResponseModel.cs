using System;

namespace Application.Core.Models.Dancer;

public class GetDancerBadgesResponseModel
{
    public Guid Id;
    public string Name;
    public string Description;
    public string? EventName;
}