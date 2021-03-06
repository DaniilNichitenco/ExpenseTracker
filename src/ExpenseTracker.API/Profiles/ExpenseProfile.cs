﻿using AutoMapper;
using ExpenseTracker.API.Dtos.Expenses;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Profiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Expense, ExpenseDto>();
            CreateMap<ExpenseForCreateDto, Expense>();
            CreateMap<ExpenseForUpdateDto, Expense>();
        }
    }
}
