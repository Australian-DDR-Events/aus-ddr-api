using System;

namespace AusDdrApi.GraphQL.Courses
{
    public record AddCourseInput(
        string Name, 
        string Description,
        Guid[] Songs);
}