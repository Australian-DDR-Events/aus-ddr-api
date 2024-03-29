using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using AusDdrApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.AdminEndpoints;

[ApiController]
public class ImageUploader : ControllerBase
{
    private readonly IAdminService _adminService;
    
    public ImageUploader(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost(UploadImageRequest.Route)]
    [SwaggerOperation(
        Summary = "Upload an image to the data storage",
        Description = "Uploads a given image to the data store location using the provided sizes",
        OperationId = "Admin.Image.Upload",
        Tags = new[] { "Admin", "Image" })
    ]
    [Authorize]
    [Admin]
    public async Task<ActionResult> HandleAsync([FromForm] UploadImageRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (request.Image == null)
        {
            return new BadRequestResult();
        }

        if (request.FileSizesX.Count != request.FileSizesY.Count)
        {
            return new BadRequestResult();
        }

        var joinedSizes = request.FileSizesX.Zip(request.FileSizesY).Select(tuple => new Tuple<int, int>(tuple.First, tuple.Second)).ToList();

        var result = await _adminService.UploadImage(request.FileName, request.Image.OpenReadStream(), joinedSizes, cancellationToken);
        return result.ResultCode switch
        {
            ResultCode.Ok => Accepted(),
            ResultCode.BadRequest => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}