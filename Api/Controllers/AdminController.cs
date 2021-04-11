using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Helpers;
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
    [Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IFileStorage _fileStorage;

        public AdminController(ILogger<AdminController> logger, IFileStorage fileStorage)
        {
            _logger = logger;
            _fileStorage = fileStorage;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult> UploadImage(
            [FromForm] IFormFile formImage,
            [FromForm] string fileName,
            [FromQuery(Name = "image_size")] IEnumerable<string> imageSizes)
        {
            if (!imageSizes.Any()) return BadRequest("no sizes provided");
            var squareSizes = new List<int>(imageSizes.Select(int.Parse));

            using var dishImage = await Image.LoadAsync(formImage.OpenReadStream());
            foreach (var squareSize in squareSizes)
            {
                var image = await Images.ImageToPngMemoryStream(dishImage, squareSize, squareSize);

                var destinationKey = $"{fileName}.{squareSize}.png";
                await _fileStorage.UploadFileFromStream(image, destinationKey);
            }

            return Ok();
        }
    }
}