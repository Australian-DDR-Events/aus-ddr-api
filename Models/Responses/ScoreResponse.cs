using System;

namespace AusDdrApi.Models.Responses
{
    public class ScoreResponse
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

        public string SongName { get; set; }
    }
}