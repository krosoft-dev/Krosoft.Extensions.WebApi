using FluentValidation;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Validations.Extensions;

public static class ValidatorExtensions
{
    public static async Task ValidateMoreAndThrowAsync<T>(this IValidator<T> validator,
                                                          T item,
                                                          CancellationToken cancellationToken)
    {
        await validator.ValidateMoreAsync(item, errorsDetail =>
        {
            if (errorsDetail.Any())
            {
                var errors = errorsDetail.SelectMany(x => x.Errors).ToHashSet();
                throw new KrosoftFunctionalDetailedException(errors, errorsDetail);
            }
        }, cancellationToken);
    }

    public static async Task ValidateMoreAsync<T>(this IValidator<T> validator,
                                                  T request,
                                                  Action<ISet<ErrorDetail>> action,
                                                  CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult != null)
        {
            var validatorType = typeof(T).Name;
            var g = validationResult.Errors.GroupBy(x => x.PropertyName);
            var errors = new HashSet<ErrorDetail>();
            foreach (var grouping in g)
            {
                errors.Add(new ErrorDetail(validatorType, grouping.Key, grouping.Select(x => x.ErrorMessage).ToList()));
            }

            action(errors);
        }
    }
}