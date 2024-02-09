using Training.User.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Identity;
using Training.User.Model;

namespace Traingin.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RegisterController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IRegisterRepository _registerRepository;
        private readonly ILogger<RegisterController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterController(IConfiguration configuration, IRegisterRepository registerRepository, ILogger<RegisterController> logger)
        {
            _configuration = configuration;
            _registerRepository = registerRepository;
            _logger = logger;

        }
        /*   public RegisterController(UserManager<IdentityUser> userManager,
                                     RoleManager<IdentityRole> roleManager,
                                     IConfiguration configuration)
           {
               _userManager = userManager;
               _roleManager = roleManager;
               _configuration = configuration;
           }
   */
        /// <summary>
        /// Creates a new user registration based on the provided user entity.
        /// </summary>
        /// <param name="user">The user entity containing registration details.</param>
        /// <returns>An IActionResult representing the result of the registration operation.</returns>

        /*   [HttpPost("CreateRegister")]
           public async Task<IActionResult> Create(RegisterEntity registeruser, string role)
           {
               //check user Exist
               var userExist = await _userManager.FindByEmailAsync(registeruser.Email);
               if (userExist != null)
               {
                   throw new Exception("User is Already Exist! Please Try with another Email");
               }
               //Add the user in database
               IdentityUser user = new()
               {
                   Email = registeruser.Email,
                   SecurityStamp = Guid.NewGuid().ToString(),
                   UserName = registeruser.Name,
               };
               if(await _roleManager.RoleExistsAsync(role))
               {
                   var result = await _userManager.CreateAsync(user, registeruser.Password);
                   if(! result.Succeeded)
                   {
                       throw new Exception("User Failed to Create!");
                   }
                   //Add Role to the user
                  await _userManager.AddToRoleAsync(user, role);
                   return StatusCode(StatusCodes.Status200OK,
                       new Response { Status = "Success", Message = "User Created SuccessFully" }
                       );
               }
               else
               {
                   throw new Exception("This Role does not exist !");

               }




           }*/

        [HttpPost("CreateRegister")]
        public IActionResult Create(RegisterEntity user)
        {
            string result;

            result = _registerRepository.Create(user);
            if (result == "success")
            {
                return Ok(result);
            }
            throw new ArgumentException("Failed to create user registration.");
        }



        /// <summary>
        /// Logs in a user based on the provided login entity.
        /// </summary>
        /// <param name="user">The login entity containing user credentials.</param>
        /// <returns>
        /// An IActionResult representing the result of the login operation.
        /// If the login is successful, returns Ok with user details.
        /// If the user is not found, returns NoContent.
        /// </returns>

        /* [HttpPost("LoginUser")]
         public async Task<IActionResult> Login(LoginEntity loginEntity, string role)
         {
             // Check if the user exists based on the provided email
             var user = await _userManager.FindByEmailAsync(loginEntity.Email);
             if (user == null)
             {
                 // User not found, return appropriate response
                 throw new Exception("Email is not valid");
             }

             // Use UserManager to verify the user's password
             var result = await _userManager.CheckPasswordAsync(user, loginEntity.Password);
             if (!result)
             {
                 // Password doesn't match, return appropriate response
                 throw new InvalidOperationException("password is not valid");
             }

             // Optionally, check if the user has a specific role
             if (!string.IsNullOrEmpty(role) && !await _userManager.IsInRoleAsync(user, role))
             {
                 // User does not have the required role, return appropriate response
                 throw new Exception("Please provide a Role");
             }

             // User exists, credentials are valid, return user details
             return Ok(user);
         }
 */


        [HttpPost("LoginUser")]
        public IActionResult Login(LoginEntity user)
        {
            var userAvailable = _registerRepository.Login(user);

            if (userAvailable != null)
            {
                return Ok(userAvailable);
            }
            throw new InvalidOperationException($"User not found.");

        }

        /// <summary>
        /// Finds the current user based on the provided email.
        /// </summary>
        /// <param name="email">The email of the user to find.</param>
        /// <returns>
        /// An IActionResult representing the result of the user retrieval operation.
        /// If the user is found, returns Ok with user details.
        /// If the user is not found, throws an InvalidOperationException.
        /// </returns>
        /// <exception>Thrown when the user with the specified email is not found.</exception>

        [HttpGet("CurrentUser")]
        public async Task<IActionResult> FindCurrentUser(string email)
        {
            var currentUser = await _registerRepository.FindCurrentUser(email);

            if (currentUser != null)
            {
                return Ok(currentUser);
            }
            throw new InvalidOperationException($"User with Email {email} not found.");

        }


        /// <summary>
        /// Finds the current user by their unique Id.
        /// </summary>
        /// <param name="userId">The unique Id of the user to find.</param>
        /// <returns>
        /// An IActionResult representing the result of the user retrieval operation.
        /// If the user is found, returns Ok with user details.
        /// If the user is not found, throws an InvalidOperationException.
        /// </returns>
        /// <exception>Thrown when the user with the provided ID is not found.</exception>


        [HttpGet("CurrentUserById/{userId}")]
        public async Task<IActionResult> FindCurrentUserByID(int userId)
        {
            if (userId <= 0)
            {
                // Throw an exception when the client does not provide a proper ID
                throw new ArgumentException("Invalid user ID.");
            }

            var currentUser = await _registerRepository.FindCurrentUserById(userId);

            if (currentUser != null)
            {
                return Ok(currentUser);
            }

            // Throw an exception when the user with the provided ID is not found
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

    }
}
