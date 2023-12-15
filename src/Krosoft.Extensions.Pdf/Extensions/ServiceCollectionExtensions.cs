using Krosoft.Extensions.Pdf.Interfaces;
using Krosoft.Extensions.Pdf.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Pdf.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPdf(this IServiceCollection services)
    {
        services.AddTransient<IPdfService, PdfService>();

        return services;
    }
}