using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Helpers;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.Authorization;
using AusDdrApi.Services.Badges;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace AusDdrApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class DancersController : ControllerBase
    {
        private readonly ILogger<DancersController> _logger;
        private readonly ICoreData _coreService;
        private readonly IDancer _dancerService;
        private readonly IBadge _badgeService;
        private readonly IFileStorage _fileStorage;
        private readonly IAuthorization _authorizationService;

        public DancersController(
            ILogger<DancersController> logger,
            ICoreData coreService,
            IDancer dancerService,
            IBadge badgeService,
            IFileStorage fileStorage,
            IAuthorization authorizationService)
        {
            _logger = logger;
            _coreService = coreService;
            _dancerService = dancerService;
            _badgeService = badgeService;
            _fileStorage = fileStorage;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DancerResponse>> Get()
        {
            return Ok(_dancerService.GetAll().Select(DancerResponse.FromEntity));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DancerResponse> GetDancer(string id)
        {
            var dancer = Guid.TryParse(id, out var dancerId) ? _dancerService.Get(dancerId) : _dancerService.GetByAuthId(id);
            if (dancer == null)
            {
                return NotFound();
            }

            return Ok(DancerResponse.FromEntity(dancer));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<DancerResponse>> Post(DancerRequest dancerRequest)
        {
            var authId = _authorizationService.GetUserId();
            if (authId == null) return Unauthorized();
            var existingDancer = _dancerService.GetByAuthId(authId);
            if (existingDancer != null)
            {
                return Conflict();
            }
            var dancer = dancerRequest.ToEntity();
            dancer.AuthenticationId = authId;
            var newDancer = await _dancerService.Add(dancer);
            var baseBadge = _badgeService.GetByName("Base");
            if (baseBadge != null) _badgeService.AssignBadge(baseBadge.Id, newDancer.Id);
            await _coreService.SaveChanges();
            return Created($"/dancers/{newDancer.Id}", DancerResponse.FromEntity(newDancer));
        }

        [HttpPost]
        [Route("profilepicture")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PostProfilePicture(IFormFile profilePicture)
        {
            var authId = _authorizationService.GetUserId();
            if (authId == null) return Unauthorized();
            var existingDancer = _dancerService.GetByAuthId(authId);
            if (existingDancer == null)
            {
                return NotFound();
            }

            try
            {
                using var profileImage = await Image.LoadAsync(profilePicture.OpenReadStream());
                var image = await Images.ImageToPngMemoryStream(profileImage, 256, 256);
                
                var destinationKey = $"profile/picture/{authId}.png";
                await _fileStorage.UploadFileFromStream(image, destinationKey);
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
            var authId = _authorizationService.GetUserId();
            if (authId == null) return Unauthorized();
            var existingDancer = _dancerService.Get(dancerId);
            if (existingDancer == null)
            {
                return NotFound();
            }
            if (existingDancer.AuthenticationId != authId)
            {
                return Unauthorized();
            }

            existingDancer.State = dancerRequest.State;
            existingDancer.DdrCode = dancerRequest.DdrCode;
            existingDancer.DdrName = dancerRequest.DdrName;
            existingDancer.PrimaryMachineLocation = dancerRequest.PrimaryMachineLocation;

            var newDancer = _dancerService.Update(existingDancer);
            if (newDancer == null)
            {
                return BadRequest();
            }
            await _coreService.SaveChanges();
            return Ok(DancerResponse.FromEntity(newDancer));
        }
    }
}
