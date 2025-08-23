using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichier;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichierSansRetour;

internal class DeposerFichierSansRetourCommandHandler : IRequestHandler<DeposerFichierSansRetourCommand>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<DeposerFichierSansRetourCommandHandler> _logger;

    public DeposerFichierSansRetourCommandHandler(ILogger<DeposerFichierSansRetourCommandHandler> logger,
                                                  IDateTimeService dateTimeService)
    {
        _logger = logger;
        _dateTimeService = dateTimeService;
    }

    public async Task Handle(DeposerFichierSansRetourCommand request,
                             CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Dépot du fichier '{request.File!.Name}' ({request.File.Content.Length}) avec l'identifiant '{request.FichierId}'...");

        var now = _dateTimeService.Now;

        await Task.CompletedTask;

        var tempPath = Path.Join("temp", request.FichierId.ToString());
        DirectoryHelper.CreateDirectoryIfNotExist(tempPath);

        using var xmlStream = new MemoryStream(request.File.Content);
        var filePath = Path.Join(tempPath, $"{now.Ticks}.txt");
        FileHelper.CreateFile(filePath, xmlStream);

        _logger.LogInformation("Fichier dispo");
    }
}