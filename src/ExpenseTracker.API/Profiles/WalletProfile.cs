using AutoMapper;
using ExpenseTracker.API.Dtos.Wallets;
using ExpenseTracker.Domain.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Profiles
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, WalletDto>().IncludeAllDerived();
            CreateMap<WalletForUpdateDto, Wallet>().IncludeAllDerived();
            CreateMap<WalletForCreateDto, Wallet>().IncludeAllDerived();
            CreateMap<Wallet, WalletForListDto>().IncludeAllDerived();
        }
    }
}
