using System;
using AusDdrApi.Entities;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Courses
{
    public record AddCourseInput(
        string Name, 
        string Description,
        [ID(nameof(Song))] Guid[] Songs);
}