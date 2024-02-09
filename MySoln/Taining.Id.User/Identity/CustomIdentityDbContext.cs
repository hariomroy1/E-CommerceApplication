using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Taining.Id.User.Models;

namespace Taining.Id.User.Identity
{
    public class CustomIdentityDbContext: IdentityDbContext<AppUser>
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options): base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
