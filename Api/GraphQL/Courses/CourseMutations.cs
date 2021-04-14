using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL.Common;
using AusDdrApi.GraphQL.Dancers;
using AusDdrApi.Helpers;
using AusDdrApi.Persistence;
using AusDdrApi.Services.Authorization;
using AusDdrApi.Services.FileStorage;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using SixLabors.ImageSharp;

namespace AusDdrApi.GraphQL.Courses
{
    [ExtendObjectType("Mutation")]
    public class CourseMutations
    {
        [UseDatabaseContext]
        [Authorize(Policy = "Admin")]
        public async Task<AddCoursePayload> AddCourseAsync(
            AddCourseInput input,
            [ScopedService] DatabaseContext context,
            CancellationToken cancellationToken)
        {
            var course = new Course
            {
                Name = input.Name,
                Description = input.Description
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
            [Service]IAuthorization authorization,
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