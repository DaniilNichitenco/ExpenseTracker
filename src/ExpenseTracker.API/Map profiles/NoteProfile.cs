using AutoMapper;
using ExpenseTracker.Domain;
using ExpenseTracker.API.Dtos.Notes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpenseTracker.API.Map_profiles
{
    public static class NoteProfile
    {
        public static Note MapToNote(this IMapper mapper, NoteForUpdateDto noteForUpdateDto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NoteForUpdateDto, Note>();
            });
            return mapper.Map<Note>(noteForUpdateDto);
        }

    }
}
