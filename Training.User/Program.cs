using DataLayer.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Training.User;
using Training.User.Middleware;
using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;


// add logger
Log.Logger = new LoggerConfiguration().MinimumLevel.Information().MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                      .Enrich.WithThreadId()
                                      .Enrich.WithProcessId()
                                      .Enrich.WithEnvironmentName()
                                      .Enrich.WithMachineName()
                                      .WriteTo.Console(new CompactJsonFormatter())
                                      .WriteTo.File(new CompactJsonFormatter(),"Log/log.txt",rollingInterval: RollingInterval.Day )
                                      .CreateLogger();

Log.Logger.Information("Logging is working fine");
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

//add middleware services
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

//Register Repository
builder.Services.RegisterRepositories();

var app = builder.Build();


// use middleware for exception
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
