using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Models;

namespace Application.Core.Interfaces.Services;

public interface IAdminService
{
    Task<Result> UploadImage(string filePath, Stream fileStream, IList<Tuple<int, int>> sizes, CancellationToken cancellationToken);
}