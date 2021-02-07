using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ILogger<SongsController> _logger;
        private DatabaseContext _context;

        public SongsController(ILogger<SongsController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<SongResponse> Get()
        {
            return _context.Songs.Select(SongResponse.FromEntity);
        }

        [HttpGet]
        [Route("~/songs/{songId}")]
        public ActionResult<SongResponse> GetSong(Guid songId)
        {
            var song = _context.Songs.AsQueryable().SingleOrDefault(song => song.Id == songId);
            if (song == null)
            {
                return NotFound();
            }

            return SongResponse.FromEntity(song);
        }
        
        /*[HttpPost]
        public async Task<SongResponse> Post(SongRequest song)
        {
            var newSong = await _context.Songs.AddAsync(song.ToEntity());
            await _context.SaveChangesAsync();
            return SongResponse.FromEntity(newSong.Entity);
        }

        [HttpPut]
        [Route("~/songs/{songId}")]
        public async Task<SongResponse> Put(Guid songId, SongRequest songRequest)
        {
            var song = songRequest.ToEntity();
            song.Id = songId;
            var updatedSong = _context.Songs.Update(song);
            await _context.SaveChangesAsync();
            return SongResponse.FromEntity(updatedSong.Entity);
        }

        [HttpDelete]
        [Route("~/songs/{songId}")]
        public async Task<ActionResult> Delete(Guid songId)
        {
            var song = await _context.Songs.FindAsync(songId);
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return Ok();
        }*/
    }
}