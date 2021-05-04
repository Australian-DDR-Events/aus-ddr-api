using System;
using AusDdrApi.Entities;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Songs
{
    public record UpdateSongInput(
        [ID(nameof(Song))] Guid SongId,
        string Name, 
        string Artist, 
        string Difficulty,
        int Level,
        int MaxScore,
        IFile SongJacket);
}