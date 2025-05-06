using FluentValidation;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Validations.Extensions;

public static class ValidatorsExtensions
{
    public static async Task ValidateMoreAndThrowAsync<T>(this IEnumerable<IValidator<T>> validators,
                                                          T item,
                                                          CancellationToken cancellationToken)
    {
        var errorsDetail = await validators.ValidateMoreAsync(item, cancellationToken);
        if (errorsDetail.Any())
        {
            var errors = errorsDetail.SelectMany(x => x.Errors).ToHashSet();
            throw new KrosoftFunctionalDetailedException(errors, errorsDetail);
        }
    }

    public static async Task<ISet<ErrorDetail>> ValidateMoreAsync<T>(this IEnumerable<IValidator<T>> validators,
                                                                     T request,
                                                                     CancellationToken cancellationToken)
    {
        var failures = new List<ErrorDetail>();
        foreach (var validator in validators)
        {
            await validator.ValidateMoreAsync(request, f => { failures.AddRange(f); }, cancellationToken);
        }

        return failures.ToHashSet();
    }

    public static async Task ValidateMoreAsync<T>(this IEnumerable<IValidator<T>> validators,
                                                  T request,
                                                  Action<ISet<ErrorDetail>> action,
                                                  CancellationToken cancellationToken)
    {
        var failures = await validators.ValidateMoreAsync(request, cancellationToken);

        action(failures);
    }
}