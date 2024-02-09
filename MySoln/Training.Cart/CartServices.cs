using Training.Cart.Services;

namespace Training.Cart
{
    public static class CartServices
    {
        public static void CartRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICartRepository, CartRepository>();
        }
    }
}
