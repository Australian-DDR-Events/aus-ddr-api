using System;
using Application.Core.Entities;

namespace Application.Core.Models.SongDifficulties;

public class CreateSongDifficultyRequestModel
{
    public Guid SongId { get; set; }
    public Difficulty Difficulty { get; set; }
    public PlayMode Mode { get; set; }
    public int Level { get; set; }
    public int MaxScore { get; set; }
}
