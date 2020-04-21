using AutoMapper;
using SmoothConfig.Api.Controllers;
using SmoothConfig.Api.Core.Lib;
using SmoothConfig.Api.Model;
using SmoothConfig.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmoothConfig.Api.MappingProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            var passwordHasher = new PasswordHasher(new HashingOptions());
            CreateMap<CreateUserViewModel, User>()
                .ForMember(
                    dest => dest.Password,
                    opt => opt.MapFrom(src => passwordHasher.Hash(src.Password)
                )
            );
        }
    }
}
