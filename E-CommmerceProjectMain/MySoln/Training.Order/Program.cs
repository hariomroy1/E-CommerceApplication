using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Training.Order;
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

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderContext>(options =>

   options.UseInMemoryDatabase("orderDatabase"));


builder.Services.OrderRepositories();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Training.Order v1"));
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
