using System;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

       public IActionResult SaveUser(CreateUserViewModel createUserViewModel)
        {
            _userService.CreateUser(createUserViewModel);
            return Ok();
        }
    }
}