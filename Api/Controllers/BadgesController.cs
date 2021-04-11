using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Helpers;
using AusDdrApi.Models.Requests;
using AusDdrApi.Models.Responses;
using AusDdrApi.Services.Badges;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace AusDdrApi.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class BadgesController : ControllerBase
    {
        private readonly ILogger<BadgesController> _logger;
        private readonly ICoreData _coreService;
        private readonly IDancerService _dancerService;
        private readonly IFileStorage _fileStorage;
        private readonly IBadge _badgeService;

        public BadgesController(
            ILogger<BadgesController> logger,
            ICoreData coreService,
            IDancerService dancerService,
            IFileStorage fileStorage,
            IBadge badgeService)
        {
            _logger = logger;
            _coreService = coreService;
            _dancerService = dancerService;
            _fileStorage = fileStorage;
            _badgeService = badgeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BadgeResponse>> Get([FromQuery(Name = "dancer_id")] Guid? dancerId)
        {
            var badges = dancerId != null ? _badgeService.GetAssigned(dancerId.Value) : _badgeService.GetAll();
            return Ok(badges.Select(BadgeResponse.FromEntity));
        }

        [HttpGet]
        [Route("{badgeId}")]
        public ActionResult<BadgeResponse> GetBadge(Guid badgeId)
        {
            var badge = _badgeService.Get(badgeId);
            if (badge == null) return NotFound();
            return Ok(BadgeResponse.FromEntity(badge));
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<BadgeResponse>> Post([FromForm] BadgeRequest badgeRequest)
        {
            var newBadge = await _badgeService.Add(badgeRequest.ToEntity());
            if (newBadge == null) return BadRequest();
            
            try
            {
                int[] imageSizes = {32, 64, 128, 256};
                using var ingredientImage = await Image.LoadAsync(badgeRequest.BadgeImage!.OpenReadStream());
                foreach (var size in imageSizes)
                {
                    var image = await Images.ImageToPngMemoryStream(ingredientImage, size, size);

                    var destinationKey = $"badges/{newBadge.Id}.{size}.png";
                    await _fileStorage.UploadFileFromStream(image, destinationKey);
                }
            }
            catch
            {
                return BadRequest();
            }
            
            await _coreService.SaveChanges();
            return Ok(BadgeResponse.FromEntity(newBadge));
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [Route("{badgeId}/dancers/{dancerId}")]
        public async Task<ActionResult> AssignBadge([FromRoute] Guid badgeId, [FromRoute] Guid dancerId)
        {
            if (!_badgeService.AssignBadge(badgeId, dancerId)) return BadRequest();
            await _coreService.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Authorize(Policy = "Admin")]
        [Route("{badgeId}/dancers/{dancerId}")]
        public async Task<ActionResult> RevokeBadge([FromRoute] Guid badgeId, [FromRoute] Guid dancerId)
        {
            if (!_badgeService.RevokeBadge(badgeId, dancerId)) return BadRequest();
            await _coreService.SaveChanges();
            return Ok();
        }
    }
}