using System;

namespace AusDdrApi.Entities
{
    public class Score
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// The score that 
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Stores the date time the score is submitted.<br/>
        /// This value is generated once on create and cannot be updated.
        /// </summary>
        public DateTime SubmissionTime { get; set; }

        public Guid DancerId { get; set; }
        public Dancer? Dancer { get; set; }
        
        public Guid SongId { get; set; }
        public Song? Song { get; set; }
    }
}