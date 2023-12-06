using FluentValidation;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Validations.Extensions;

public static class ValidatorExtensions
{
    public static async Task<ISet<string>> ValidateAsync<T>(this IEnumerable<IValidator<T>> validators,
                                                            T request,
                                                            CancellationToken cancellationToken)
    {
        var failures = new List<string>();
        foreach (var validator in validators)
        {
            await validator.ValidateAsync(request, f => { failures.AddRange(f); }, cancellationToken);
        }

        return failures.ToHashSet();
    }

    public static async Task ValidateAsync<T>(this IValidator<T> validator,
                                              T request,
                                              Action<IEnumerable<string>> action,
                                              CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult != null)
        {
            action(validationResult.Errors.Select(er => er.ErrorMessage).ToHashSet());
        }
    }

    public static async Task ValidateAndThrowAsync<T>(this IEnumerable<IValidator<T>> validators, T item, CancellationToken cancellationToken)
    {
        var failures = await validators.ValidateAsync(item, cancellationToken);
        if (failures.Any())
        {
            throw new KrosoftMetierException(failures);
        }
    }
}