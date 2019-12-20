using AutoMapper;
using ReChat.API.DTOs;
using ReChat.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReChat.API.Extensions
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserListDto>();
            CreateMap<User, UserDetailDto>();
            CreateMap<UserRegisterDto,User >();
        }
    }
}
