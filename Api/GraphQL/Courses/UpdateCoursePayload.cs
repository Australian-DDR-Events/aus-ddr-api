using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Courses
{
    public class UpdateCoursePayload : Payload
    {
        public UpdateCoursePayload(Course course)
        {
            Course = course;
        }
        
        public UpdateCoursePayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}
        
        public Course? Course { get; }
    }
}