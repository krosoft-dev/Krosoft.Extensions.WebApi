using FluentValidation;
using Krosoft.Extensions.Samples.DotNet9.Api.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Validators;

internal class AppSettingsValidator : AbstractValidator<AppSettings>
{
    public AppSettingsValidator(IValidator<JobAmqpSettings> validator)
    {
        RuleFor(v => v.BaseUrl)
            .NotEmpty();
        RuleFor(v => v.Token)
            .NotEmpty();
        RuleForEach(v => v.JobsAmqp)
            .SetValidator(validator);
    }
}