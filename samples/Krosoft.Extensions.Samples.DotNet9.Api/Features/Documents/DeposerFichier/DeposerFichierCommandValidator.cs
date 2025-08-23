using FluentValidation;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichierSansRetour;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichier;

public class DeposerFichierCommandValidator : AbstractValidator<DeposerFichierCommand>
{
    public DeposerFichierCommandValidator()
    {
        RuleFor(m => m.FichierId).NotEmpty();
        RuleFor(v => v.File)
            .NotEmpty()
            .NotNull();
    }
}