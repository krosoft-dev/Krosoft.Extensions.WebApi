using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Documents.DeposerFichier;

internal record DeposerFichierDto(
    [FromForm] long FichierId,
    IFormFile File);