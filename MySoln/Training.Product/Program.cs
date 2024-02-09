using DataLayer.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System.Configuration;
using Training.Product;
using Training.User.Middleware;


// add logger
Log.Logger = new LoggerConfiguration().MinimumLevel.Information().MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                      .Enrich.WithThreadId()
                                      .Enrich.WithProcessId()
                                      .Enrich.WithEnvironmentName()
                                      .Enrich.WithMachineName()
                                      .WriteTo.Console(new CompactJsonFormatter())
                                      .WriteTo.File(new CompactJsonFormatter(), "Log/log.txt", rollingInterval: RollingInterval.Day)
                                      .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//use serilog to log files
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

//add exception middleware Services
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//for eureka
builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IHealthCheckHandler, ScopedEurekaHealthCheckHandler>();

builder.Services.AddDbContext<ProductContext>(options =>

   options.UseInMemoryDatabase("productDatabase"));
/*builder.Services.AddDbContext<ProductContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
*/



builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", config => {

    config.Authority = "http://localhost:7226/";
    config.Audience = "ApiOne";
    config.RequireHttpsMetadata = false;


    builder.Services.AddAuthorization();


});

//for keys
/*builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("C:\\Users\\hariomroy\\Desktop\\MySoln\\Training.Product\\keys"));
*/
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ProductRepositories();

var app = builder.Build();


app.UseMiddleware<ExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MY API2");
});

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();

app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/info");
app.MapControllers();

app.Run();
