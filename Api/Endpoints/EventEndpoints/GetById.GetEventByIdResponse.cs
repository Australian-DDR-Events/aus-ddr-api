using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.EventEndpoints;

public class GetEventByIdResponse
{
    private GetEventByIdResponse(Guid eventId, string name, string description, IEnumerable<CourseResponseFragment> courses, IEnumerable<ChartResponseFragment> charts)
    {
        EventId = eventId;
        Name = name;
        Description = description;
        Courses = courses;
        Charts = charts;
    }

    public Guid EventId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public IEnumerable<CourseResponseFragment> Courses { get; init; }
    public IEnumerable<ChartResponseFragment> Charts { get; init; }
    
    public static GetEventByIdResponse Convert(Event u)
    {
        return new GetEventByIdResponse(
            u.Id,
            u.Name,
            u.Description,
            u.Courses.Select(ConvertToCourseResponseFragment),
            u.Charts.Select(ConvertToChartResponseFragment)
        );
    }

    private static CourseResponseFragment ConvertToCourseResponseFragment(Course cr)
    {
        return new CourseResponseFragment(cr.Name, cr.Description,
            cr.Charts.Select(ConvertToChartResponseFragment));
    }

    private static ChartResponseFragment ConvertToChartResponseFragment(Chart sd)
    {
        var score = sd.Scores?.FirstOrDefault();
        ScoreResponseFragment? scoreFragment = null;
        if (score != null)
        {
            var dancerFragment = new DancerResponseFragment(
                score.Dancer.Id,
                score.Dancer.DdrName,
                score.Dancer.ProfilePictureTimestamp);
            scoreFragment = new ScoreResponseFragment(score.ExScore, dancerFragment);
        }


        return new ChartResponseFragment(sd.Id, sd.Song.Name, sd.Song.Artist, sd.Level, sd.Difficulty,
            sd.PlayMode, scoreFragment);
    }
}

public record CourseResponseFragment(string Name, string Description, IEnumerable<ChartResponseFragment> Chart);

public record ChartResponseFragment(Guid Id, string Name, string Artist, int Level, Difficulty Difficulty, PlayMode PlayMode,
    ScoreResponseFragment? Score);

public record ScoreResponseFragment(int ExScore, DancerResponseFragment Dancer);

public record DancerResponseFragment(Guid Id, string Name, DateTime? ProfilePictureTimestamp);
