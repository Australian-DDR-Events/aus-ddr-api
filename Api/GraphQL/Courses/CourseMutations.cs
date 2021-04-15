using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.Common;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.Persistence;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace AusDdrApi.GraphQL.Courses
{
    [ExtendObjectType("Mutation")]
    public class CourseMutations
    {
        [UseDatabaseContext]
        public async Task<AddCoursePayload> AddCourseAsync(
            AddCourseInput input,
            [ScopedService] DatabaseContext context,
            [ScopedService] SongByIdDataLoader songByIdDataLoader,
            CancellationToken cancellationToken)
        {
            var songs = context
                .Songs
                .Where(s => input.Songs.Contains(s.Id))
                .ToImmutableList();
            var course = new Course
            {
                Name = input.Name,
                Description = input.Description,
                Songs = songs
            };

            var courseEntity = await context.Courses.AddAsync(course, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return new AddCoursePayload(courseEntity.Entity);
        }

        [UseDatabaseContext]
        [Authorize(Policy = "Admin")]
        public async Task<UpdateCoursePayload> UpdateCourseAsync(
            UpdateCourseInput input,
            [ScopedService] DatabaseContext context,
            CancellationToken cancellationToken)
        {
            var course = await context.Courses.FindAsync(new object[]{input.CourseId}, cancellationToken);

            if (course is null)
            {
                return new UpdateCoursePayload(
                    new []
                    {
                        new UserError("Course not found.", CourseErrorCodes.COURSE_NOT_FOUND)
                    });
            }

            course.Name = input.Name;
            course.Description = input.Description;

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateCoursePayload(course);
        }
    }
}