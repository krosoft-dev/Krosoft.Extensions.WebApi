using FluentValidation;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Validations.Extensions;

public static class ValidatorsExtensions
{
    public static async Task ValidateMoreAndThrowAsync<T>(this IEnumerable<IValidator<T>> validators,
                                                          T item,
                                                          CancellationToken cancellationToken)
    {
        var failures = await validators.ValidateMoreAsync(item, cancellationToken);
        if (failures.Any())
        {
            throw new KrosoftFunctionalException(failures);
        }
    }

    public static async Task<ISet<string>> ValidateMoreAsync<T>(this IEnumerable<IValidator<T>> validators,
                                                                T request,
                                                                CancellationToken cancellationToken)
    {
        var failures = new List<string>();
        foreach (var validator in validators)
        {
            await validator.ValidateMoreAsync(request, f => { failures.AddRange(f); }, cancellationToken);
        }

        return failures.ToHashSet();
    }

    public static async Task ValidateMoreAsync<T>(this IEnumerable<IValidator<T>> validators,
                                                  T request,
                                                  Action<ISet<string>> action,
                                                  CancellationToken cancellationToken)
    {
        var failures = await validators.ValidateMoreAsync(request, cancellationToken);

        action(failures);
    }
}