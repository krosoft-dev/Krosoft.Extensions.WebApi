using FluentValidation;
using Krosoft.Extensions.Samples.Library.Models.Queries;

namespace Krosoft.Extensions.Samples.Library.Validators;

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