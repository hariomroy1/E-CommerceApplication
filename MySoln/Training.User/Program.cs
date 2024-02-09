using DataLayer.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Training.User;
using Training.User.Middleware;
using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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

//Use serilog to log files
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

//1. add middleware services
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//For EntityFramework
/*builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
*/
//For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

//Adding authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});


/*builder.Services.AddDbContext<UserContext>(options => options.UseInMemoryDatabase("UserDatabase"));
*/
//Register Repository
builder.Services.RegisterRepositories();

//database feature for DOCKER
/*var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

var connectionString = Environment.GetEnvironmentVariable("DB_HOST") != null ?
                         $"Data Source ={dbHost};Initial-Catalog={dbName} User Id=sa;Password={dbPassword}" :
                         builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(connectionString);
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));*/



var app = builder.Build();


//2. use middleware for exception
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
    app.UseSwagger();
    app.UseSwaggerUI();
/*}*/

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
