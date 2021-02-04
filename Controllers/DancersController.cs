using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using AusDdrApi.Authentication;
using AusDdrApi.Context;
using AusDdrApi.Entities;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
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
        private readonly IAmazonS3 _s3Client;

        public DancersController(ILogger<DancersController> logger, DatabaseContext context, IAmazonS3 s3Client)
        {
            _logger = logger;
            _context = context;
            _s3Client = s3Client;
        }

        [HttpGet]
        public IEnumerable<DancerResponse> Get()
        {
            return _context.Dancers.Select(DancerResponse.FromDancer).ToArray();
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

            return DancerResponse.FromDancer(dancer);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Dancer>> Post(DancerRequest dancerRequest)
        {
            var existingDancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == HttpContext.GetUserId());
            if (existingDancer != null)
            {
                return Conflict();
            }
            var dancer = dancerRequest.ToDancer();
            dancer.AuthenticationId = HttpContext.GetUserId();
            var newDancer = await _context.Dancers.AddAsync(dancer);
            await _context.SaveChangesAsync();
            return newDancer.Entity;
        }

        [HttpPost]
        [Route("~/dancers/profilepicture")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PostProfilePicture(IFormFile ProfilePicture)
        {
            var authenticationId = HttpContext.GetUserId();
            var existingDancer = _context.Dancers.AsQueryable().SingleOrDefault(dancer => dancer.AuthenticationId == authenticationId);
            if (existingDancer == null)
            {
                return NotFound();
            }

            using var image = await Image.LoadAsync(ProfilePicture.OpenReadStream());
            image.Mutate(x => x.Resize(256, 256));

            try
            {
                await using var newMemoryStream = new MemoryStream();
                image.SaveAsync(newMemoryStream, new PngEncoder(), CancellationToken.None);

                var extension = ProfilePicture.FileName.Substring(ProfilePicture.FileName.LastIndexOf('.'));

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = $"Profile/Picture/{authenticationId}{extension}",
                    BucketName = HttpContext.GetAWSConfiguration().AssetsBucketName,
                    CannedACL = S3CannedACL.PublicRead,
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);
                existingDancer.ProfilePictureUrl =
                    $"https://{HttpContext.GetAWSConfiguration().AssetsBucketName}.s3-{HttpContext.GetAWSConfiguration().AssetsBucketLocation}.amazonaws.com/Profile/Picture/{authenticationId}{extension}";
                
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
        public async Task<ActionResult<Dancer>> Put(Guid dancerId, DancerRequest dancerRequest)
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
            var dancer = dancerRequest.ToDancer();
            dancer.Id = dancerId;
            dancer.AuthenticationId = HttpContext.GetUserId();
            var newDancer = _context.Dancers.Update(dancer);
            await _context.SaveChangesAsync();
            return Ok(newDancer.Entity);
        }
    }
}