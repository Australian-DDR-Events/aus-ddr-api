using System;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.SongEndpoints;

public class GetSongByIdRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}