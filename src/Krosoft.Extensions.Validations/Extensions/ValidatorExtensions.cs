using FluentValidation;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Validations.Extensions;

public static class ValidatorExtensions
{
    public static async Task ValidateMoreAndThrowAsync<T>(this IValidator<T> validator,
                                                          T item,
                                                          CancellationToken cancellationToken)
    {
        await validator.ValidateMoreAsync(item, failures =>
        {
            if (failures.Any())
            {
                throw new KrosoftFunctionalException(failures);
            }
        }, cancellationToken);
    }

    public static async Task ValidateMoreAsync<T>(this IValidator<T> validator,
                                                  T request,
                                                  Action<ISet<string>> action,
                                                  CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult != null)
        {
            action(validationResult.Errors.Select(er => er.ErrorMessage).ToHashSet());
        }
    }
}