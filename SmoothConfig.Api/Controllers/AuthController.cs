﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmoothConfig.Api.Services;
using SmoothConfig.Api.ViewModel;

namespace SmoothConfig.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel is null || string.IsNullOrEmpty(loginViewModel.Username) || string.IsNullOrEmpty(loginViewModel.Password))
                return Unauthorized();

            var jwt = _authenticationService.Login(loginViewModel.Username, loginViewModel.Password);

            return Ok(new { jwt });
        }

        /// <summary>
        /// Keep the user authenticated
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Refresh(string accessToken, string refreshToken)
        {
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessToken))
                return Unauthorized();

            try
            {
                var jwt = _authenticationService.Refresh(accessToken, refreshToken);
                return Ok(jwt);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public string Authenticated() => $"{User.Identity.Name}";

#if DEBUG
        [HttpGet]
        [Authorize]
        public object Claims() => User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            }
        );
#endif
    }
}