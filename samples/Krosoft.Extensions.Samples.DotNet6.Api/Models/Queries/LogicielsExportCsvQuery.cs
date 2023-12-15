using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Reporting.Csv.Models;
using Krosoft.Extensions.Samples.Library.Models.Dto;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Models.Queries;

public class LogicielsExportCsvQuery : AuthBaseQuery<CsvFileData<LogicielCsvDto>>
{
}