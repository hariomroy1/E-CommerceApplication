using JwtAuthenticationManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});
// Add services to the container.


builder.Services.AddControllers();

builder.Services.AddSingleton<JwtTokenHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
