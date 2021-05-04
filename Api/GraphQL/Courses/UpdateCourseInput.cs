using System;
using AusDdrApi.Entities;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Courses
{
    public record UpdateCourseInput(
        [ID(nameof(Course))] Guid CourseId,
        string Name, 
        string Description);
}