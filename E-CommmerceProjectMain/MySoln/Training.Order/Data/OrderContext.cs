using Microsoft.EntityFrameworkCore;
using Training.Order.Model;

namespace DataLayer.Data
{
    public class OrderContext:DbContext
    {
        public OrderContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<OrderEntity> Orders { get; set; }
    }
}
