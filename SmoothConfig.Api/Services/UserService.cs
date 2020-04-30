using AutoMapper;
using Microsoft.AspNetCore.Http;
using SmoothConfig.Api.Controllers;
using SmoothConfig.Api.Core.Lib;
using SmoothConfig.Api.Model;
using SmoothConfig.Api.Repositories;
using SmoothConfig.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmoothConfig.Api.Core.Extension;
using FluentValidation.Results;

namespace SmoothConfig.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IMapper mapper, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="createUserViewModel"></param>
        /// <returns>ValidationResult</returns>
        public ValidationResult CreateUser(CreateUserViewModel createUserViewModel)
        {

#warning Think in a better way to remove this dependence - ValidationResult from FluentValidation
            var validation = new ValidationResult();

            var rolesCurrentUser = _httpContextAccessor.HttpContext.User.GetRoles();

            if (rolesCurrentUser.All(r => r != Role.Admin) && createUserViewModel.Roles.Any(role => rolesCurrentUser.All(rc => rc != role)))
            {
                // Dot not have permission in a determinated "environment"
                var rolesWithoutPermission = createUserViewModel.Roles.Aggregate(string.Empty, (result, role) =>
                {
                    if(rolesCurrentUser.All(rc => rc != role))
                    {
                        result += $"[{role}] ";
                    }
                    return result;
                });
                
                validation.Errors.Add(new ValidationFailure("Roles", $"The current user do not have permission in: {rolesWithoutPermission}"));
                return validation;
            }

            var user = _mapper.Map<User>(createUserViewModel);
            _userRepository.NewUser(user);
            return validation;
        }
    }
}
