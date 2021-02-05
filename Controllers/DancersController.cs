using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AusDdrApi.Authentication;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace AusDdrApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class DancersController : ControllerBase
    {
        private readonly ILogger<DancersController> _logger;
        private readonly DatabaseContext _context;
        private readonly IFileStorage _fileStorage;

        public DancersController(ILogger<DancersController> logger, DatabaseContext context, IFileStorage fileStorage)
        {
            _logger = logger;
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public IEnumerable<DancerResponse> Get()
        {
            return _context.Dancers.Select(DancerResponse.FromEntity).ToArray();
        }

        [HttpGet("{authId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DancerResponse> GetDancer(string authId)
        {
            var dancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == authId);
            if (dancer == null)
            {
                return NotFound();
            }

            return DancerResponse.FromEntity(dancer);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<DancerResponse>> Post(DancerRequest dancerRequest)
        {
            var existingDancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == HttpContext.GetUserId());
            if (existingDancer != null)
            {
                return Conflict();
            }
            var dancer = dancerRequest.ToEntity();
            dancer.AuthenticationId = HttpContext.GetUserId();
            var newDancer = await _context.Dancers.AddAsync(dancer);
            await _context.SaveChangesAsync();
            return DancerResponse.FromEntity(newDancer.Entity);
        }

        [HttpPost]
        [Route("~/dancers/profilepicture")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PostProfilePicture(IFormFile profilePicture)
        {
            var authenticationId = HttpContext.GetUserId();
            var existingDancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == authenticationId);
            if (existingDancer == null)
            {
                return NotFound();
            }

            try
            {
                using var image = await Image.LoadAsync(profilePicture.OpenReadStream());
                image.Mutate(x => x.Resize(256, 256));
                
                await using var newMemoryStream = new MemoryStream();
                await image.SaveAsync(newMemoryStream, new PngEncoder(), CancellationToken.None);

                var destinationKey = $"Profile/Picture/{authenticationId}.png";
                var imageUrl = await _fileStorage.UploadFileFromStream(new MemoryStream(), destinationKey);

                existingDancer.ProfilePictureUrl = imageUrl;
                _context.Dancers.Update(existingDancer);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("{dancerId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DancerResponse>> Put(Guid dancerId, DancerRequest dancerRequest)
        {
            var existingDancer = await _context.Dancers.FindAsync(dancerId);
            if (existingDancer == null)
            {
                return NotFound();
            }
            if (existingDancer.AuthenticationId != HttpContext.GetUserId())
            {
                return Unauthorized();
            }

            existingDancer.State = dancerRequest.State;
            existingDancer.DdrCode = dancerRequest.DdrCode;
            existingDancer.DdrName = dancerRequest.DdrName;
            existingDancer.PrimaryMachineLocation = dancerRequest.PrimaryMachineLocation;
            existingDancer.ProfilePictureUrl = dancerRequest.ProfilePictureUrl;
            
            var newDancer = _context.Dancers.Update(existingDancer);
            await _context.SaveChangesAsync();
            return Ok(DancerResponse.FromEntity(newDancer.Entity));
        }
    }
}