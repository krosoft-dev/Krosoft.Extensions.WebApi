using Krosoft.Extensions.Cqrs.Models.Commands;
using Krosoft.Extensions.Cqrs.Models.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Testing.Cqrs.Extensions;

public static class BaseTestExtensions
{
    public static async Task SendCommandAsync(this BaseTest baseTest,
                                              IServiceProvider serviceProvider,
                                              ICommand command)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command, CancellationToken.None);
    }

    public static async Task<T> SendCommandAsync<T>(this BaseTest baseTest,
                                                    IServiceProvider serviceProvider,
                                                    ICommand<T> command)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(command, CancellationToken.None);
    }

    public static async Task<TResponse> SendQueryAsync<TResponse>(this BaseTest baseTest,
                                                                  IServiceProvider serviceProvider,
                                                                  IQuery<TResponse> query)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var response = await mediator.Send(query, CancellationToken.None);

        return response;
    }
}