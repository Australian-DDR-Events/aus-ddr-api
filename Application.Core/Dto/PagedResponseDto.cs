using System.Collections.Generic;
using Application.Core.Entities;

namespace Application.Core.Dto;

public class PagedResponseDto<T> where T : BaseEntity
{
    public int Total { get; set; }
    public IEnumerable<T> Result { get; set; }
}