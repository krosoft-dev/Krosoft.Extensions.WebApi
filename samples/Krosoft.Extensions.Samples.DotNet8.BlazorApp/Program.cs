using Krosoft.Extensions.Samples.DotNet8.BlazorApp.Components;
using Krosoft.Extensions.Samples.DotNet8.BlazorApp.Interfaces;
using Krosoft.Extensions.Samples.DotNet8.BlazorApp.Services;

namespace Krosoft.Extensions.Samples.DotNet8.BlazorApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
               .AddInteractiveServerComponents();

        //Services HTTP.  
        builder.Services.AddHttpClient<ILogicielsHttpService, LogicielsHttpService>(client => { client.BaseAddress = new Uri(builder.Configuration["AppSettings:Services:Api"] ?? string.Empty); });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
           .AddInteractiveServerRenderMode();

        app.Run();
    }
}