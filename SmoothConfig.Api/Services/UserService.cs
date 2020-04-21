using AutoMapper;
using SmoothConfig.Api.Controllers;
using SmoothConfig.Api.Core.Lib;
using SmoothConfig.Api.Model;
using SmoothConfig.Api.Repositories;
using SmoothConfig.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public void CreateUser(CreateUserViewModel createUserViewModel)
        {
            var user = _mapper.Map<User>(createUserViewModel);
            _userRepository.NewUser(user);
        }
    }
}
