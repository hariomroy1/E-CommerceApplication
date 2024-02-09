using Microsoft.AspNetCore.Identity;
using Taining.Id.User.Models;
using Taining.Id.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Taining.Id.User.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


   builder.Services.AddDbContext<CustomIdentityDbContext>(options =>

      options.UseInMemoryDatabase("IdentityDatabase"));

/*builder.Services.AddDbContext<CustomIdentityDbContext>(x => 
{ x.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")); });*/

// Add your existing services and configurations here...

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/Account/Login";
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<CustomIdentityDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<AppUser>>();

builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(ConfigService.GetApis())
    .AddAspNetIdentity<AppUser>()
    .AddInMemoryClients(ConfigService.GetClients())
    .AddInMemoryApiScopes(ConfigService.Scopes)
    .AddDeveloperSigningCredential()
    .AddProfileService<IdentityProfileServices>();


/*builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountService", Version = "v1" });
});*/

// Configure the HTTP request pipeline.
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseIdentityServer();
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
//app.UseAuthorization();

// Add IdentityServer and other middleware here...

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seeding users and roles
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeed.SeedUsersAsync(userManager, roleManager);
}

app.Run();
