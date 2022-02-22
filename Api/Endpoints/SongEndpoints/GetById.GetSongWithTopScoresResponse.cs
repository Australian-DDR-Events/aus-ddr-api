using System;
using System.Collections.Generic;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.SongEndpoints;

public record ScoreApiResponse(int Value, Guid DancerId);

public record SongDifficultyApiResponse(Guid DifficultyId, PlayMode Mode, Difficulty Difficulty, int Level, IEnumerable<ScoreApiResponse> Scores);

public record GetSongWithTopScoresResponse(Guid SongId, string Name, string Artist, IEnumerable<SongDifficultyApiResponse> SongDifficulties);