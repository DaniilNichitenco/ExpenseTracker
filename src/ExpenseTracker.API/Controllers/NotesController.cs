using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using ExpenseTracker.API.Dtos.Notes;
using ExpenseTracker.API.Infrastructure.Models;
using ExpenseTracker.API.Map_profiles;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _repository;
        private readonly IMapper _mapper;

        public NotesController(INoteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("PaginatedSearch")]
        public async Task<IActionResult> GetPagedNotes([FromBody]PagedRequest request)
        {
            var pagedNotesDto = await _repository.GetPagedData<NoteGridRowDto>(request);
            return Ok(pagedNotesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var note = await _repository.Get(id);
            if (note != null)
            {
                var noteDto = _mapper.Map<NoteDto>(note);
                return Ok(noteDto);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _repository.GetAll();
            List<NoteDto> notesDto = new List<NoteDto>();
            notes.ToList().ForEach(note => notesDto.Add(_mapper.Map<NoteDto>(note)));
            return Ok(notesDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody]NoteForUpdateDto noteForUpdateDto)
        {
            //var note = _mapper.Map<Note>(noteForUpdateDto);
            var note = _mapper.MapToNote(noteForUpdateDto);
            await _repository.Add(note);

            var noteDto = _mapper.Map<NoteDto>(note);

            return CreatedAtAction(nameof(GetNote), new { id = noteDto.Id }, noteDto);
        }
    }
}
