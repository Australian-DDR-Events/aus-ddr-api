using System;

namespace AusDdrApi.Entities
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string ImageUrl { get; set; }
        public string Difficulty { get; set; }
        public int Level { get; set; }
    }
}