using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Endpoints.AdminEndpoints;

public class UploadImageRequest
{
    public const string Route = "/admin/image";
    
    [Required]
    public IFormFile? Image { get; set; }
    [Required]
    public string FileName { get; set; }
    [Required]
    public IList<int> FileSizesX { get; set; }
    [Required]
    public IList<int> FileSizesY { get; set; }
}