using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;

        public AccountController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }

        /// <summary>
        /// Authenticates a user based on the provided authentication request by generating a JWT 
        /// (JSON Web Token) for authorization.
        /// </summary>
        /// <param name="authenticationRequest">The authentication request containing user credentials.</param>
        /// <returns>
        /// An ActionResult containing the AuthenticationResponse object if authentication is successful.
        /// Returns Unauthorized status (401) if the authentication fails due to invalid credentials.
        /// </returns>
        [HttpPost("Login")]
        public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(authenticationRequest);
            if (authenticationResponse == null) return Unauthorized();
            return authenticationResponse;
        }
    }
}
