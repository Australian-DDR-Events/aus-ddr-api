using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services;

public interface IAdminService
{
    Task<Result<bool>> UploadImage(string filePath, Stream fileStream, IList<Tuple<int, int>> sizes, CancellationToken cancellationToken);
}