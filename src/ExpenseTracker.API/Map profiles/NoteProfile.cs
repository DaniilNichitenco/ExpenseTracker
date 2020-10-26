using AutoMapper;
using ExpenseTracker.Domain;
using ExpenseTracker.API.Dtos.Notes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpenseTracker.API.Map_Persons
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteDto>();
            CreateMap<NoteForUpdateDto, Note>();
        }
    }
}
