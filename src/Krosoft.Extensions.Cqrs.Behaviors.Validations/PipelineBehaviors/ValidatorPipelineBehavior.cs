using System.Diagnostics;
using FluentValidation;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Validations.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

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

        var failures = await _validators.ValidateMoreAsync(request, cancellationToken);
        if (failures.Any())
        {
            throw new KrosoftFunctionalException(failures);
        }

        var response = await next();
        _logger.LogInformation($"Handled ValidatorPipelineBehavior <{typeof(TRequest).Name},{typeof(TResponse).Name}> in {sw.Elapsed.ToShortString()}");

        return response;
    }
}