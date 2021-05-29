using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Songs
{
    public record AddSongInput(
        string Name, 
        string Artist,
        IFile SongJacket);
}