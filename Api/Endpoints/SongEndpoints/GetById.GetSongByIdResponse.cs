using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.SongEndpoints;

public class GetSongByIdResponse
{
    private GetSongByIdResponse(Guid id, string name, string artist, IEnumerable<Chart> charts)
    {
        Id = id;
        Name = name;
        Artist = artist;
        Charts = charts.Select(ChartFragment.Convert);
    }
    
    public Guid Id { get; }
    public string Name { get; }
    public string Artist { get; }
    
    public IEnumerable<ChartFragment> Charts { get; }

    public class ChartFragment
    {
        private ChartFragment(Guid id, PlayMode mode, Difficulty difficulty, int level)
        {
            Id = id;
            Mode = mode;
            Difficulty = difficulty;
            Level = level;
        }
        
        public Guid Id { get; }
        public PlayMode Mode { get; }
        public Difficulty Difficulty { get; }
        public int Level { get; }

        public static ChartFragment Convert(Chart chart) =>
            new(chart.Id, chart.PlayMode, chart.Difficulty,
                chart.Level);
    }

    public static GetSongByIdResponse Convert(Song song) =>
        new(song.Id, song.Name, song.Artist, song.Charts);
}
