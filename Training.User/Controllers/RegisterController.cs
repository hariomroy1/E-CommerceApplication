using Training.User.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


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

        public RegisterController(IConfiguration configuration, IRegisterRepository registerRepository, ILogger<RegisterController> logger)
        {
            _configuration = configuration;
            _registerRepository = registerRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user registration based on the provided user entity.
        /// </summary>
        /// <param name="user">The user entity containing registration details.</param>
        /// <returns>An IActionResult representing the result of the registration operation.</returns>

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
