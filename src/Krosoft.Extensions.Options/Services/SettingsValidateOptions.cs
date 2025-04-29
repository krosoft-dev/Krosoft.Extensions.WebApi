using FluentValidation;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Options.Services;

public abstract class SettingsValidateOptions<TSettings> : IValidateOptions<TSettings>
    where TSettings : class
{
    private readonly IValidator<TSettings> _validator;

    public SettingsValidateOptions(IValidator<TSettings> validator)
    {
        _validator = validator;
    }

    public ValidateOptionsResult Validate(string? name, TSettings options)
    {
        var validationResult = _validator.Validate(options);

        if (validationResult.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        return ValidateOptionsResult.Fail(validationResult.Errors.Select(validationFailure => validationFailure.ErrorMessage));
    }
}