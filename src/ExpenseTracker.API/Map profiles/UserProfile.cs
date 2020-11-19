using AutoMapper;
using ExpenseTracker.API.Dtos.Account;
using ExpenseTracker.API.Dtos.People;
using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;

namespace ExpenseTracker.API.Map_profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForSignUpDto, User>();
            CreateMap<UserForSignUpDto, Person>(); 
            CreateMap<UserForSignUpDto, PersonForUpdateDto>();
        }
    }
}
