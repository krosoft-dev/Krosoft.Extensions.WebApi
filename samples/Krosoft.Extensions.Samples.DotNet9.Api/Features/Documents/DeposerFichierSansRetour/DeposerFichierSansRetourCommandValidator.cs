using FluentValidation;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichier;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichierSansRetour;

public class DeposerFichierSansRetourCommandValidator : AbstractValidator<DeposerFichierSansRetourCommand>
{
    public DeposerFichierSansRetourCommandValidator()
    {
        RuleFor(m => m.FichierId).NotEmpty();
        RuleFor(v => v.File)
            .NotEmpty()
            .NotNull();
    }
}