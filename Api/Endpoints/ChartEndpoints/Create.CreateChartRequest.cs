using System;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.ChartEndpoints;

public class CreateChartRequest
{
    [FromRoute]
    public Guid SongId { get; set; }
    [FromBody]
    public string Mode { get; set; }
    [FromBody]
    public string Difficulty { get; set; }
    [FromBody]
    public int Level { get; set; }
    [FromBody]
    public int MaxScore { get; set; }
}
