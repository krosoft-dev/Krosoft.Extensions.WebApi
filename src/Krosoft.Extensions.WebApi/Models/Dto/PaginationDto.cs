using Krosoft.Extensions.Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.WebApi.Models.Dto;

public record PaginationDto : IPaginationDto
{
    [FromQuery]
    public int? PageNumber { get; set; }

    [FromQuery]
    public int? PageSize { get; set; }

    [FromQuery]
    public string[]? SortBy { get; set; }

    [FromQuery]
    public string? Text { get; set; }
}