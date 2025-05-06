using FluentValidation;
using Krosoft.Extensions.Samples.Library.Models.Commands;

namespace Krosoft.Extensions.Samples.Library.Validators;

internal class HelloDotNet9CommandValidator : AbstractValidator<HelloDotNet9Command>
{
    public HelloDotNet9CommandValidator()
    { 
        RuleFor(c => c.Name).NotEmpty().NotNull();
    }
}