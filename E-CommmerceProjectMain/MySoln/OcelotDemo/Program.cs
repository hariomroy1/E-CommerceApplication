using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using JwtAuthenticationManager;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
       .AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseOcelot();

app.Run();
