using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Application.Core.Services;

public class AdminService : IAdminService
{
    private readonly IFileStorage _fileStorage;
    
    public AdminService(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }
    
    public async Task<Result> UploadImage(string filePath, Stream fileStream, IList<Tuple<int, int>> sizes, CancellationToken cancellationToken)
    {
        var baseImage = await Image.LoadAsync(fileStream, cancellationToken);
            
        var uploadProcess = sizes.Select(async size =>
        {
            var copiedImage = baseImage.Clone(image => image.Resize(size.Item1, size.Item2));
            using var stream = new MemoryStream();
            await copiedImage.SaveAsPngAsync(stream, cancellationToken);
            var sizeDelimiter = size.Item1 == size.Item2 ? $"{size.Item1}" : $"{size.Item1}-{size.Item2}";
            await _fileStorage.UploadFileFromStream(stream, $"{filePath}.{sizeDelimiter}.png");
        });

        await Task.WhenAll(uploadProcess);
        return new Result
        {
            ResultCode = ResultCode.Ok
        };
    }
}