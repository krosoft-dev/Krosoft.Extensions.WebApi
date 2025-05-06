using System.Diagnostics;
using FluentValidation;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Validations.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using TimeSpanExtensions = Krosoft.Extensions.Core.Extensions.TimeSpanExtensions;

namespace Krosoft.Extensions.Cqrs.Behaviors.Validations.PipelineBehaviors;

public class ValidatorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<ValidatorPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorPipelineBehavior(ILogger<ValidatorPipelineBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation($"Handling ValidatorPipelineBehavior <{typeof(TRequest).Name},{typeof(TResponse).Name}>");

        var errorsDetail = await _validators.ValidateMoreAsync(request, cancellationToken);
        if (errorsDetail.Any())
        {
            var errors = errorsDetail.SelectMany(x => x.Errors).ToHashSet();
            throw new KrosoftFunctionalDetailedException(errors, errorsDetail);
        }

        var response = await next();
        _logger.LogInformation($"Handled ValidatorPipelineBehavior <{typeof(TRequest).Name},{typeof(TResponse).Name}> in {TimeSpanExtensions.ToShortString(sw.Elapsed)}");

        return response;
    }
}