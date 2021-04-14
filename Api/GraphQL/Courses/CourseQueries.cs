using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace AusDdrApi.GraphQL.Courses
{
    [ExtendObjectType("Query")]
    public class CourseQueries
    {
        [UseDatabaseContext]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Course> GetCourses([ScopedService] DatabaseContext context) => context.Courses;

        public Task<Course?> GetCourseByIdAsync(
            [ID(nameof(Course))] Guid id,
            CourseByIdDataLoader courseByIdDataLoader,
            CancellationToken cancellationToken) => courseByIdDataLoader.LoadAsync(id, cancellationToken)!;

        public async Task<IEnumerable<Course>> GetCoursesByIdAsync(
            [ID(nameof(Course))] Guid[] ids,
            CourseByIdDataLoader courseByIdDataLoader,
            CancellationToken cancellationToken) => await courseByIdDataLoader.LoadAsync(ids, cancellationToken) 
                                                    ?? Array.Empty<Course>();
    }
}