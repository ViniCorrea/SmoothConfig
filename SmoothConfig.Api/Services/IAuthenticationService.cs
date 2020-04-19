using SmoothConfig.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Services
{
    public interface IAuthenticationService
    {
        JsonWebTokenDto Login(string username, string password);
    }
}
