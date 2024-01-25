using Training.User.Respositories;
using Training.User.Services;

namespace Training.User
{
    public static class RegisterServices
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRegisterRepository, RegisterRepository>();
        }
    }
}
