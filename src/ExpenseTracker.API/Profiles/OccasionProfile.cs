using AutoMapper;
using ExpenseTracker.API.Dtos.Occasions;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Profiles
{
    public class OccasionProfile : Profile
    {
        public OccasionProfile()
        {
            CreateMap<Occasion, OccasionDto>();
            CreateMap<OccasionForUpdateDto, Occasion>();
        }
    }
}
