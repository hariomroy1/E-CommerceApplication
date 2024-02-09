using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Training.Cart;
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

//use Serilog to log files
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

builder.Services.AddDbContext<CartContext>(options =>

   options.UseInMemoryDatabase("cartDatabase"));
/*builder.Services.AddDbContext<CartContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
*/
builder.Services.CartRepositories();

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", config => {

    config.Authority = "http://localhost:7226/";
    config.Audience = "ApiTwo";
    config.RequireHttpsMetadata = false;


    builder.Services.AddAuthorization();


});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Training.Cart v1"));
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MY API1");
});
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();

app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
