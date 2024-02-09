using Training.Product.Services;

namespace Training.Product
{
    public static class ProductServices
    {
        public static void ProductRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
