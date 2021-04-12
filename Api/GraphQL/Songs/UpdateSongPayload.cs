using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Songs
{
    public class UpdateSongPayload : Payload
    {
        public UpdateSongPayload(Song song)
        {
            Song = song;
        }
        
        public UpdateSongPayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}
        
        public Song? Song { get; }
    }
}