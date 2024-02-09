using Microsoft.AspNetCore.Identity;
using Taining.Id.User.Models;

namespace Taining.Id.User.Identity
{
    public class IdentitySeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            /* var user = new AppUser
             {
                 DisplayName = "admin",
                 Email = "admin3@ecommerce.com",
                 UserName = "admin3@ecommerce.com",
                 PhoneNumber = "1234567890"
             };
             if (await userManager.FindByEmailAsync(user.Email) == null)
             {
                 await userManager.CreateAsync(user, "Dhruv@22102001");
                 if (!await roleManager.RoleExistsAsync("admin"))
                 {
                     var role = new IdentityRole();
                     role.Name = "admin";
                     await roleManager.CreateAsync(role);
                 }
                 await userManager.AddToRoleAsync(user, "admin");
             }*/
            // Seed "admin" role
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                var adminRole = new IdentityRole("admin");
                await roleManager.CreateAsync(adminRole);
            }

            // Seed "user" role
            if (!await roleManager.RoleExistsAsync("user"))
            {
                var userRole = new IdentityRole("user");
                await roleManager.CreateAsync(userRole);
            }

            // Seed an admin user
            var adminUser = new AppUser
            {
                DisplayName = "admin",
                Email = "admin3@ecommerce.com",
                UserName = "admin3@ecommerce.com",
                PhoneNumber = "1234567890"
            };
            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                await userManager.CreateAsync(adminUser, "Hariom@22102001");
                await userManager.AddToRoleAsync(adminUser, "admin");
            }

            // Seed a regular user
            var regularUser = new AppUser
            {
                DisplayName = "regular",
                Email = "regular@ecommerce.com",
                UserName = "regular@ecommerce.com",
                PhoneNumber = "0987654321"
            };
            if (await userManager.FindByEmailAsync(regularUser.Email) == null)
            {
                await userManager.CreateAsync(regularUser, "Regular@22102001");
                await userManager.AddToRoleAsync(regularUser, "user");
            }
        }
    }
}
