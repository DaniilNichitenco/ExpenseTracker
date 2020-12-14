using AutoMapper;
using ExpenseTracker.API.Dtos.Expenses;
using ExpenseTracker.API.Dtos.Topics;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Profiles
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<Topic, TopicDto>();
            CreateMap<ICollection<Topic>, ICollection<TopicDto>>();
            CreateMap<Topic, TopicWithExpensesDto>()
                .ForMember(t => t.Expenses, m =>
                    m.MapFrom(e => e.Expenses));
            CreateMap<TopicDto, Topic>();
            CreateMap<TopicForCreateDto, Topic>();
            CreateMap<Topic, TopicForListDto>()
                .ForMember(t => t.isGeneral, m => 
                m.MapFrom(t => !t.OwnerId.HasValue));
            CreateMap<TopicForUpdateDto, Topic>();
        }
    }
}
