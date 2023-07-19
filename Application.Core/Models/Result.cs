using Microsoft.CodeAnalysis;

namespace Application.Core.Models;

public record Result
{
    public required ResultCode ResultCode { get; init; }
}

public record Result<T>
{
    public required ResultCode ResultCode { get; init; }
    public required Optional<T> Value { get; init; }
}

public enum ResultCode
{
    Ok,
    NotFound,
    BadRequest,
    Conflict,
    Error
}
