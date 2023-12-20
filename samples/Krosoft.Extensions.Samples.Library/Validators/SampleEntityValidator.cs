using FluentValidation;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Samples.Library.Validators;

public class SampleEntityValidator : AbstractValidator<SampleEntity>
{
    public SampleEntityValidator()
    {
        RuleFor(c => c.Id).NotEmpty().NotNull();
        RuleFor(c => c.Name).NotEmpty().NotNull();
    }
}