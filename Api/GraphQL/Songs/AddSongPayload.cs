using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Songs
{
    public class AddSongPayload : Payload
    {
        public AddSongPayload(Song song)
        {
            Song = song;
        }
        
        public AddSongPayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}

        public Song? Song { get; }
    }
}