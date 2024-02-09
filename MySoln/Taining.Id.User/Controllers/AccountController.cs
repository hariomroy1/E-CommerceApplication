using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taining.Id.User.DTO;
using Taining.Id.User.Models;

namespace Taining.Id.User.Controllers
{

    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            _roleManager = roleManager;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            var User = new AppUser
            {
                DisplayName = registerDto.DisplayName,

                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.Phone
            };
            var result = await userManager.CreateAsync(User, registerDto.Password);
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                var role = new IdentityRole();
                role.Name = "admin";
                await _roleManager.CreateAsync(role);
            }
            if (!await _roleManager.RoleExistsAsync("user"))
            {
                var role = new IdentityRole();
                role.Name = "user";
                await _roleManager.CreateAsync(role);
            }
            if (!result.Succeeded) return BadRequest(StatusCode(400));
            var adminUser = await userManager.FindByEmailAsync("admin@ecommerce.com");

            if (adminUser != null)
            {
                await userManager.AddToRoleAsync(adminUser, "admin");
            }


            if (User.Email != "admin@ecommerce.com")
            {
                var user1 = await userManager.FindByEmailAsync(User.Email);
                await userManager.AddToRoleAsync(user1, "user");
            }
            return new UserDTO
            {
                DisplayName = User.DisplayName,

                Email = User.Email,
                Phone = User.PhoneNumber
            };
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginDTO { ReturnUrl = returnUrl });
        }
        // [HttpGet]

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
            var user1 = await userManager.FindByEmailAsync(loginDto.Email);
            var userRole = await userManager.GetRolesAsync(user1);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(loginDto.ReturnUrl) && Url.IsLocalUrl(loginDto.ReturnUrl))
                {
                    return Redirect(loginDto.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home"); 
                }
            }
            else
            {
                return View();
            }

        }

       
    }
}
