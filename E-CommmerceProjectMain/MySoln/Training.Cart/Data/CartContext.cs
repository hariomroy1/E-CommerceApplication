
using Microsoft.EntityFrameworkCore;
using Training.Cart.Model;

namespace DataLayer.Data
{
    public class CartContext:DbContext
    {
        public CartContext(DbContextOptions options):base(options)
        {
            
        }     
        public DbSet<CartEntity> Carts { get; set; }  
    }
}
