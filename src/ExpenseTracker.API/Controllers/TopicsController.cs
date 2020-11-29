using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Topics;
using ExpenseTracker.API.Infrastructure.Models;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.API.Infrastructure.Extensions;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public TopicsController(ITopicRepository repository, IMapper mapper, 
            UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTopics()
        {
            var topics = await _repository.GetAll();

            List<TopicDto> topicsDto = new List<TopicDto>();
            topics.ToList().ForEach(topic => topicsDto.Add(_mapper.Map<TopicDto>(topic)));
            return Ok(topicsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTopic(int id)
        {
            var topic = await _repository.Get(id);

            var topicDto = _mapper.Map<TopicDto>(topic);
            return Ok(topicDto);
        }

        [HttpGet("topicsWithFixedExpenses/{count}")]
        public IActionResult GetTopicsWithFixedExpenses(int count)
        {
            var userId = HttpContext.GetUserIdFromToken();
            var topics = _repository.GetTopicsWithFixedExpenses(count, int.Parse(userId));


            List<TopicWithExpensesDto> topicsDto = new List<TopicWithExpensesDto>();
            topics.ToList().ForEach(topic => topicsDto.Add(_mapper.Map<TopicWithExpensesDto>(topic)));
            return Ok(topicsDto);
        }
    }
}
