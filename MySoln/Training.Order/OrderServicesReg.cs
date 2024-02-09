using Training.Order.Services;

namespace Training.Order
{
    public static class OrderServicesReg
    {
        public static void OrderRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderServices, OrderServices>();
        }
    }
}
