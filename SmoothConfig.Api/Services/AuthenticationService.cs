using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmoothConfig.Api.Dto;
using SmoothConfig.Api.Model;
using SmoothConfig.Api.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly double _expirationTokenIn;

        public AuthenticationService(IConfiguration configuration, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _config = configuration;

            double expireIn;
            if (!double.TryParse(_config["Jwt:ExpirationTime"], out expireIn))
            {
                throw new InputFormatterException(
                    $"Jwt:ExpirationTime is in a wrong format: [{_config["Jwt:ExpirationTime"]}]");
            }

            _expirationTokenIn = expireIn;
        }

        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="login">Email and Password</param>
        /// <returns>JWT</returns>
        public JsonWebTokenDto Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _userRepository.GetUserByUsernameAndPassword(username, password);
            if (user is null)
                return null;

            var jwt = CreateJwt(user);

            _userRepository.SaveToken(user.Id, jwt.AccessToken, jwt.RefreshToken, jwt.ExpiresIn);

            return jwt;
        }

        /// <summary>
        /// Create the main object to return
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private JsonWebTokenDto CreateJwt(User user)
        {
            var jwt = new JsonWebTokenDto
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = GenerateRefreshToken(),
                ExpiresIn = DateTime.UtcNow.AddMinutes(_expirationTokenIn)
            };

            return jwt;
        }

        /// <summary>
        /// Create the Access token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateAccessToken(User user)
        {
            var jwtIssuer = _config["Jwt:Issuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var options = new IdentityOptions();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName),
                //new Claim(ClaimTypes.Role, user.SuperUser ? Role.SuperUser : Role.Admin)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationTokenIn),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }

        /// <summary>
        /// Create the Refresh Token.
        /// This one is responsible to keep the authentication alive
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            // GUID is not random is unique
            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
