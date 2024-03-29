using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;

namespace Infrastructure.Data;

public class EventRepository : IEventRepository
{
    private readonly EFDatabaseContext _context;
    
    public EventRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public IEnumerable<Event> GetEvents()
    {
        return _context
            .Events
            .Select(e => new Event
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            })
            .OrderByDescending(e => e.StartDate)
            .ToList();
    }

    public Event GetEventWithTopScore(Guid eventId)
    {
        return _context
            .Events
            .Where(e => e.Id.Equals(eventId))
            .Select(e => new Event
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Courses = e.Courses.Select(course => new Course
                {
                    Name = course.Name,
                    Description = course.Description,
                    Charts = course.Charts.Select(chart => new Chart
                    {
                        Song = new Song
                        {
                            Name = chart.Song.Name,
                            Artist = chart.Song.Artist
                        },
                        Id = chart.Id,
                        Level = chart.Level,
                        Difficulty = chart.Difficulty,
                        PlayMode = chart.PlayMode,
                        // Scores = chart.Scores.OrderByDescending(s => new { s.ExScore, s.SubmissionTime }).Take(3).Select(score => new Score
                        // {
                        //     ExScore = score.ExScore,
                        //     Dancer = new Dancer
                        //     {
                        //         DdrName = score.Dancer.DdrName,
                        //         Id = score.Dancer.Id,
                        //         ProfilePictureTimestamp = score.Dancer.ProfilePictureTimestamp
                        //     }
                        // }).ToList()
                    }).ToList()
                }).ToList(),
                Charts = e.Charts.Select(chart => new Chart
                {
                    Song = new Song
                    {
                        Name = chart.Song.Name,
                        Artist = chart.Song.Artist
                    },
                    Id = chart.Id,
                    Level = chart.Level,
                    Difficulty = chart.Difficulty,
                    PlayMode = chart.PlayMode,
                    // Scores = chart.Scores.OrderByDescending(s => new { s.ExScore, s.SubmissionTime }).Take(3).Select(score => new Score
                    // {
                    //     ExScore = score.ExScore,
                    //     Dancer = new Dancer
                    //     {
                    //         DdrName = score.Dancer.DdrName,
                    //         Id = score.Dancer.Id,
                    //         ProfilePictureTimestamp = score.Dancer.ProfilePictureTimestamp
                    //     }
                    // }).ToList()
                }).ToList()
            })
            .FirstOrDefault();
    }

    public IEnumerable<Course> GetEventCourses(Guid eventId)
    {
        return _context
            .Events
            .Where(e => e.Id.Equals(eventId))
            .Select(e => e.Courses.Select(course => new Course {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description
            }))
            .FirstOrDefault() ?? new List<Course>();
    }

    public IEnumerable<Chart> GetEventSongs(Guid eventId)
    {
        return _context
            .Events
            .Where(e => e.Id.Equals(eventId))
            .Select(e => e.Charts.Select(sd => new Chart {
                Id = sd.Id,
                PlayMode = sd.PlayMode,
                Difficulty = sd.Difficulty,
                Level = sd.Level,
                MaxScore = sd.MaxScore,
                Song = new Song
                {
                    Name = sd.Song.Name,
                    Artist = sd.Song.Artist
                }
            }).ToList())
            .FirstOrDefault() ?? new List<Chart>();
    }
}