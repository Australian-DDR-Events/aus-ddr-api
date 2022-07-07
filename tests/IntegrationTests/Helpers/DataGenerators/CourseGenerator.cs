using System;
using System.Collections.Generic;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class CourseGenerator
{
    public static Course CreateCourse() => new Course
    {
        Id = Guid.NewGuid()
    };

    public static Course CreateCourse(ICollection<SongDifficulty> songDifficulties) => new Course
    {
        Id = Guid.NewGuid(),
        SongDifficulties = songDifficulties
    };
}