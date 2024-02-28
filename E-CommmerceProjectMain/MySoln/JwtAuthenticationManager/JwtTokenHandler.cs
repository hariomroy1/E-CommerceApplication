using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {

        public const string JWT_SECURITY_KEY = "yPkCqn4kSWLtaJwXvN2jGzpQRyTZ3gdXkt7FeBJP";
        private const int JWT_TOKEN_VALIDITY_MINUTES = 20;
        private readonly List<UserAccount> _userAccountList;

        public JwtTokenHandler()
        {

            _userAccountList = new List<UserAccount>
            {
                new UserAccount {UserName = "admin", Password = "admin", Role="admin"},
                new UserAccount {UserName = "user1", Password = "user1", Role="user"},
                new UserAccount {UserName = "user2", Password = "user2", Role="user"},
            };

        }

        /// <summary>
        /// Generates a JWT (JSON Web Token) for authentication based on the provided authentication request.
        /// </summary>
        /// <param name="authenticationRequest">The authentication request containing user credentials.</param>
        /// <returns>
        /// An AuthenticationResponse object containing the generated JWT token along with additional
        /// details such as the username and token expiration time.
        /// Returns null if the authentication request is invalid or the user credentials are incorrect.
        /// </returns>
        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticationRequest.UserName) || string.IsNullOrWhiteSpace(authenticationRequest.Password))
                return null;

            var userAccount = _userAccountList.Where(x => x.UserName == authenticationRequest.UserName && x.Password == authenticationRequest.Password).FirstOrDefault();

            if (userAccount == null) return null;

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINUTES);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim(ClaimTypes.Role, userAccount.Role)
               // new Claim("Role", userAccount.Role)
            });

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor =
            new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserName = userAccount.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
