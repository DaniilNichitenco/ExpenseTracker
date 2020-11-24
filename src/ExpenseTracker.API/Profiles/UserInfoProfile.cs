using AutoMapper;
using ExpenseTracker.API.Dtos.UserDto;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Profiles
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfo, UserDto>();
            CreateMap<UserForUpdateDto, UserInfo>();
        }
    }
}
