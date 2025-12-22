using FluentValidation;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Hello.Create;

internal class HelloCommandValidator : AbstractValidator<HelloCommand>
{
    public HelloCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty();
    }
}