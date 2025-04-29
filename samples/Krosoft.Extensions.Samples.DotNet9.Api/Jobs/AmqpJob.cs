using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.DotNet9.Api.Models;
using Krosoft.Extensions.Samples.Library.Models.Enums;
using Krosoft.Extensions.Samples.Library.Models.Exceptions;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Jobs;

internal class AmqpJob : IRecurringJob
{
    private readonly ILogger<AmqpJob> _logger;
    private readonly IOptions<AppSettings> _options;

    public AmqpJob(ILogger<AmqpJob> logger, IOptions<AppSettings> options)
    {
        _logger = logger;
        _options = options;
    }

    public string Type => JobTypeCode.Amqp.ToString();

    public async Task ExecuteAsync(string identifiant)
    {
        Guard.IsNotNull(nameof(identifiant), identifiant);

        var cancellationToken = CancellationToken.None;
        var sw = Stopwatch.StartNew();

        _logger.LogInformation($"Exécution du job '{identifiant}'...");

        try
        {
            var jobSetting = _options.Value.JobsAmqp.FirstOrDefault(x => x.Identifiant == identifiant);
            if (jobSetting == null)
            {
                throw new JobIntrouvableException(identifiant);
            }

            _logger.LogInformation($"Exécution du job '{identifiant}'...");
            await Task.Delay(2000, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exécution du job '{identifiant}' en erreur : {e.Message}.", e);
        }
        finally
        {
            _logger.LogInformation($"Exécution du job '{identifiant}' terminée en {sw.Elapsed.ToShortString()}.");
        }
    }
}