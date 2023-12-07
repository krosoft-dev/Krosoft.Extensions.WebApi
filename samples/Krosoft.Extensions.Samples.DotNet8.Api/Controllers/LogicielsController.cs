using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Reporting.Csv.Extensions;
using Krosoft.Extensions.Samples.DotNet8.Api.Models.Commands;
using Krosoft.Extensions.Samples.DotNet8.Api.Models.Dto;
using Krosoft.Extensions.Samples.DotNet8.Api.Models.Queries;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.WebApi.Controllers;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Controllers;

[AllowAnonymous]
public class LogicielsController : ApiControllerBase
{
    [ProducesResponseType(typeof(IEnumerable<LogicielDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public Task<IEnumerable<LogicielDto>> GetAsync([FromQuery] LogicielsQuery query,
                                                   CancellationToken cancellationToken)
        => Mediator.Send(query, cancellationToken);

    [ProducesResponseType(typeof(IEnumerable<PickListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
    [HttpGet("PickList")]
    public Task<IEnumerable<PickListDto>> GetPickListAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsPickListQuery(), cancellationToken);

    [ProducesResponseType(typeof(LogicielDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public Task<LogicielDetailDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Mediator.Send(new LogicielDetailQuery(id), cancellationToken);

    [HttpPut]
    public Task UpdateAsync([FromBody] LogicielUpdateCommand command, CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);

    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [HttpPost]
    public Task<Guid> CreateAsync([FromBody] LogicielCreateCommand command, CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);

    [HttpPost("Import")]
    public async Task<int> ImportAsync(CancellationToken cancellationToken)
    {
        var files = await this.GetRequestToBase64StringAsync();
        return await Mediator.Send(new LogicielImportCommand(files), cancellationToken);
    }

    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK, "text/csv")]
    [HttpGet("Export/Csv")]
    public Task<FileStreamResult> ExportCsvAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsExportCsvQuery(), cancellationToken)
                   .ToCsvStreamResult()
                   .ToFileStreamResult();

    [HttpGet("Export/Pdf")]
    public Task<FileStreamResult> ExportPdfAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsExportPdfQuery(), cancellationToken)
                   .ToFileStreamResult();

    [HttpGet("Export/Zip")]
    public Task<FileStreamResult> ExportZipAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsExportZipQuery(), cancellationToken)
                   .ToFileStreamResult();

    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    public Task DeleteAsync([FromBody] LogicielsDeleteCommand command,
                            CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);
}