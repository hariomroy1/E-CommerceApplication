using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Eureka;

var builder = WebApplication.CreateBuilder(args);

/*builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
       .AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();*/

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
       .AddJsonFile("ocelotdevop.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.AddOcelot().AddEureka();
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseOcelot();

app.Run();
