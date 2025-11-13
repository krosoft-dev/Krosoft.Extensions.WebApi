using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Samples.DotNet10.Api.Features.Documents.DeposerFichier;
using Krosoft.Extensions.Samples.DotNet10.Api.Features.Documents.DeposerFichierSansRetour;
using Krosoft.Extensions.Samples.DotNet10.Api.Tests.Core;
using Krosoft.Extensions.Testing.Cqrs.Extensions;
using Krosoft.Extensions.Testing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Tests.Unit.Handlers.Commands;

[TestClass]
public class DeposerFichierCommandHandlerTests : SampleBaseTest<Program>
{
    //TestInitialize
    private Mock<ILogger<DeposerFichierCommandHandler>> _mockLogger = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        _mockLogger = new Mock<ILogger<DeposerFichierCommandHandler>>();
        services.SwapTransient(_ => _mockLogger.Object)
            ;

        base.AddServices(services, configuration);
    }

    [TestMethod]
    public async Task Handle_Empty()
    {
        await using var serviceProvider = CreateServiceCollection();

        Check.ThatCode(() => this.SendCommandAsync(serviceProvider, new DeposerFichierCommand(0, null!)))
             .Throws<KrosoftFunctionalException>()
             .WithMessage("'Fichier Id' ne doit pas être vide.");
    }

    [TestMethod]
    public async Task Handle_Ok()
    {
        await using var serviceProvider = CreateServiceCollection();

        var file = new KrosoftFile("text.txt", ByteHelper.GetBytes("Hello"));
        var command = new DeposerFichierCommand(42, file);
        var depotDto = await this.SendCommandAsync(serviceProvider, command);
        Check.That(depotDto).IsNotNull();
        Check.That(depotDto.Message).StartsWith("Fichier dispo sur temp");
    }
}