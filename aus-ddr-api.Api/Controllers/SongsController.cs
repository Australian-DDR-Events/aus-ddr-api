using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Helpers;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.FileStorage;
using AusDdrApi.Services.Song;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace AusDdrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ILogger<SongsController> _logger;
        private readonly ICoreData _coreService;
        private readonly IFileStorage _fileStorage;
        private readonly ISong _songService;

        public SongsController(
            ILogger<SongsController> logger, 
            ICoreData coreService,
            IFileStorage fileStorage,
            ISong songService)
        {
            _logger = logger;
            _coreService = coreService;
            _fileStorage = fileStorage;
            _songService = songService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SongResponse>> Get(
            [FromQuery(Name = "song_id")] Guid[]? songIds
            )
        {
            var songs = songIds?.Length > 0 ? _songService.Get(songIds) : _songService.GetAll();
            return Ok(songs.Select(SongResponse.FromEntity).AsEnumerable());
        }

        [HttpGet]
        [Route("{songId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SongResponse> GetSong(Guid songId)
        {
            var song = _songService.Get(songId);
            if (song == null) return NotFound();

            return Ok(SongResponse.FromEntity(song));
        }
        
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SongResponse>> Post([FromForm] SongRequest songRequest)
        {
            var newSong = await _songService.Add(songRequest.ToEntity());

            if (songRequest.SongJacket != null)
            {
                var uploadTasks = new List<Task<string>>();
                try
                {
                    int[] imageSizes = {32, 64, 128, 256, 512};
                    using var ingredientImage = await Image.LoadAsync(songRequest.SongJacket!.OpenReadStream());
                    foreach (var size in imageSizes)
                    {
                        var image = await Images.ImageToPngMemoryStream(ingredientImage, size, size);

                        var destinationKey = $"songs/{newSong.Id}.{size}.png";
                        uploadTasks.Add(_fileStorage.UploadFileFromStream(image, destinationKey));
                    }
                }
                catch
                {
                    return BadRequest();
                }
                
                var t = Task.WhenAll(uploadTasks);
                try
                {
                    t.Wait();
                }
                catch
                {
                    return BadRequest();
                }
            }
            
            await _coreService.SaveChanges();
            return Created($"/songs/{newSong.Id}", SongResponse.FromEntity(newSong));
        }

        [HttpPut]
        [Route("{songId}")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SongResponse>> Put(Guid songId, SongRequest songRequest)
        {
            var song = songRequest.ToEntity();
            song.Id = songId;
            var updatedSong = _songService.Update(song);
            if (updatedSong == null)
            {
                return NotFound($"song {songId} does not exist");
            }
            await _coreService.SaveChanges();
            return Ok(SongResponse.FromEntity(updatedSong));
        }

        [HttpDelete]
        [Route("{songId}")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid songId)
        {
            if (!_songService.Delete(songId))
            {
                return NotFound();
            }

            await _coreService.SaveChanges();
            return Ok();
        }
    }
}
