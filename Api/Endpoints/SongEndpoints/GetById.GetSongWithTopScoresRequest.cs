using System;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.SongEndpoints;

public class GetSongWithTopScoresRequest
{
    public const string Route = "/songs/{Id:guid}";
    public static string BuildRoute(Guid id) => Route.Replace("{Id:guid}", id.ToString());
        
    [FromRoute]
    public Guid Id { get; set; }
    [FromQuery]
    public bool WithTopScores { get; set; }
}