using System.Collections.Generic;
using AusDdrApi.Entities;
using AusDdrApi.GraphQL.Common;

namespace AusDdrApi.GraphQL.Courses
{
    public class AddCoursePayload : Payload
    {
        public AddCoursePayload(Course course)
        {
            Course = course;
        }
        
        public AddCoursePayload(IReadOnlyList<UserError> userErrors) : base (userErrors) {}

        public Course? Course { get; }
    }
}