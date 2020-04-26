using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmoothConfig.Api.Core.Lib;
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
        private readonly IPasswordHasher _passwordHasher;
        private readonly double _expirationTokenIn;

        public AuthenticationService(IConfiguration configuration, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _config = configuration;
            _passwordHasher = passwordHasher;

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
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>JWT</returns>
        public JsonWebTokenDto Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _userRepository.GetUserByUsername(username);

            // Check if user exists
            if (user is null)
                return null;

            // Check password
            (bool verified, bool needsUpgrade) = _passwordHasher.Check(user.Password, password);
            if (!verified)
                return null;

            var jwt = CreateJwt(user);

            _userRepository.SaveToken(user.Id, jwt.AccessToken, jwt.RefreshToken, jwt.ExpiresIn);

            return jwt;
        }

        /// <summary>
        /// The access token has a expiration time, you can use the refresh token to create a new one
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns>JWT</returns>
        /// <exception cref="SecurityTokenException">Invalid refresh token</exception>
        public JsonWebTokenDto Refresh(string accessToken, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = _userRepository.GetUserByUsernameAndRefreshToken(username, refreshToken);

            if (user is null || user.AccessToken.RefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

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
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Username),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.Username)
            };

            // Adding roles in the payload
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

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

        /// <summary>
        /// Reade the token and return a object with all information
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException">Invalid refresh token</exception>
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
