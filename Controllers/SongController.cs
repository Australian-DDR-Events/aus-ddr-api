using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AusDdrApi.Models;
using AusDdrApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongController
    {
        private readonly ILogger<SongController> _logger;
        private DatabaseContext _context;

        public SongController(ILogger<SongController> logger, DatabaseContext context)
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
        public async Task<Song> Post(Song song)
        {
            song.Id = Guid.Empty;
            var newSong = await _context.Songs.AddAsync(song);
            await _context.SaveChangesAsync();
            return newSong.Entity;
        }
    }
}