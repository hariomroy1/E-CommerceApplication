
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
