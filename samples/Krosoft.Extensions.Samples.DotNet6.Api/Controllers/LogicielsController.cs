using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Commands;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.WebApi.Controllers;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Controllers;

[AllowAnonymous]
public class LogicielsController : ApiControllerBase
{
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [HttpPost]
    public Task<Guid> CreateAsync([FromBody] LogicielCreateCommand command, CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);

    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [HttpDelete]
    public Task DeleteAsync([FromBody] LogicielsDeleteCommand command,
                            CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);

    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK, "text/csv")]
    [HttpGet("Export/Csv")]
    public Task<FileStreamResult> ExportCsvAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsExportCsvQuery(), cancellationToken)
                   .ToFileStreamResult();

    [HttpGet("Export/Pdf")]
    public Task<FileStreamResult> ExportPdfAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsExportPdfQuery(), cancellationToken)
                   .ToFileStreamResult();

    [HttpGet("Export/Zip")]
    public Task<FileStreamResult> ExportZipAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsExportZipQuery(), cancellationToken)
                   .ToFileStreamResult();

    [HttpGet]
    [ProducesResponseType(typeof(PaginationResult<LogicielDto>), StatusCodes.Status200OK)]
    public Task<PaginationResult<LogicielDto>> GetAsync([FromQuery] LogicielsQuery query,
                                                        CancellationToken cancellationToken)
        => Mediator.Send(query, cancellationToken);

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LogicielDetailDto), StatusCodes.Status200OK)]
    public Task<LogicielDetailDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Mediator.Send(new LogicielDetailQuery(id), cancellationToken);

    [HttpGet("PickList")]
    [ProducesResponseType(typeof(IEnumerable<PickListDto>), StatusCodes.Status200OK)]
    public Task<IEnumerable<PickListDto>> GetPickListAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsPickListQuery(), cancellationToken);

    [HttpPost("Import")]
    public async Task<int> ImportAsync(CancellationToken cancellationToken)
    {
        var files = await this.GetRequestToBase64StringAsync();
        return await Mediator.Send(new LogicielImportCommand(files), cancellationToken);
    }

    [HttpPut]
    public Task UpdateAsync([FromBody] LogicielUpdateCommand command, CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);
}