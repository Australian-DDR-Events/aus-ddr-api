using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
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
        public IAsyncEnumerable<Song> Get()
        {
            return _context.Songs.AsAsyncEnumerable();
        }
        
        [HttpPost]
        public async Task<Song> Post(SongRequest song)
        {
            var newSong = await _context.Songs.AddAsync(song.ToSong());
            await _context.SaveChangesAsync();
            return newSong.Entity;
        }

        [HttpPut]
        [Route("~/songs/{songId}")]
        public async Task<Song> Put(Guid songId, SongRequest songRequest)
        {
            var song = songRequest.ToSong();
            song.Id = songId;
            var newSong = _context.Songs.Update(song);
            await _context.SaveChangesAsync();
            return newSong.Entity;
        }

        [HttpDelete]
        [Route("~/songs/{songId}")]
        public async Task<ActionResult> Delete(Guid songId)
        {
            var song = await _context.Songs.FindAsync(songId);
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}