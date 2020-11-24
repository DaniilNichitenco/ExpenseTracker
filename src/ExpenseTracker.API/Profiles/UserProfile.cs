using AutoMapper;
using ExpenseTracker.API.Dtos.Account;
using ExpenseTracker.API.Dtos.UserDto;
using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;

namespace ExpenseTracker.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForSignUpDto, User>();
            CreateMap<UserForSignUpDto, UserInfo>(); 
            CreateMap<UserForSignUpDto, UserForUpdateDto>();
        }
    }
}
