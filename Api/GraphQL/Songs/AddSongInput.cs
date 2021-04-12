using HotChocolate.Types;

namespace AusDdrApi.GraphQL
{
    public record AddSongInput(
        string Name, 
        string Artist, 
        string Difficulty, 
        int Level,
        int MaxScore,
        IFile SongJacket);
}