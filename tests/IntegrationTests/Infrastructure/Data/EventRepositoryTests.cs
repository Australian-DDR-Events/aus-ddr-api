using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Postgres database collection")]
public class EventRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly EventRepository _eventRepository;

    public EventRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _eventRepository = new EventRepository(_fixture._context);
        Setup.DropAllRows(_fixture._context);
    }

    #region GetEvents

    [Fact(DisplayName = "Returns all events from table")]
    public void ReturnAllEventsFromTable()
    {
        var events = new List<Event> {EventGenerator.CreateEvent(), EventGenerator.CreateEvent()};
        events.ForEach(e => _fixture._context.Events.Add(e));
        // events.Select(_fixture._context.Events.Add);
        _fixture._context.SaveChanges();

        var result = _eventRepository.GetEvents();
        Assert.Equal(events.OrderByDescending(e => e.StartDate).Select(e => e.Id), result.Select(e => e.Id));
    }

    #endregion

    #region GetEventCourses

    [Fact(DisplayName = "When event is not found, return an empty list")]
    public void WhenEventNotFound_ReturnAnEmptyList()
    {
        var result = _eventRepository.GetEventCourses(Guid.NewGuid());
        Assert.Empty(result);
    }

    [Fact(DisplayName = "When event is found, and event contains no courses, return an empty list")]
    public void WhenEventFound_AndNoCourses_ThenReturnEmptyList()
    {
        var e = EventGenerator.CreateEvent();
        _fixture._context.Events.Add(e);
        _fixture._context.SaveChanges();
        
        var result = _eventRepository.GetEventCourses(e.Id);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "When event is found, and has associated courses, relevant courses are returned")]
    public void WhenEventFound_AndCourses_ThenReturnCourses()
    {
        var e = EventGenerator.CreateEvent();
        e.Courses = new List<Course>{CourseGenerator.CreateCourse(), CourseGenerator.CreateCourse()};
        _fixture._context.Events.Add(e);
        _fixture._context.SaveChanges();
        
        var result = _eventRepository.GetEventCourses(e.Id);
        Assert.Equal(e.Courses.Select(c => c.Id).OrderBy(id => id), result.Select(c => c.Id).OrderBy(id => id));
    }

    #endregion

    #region GetEventSongs

    [Fact(DisplayName = "When event is not found, then return empty list")]
    public void WhenEventNotFound_ReturnEmptyList()
    {
        var result = _eventRepository.GetEventSongs(Guid.NewGuid());
        Assert.Empty(result);
    }

    [Fact(DisplayName = "When event found, but contains no songs, then return empty list")]
    public void WhenEventFound_NoSongs_EmptyList()
    {
        var e = EventGenerator.CreateEvent();
        _fixture._context.Events.Add(e);
        _fixture._context.SaveChanges();
        
        var result = _eventRepository.GetEventSongs(e.Id);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "When event found, with songs, then return songs")]
    public void WhenEventFound_HasSongs_ReturnSongs()
    {
        var song1 = SongDifficultyGenerator.CreateSongDifficulty(SongGenerator.CreateSong());
        var song2 = SongDifficultyGenerator.CreateSongDifficulty(SongGenerator.CreateSong());
        
        var e = EventGenerator.CreateEvent();
        e.SongDifficulties = new List<SongDifficulty> {song1, song2};
        _fixture._context.Events.Add(e);
        _fixture._context.SaveChanges();
        
        var result = _eventRepository.GetEventSongs(e.Id);
        Assert.Equal(e.SongDifficulties.Select(sd => sd.Id).OrderBy(id => id), result.Select(sd => sd.Id).OrderBy(id => id));
    }

    #endregion
}
