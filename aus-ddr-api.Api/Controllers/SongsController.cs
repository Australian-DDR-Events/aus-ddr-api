using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Song;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ILogger<SongsController> _logger;
        private readonly ICoreData _coreService;
        private readonly ISong _songService;

        public SongsController(
            ILogger<SongsController> logger, 
            ICoreData coreService,
            ISong songService)
        {
            _logger = logger;
            _coreService = coreService;
            _songService = songService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SongResponse>> Get()
        {
            return Ok(_songService.GetAll().Select(SongResponse.FromEntity).AsEnumerable());
        }

        [HttpGet]
        [Route("{songId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SongResponse> GetSong(Guid songId)
        {
            var song = _songService.Get(songId);
            if (song == null)
            {
                return NotFound();
            }

            return Ok(SongResponse.FromEntity(song));
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SongResponse>> Post(SongRequest song)
        {
            var newSong = await _songService.Add(song.ToEntity());
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
