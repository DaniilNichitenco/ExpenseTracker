﻿using System;
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
using ExpenseTracker.Domain;
using ExpenseTracker.API.Infrastructure.Constants;

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

        [HttpPut]
        public async Task<IActionResult> UpdateTopic(TopicForUpdateDto topicForUpdateDto)
        {
            var topic = await _repository.Get(topicForUpdateDto.Id);
            if (topic == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, topic, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

            _mapper.Map(topicForUpdateDto, topic);
            _repository.Update(topic);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public IActionResult GetTopicsForUser()
        {
            var userId = int.Parse(HttpContext.GetUserIdFromToken());
            var topics = _repository.Where(t => t.OwnerId == null || t.OwnerId == userId);

            List<TopicDto> topicsDto = new List<TopicDto>();
            topics.ToList().ForEach(topic => topicsDto.Add(_mapper.Map<TopicDto>(topic)));
            return Ok(topicsDto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTopics()
        {
            var ar = await _authorizationService.AuthorizeAsync(HttpContext.User, "Permission");
            if(!ar.Succeeded)
            {
                return Forbid();
            }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _repository.Get(id);
            if(topic == null)
            {
                return NotFound();
            }

            var ar = await _authorizationService.AuthorizeAsync(HttpContext.User, topic, "Permission");
            if(!ar.Succeeded)
            {
                return Forbid();
            }

            _repository.DeleteTopic(id);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("maxUserTopics")]
        public IActionResult GetMaxUserTopics()
        {
            return Ok(Constants.MaxUserTopics);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] TopicForCreateDto topicForCreateDto)
        {
            var userId = int.Parse(HttpContext.GetUserIdFromToken());

            var count = _repository.Count(t => t.OwnerId == userId || t.OwnerId == null);

            if(count >= Constants.MaxUserTopics)
            {
                return BadRequest(new { Message = "User already has too much topics" });
            }

            var topic = _mapper.Map<Topic>(topicForCreateDto);

            topic.OwnerId = userId;

            await _repository.Add(topic);
            await _repository.SaveChangesAsync();

            var topicDto = _mapper.Map<TopicDto>(topic);

            return CreatedAtAction(nameof(GetTopic), new { id = topic.Id }, topicForCreateDto);
        }

        [HttpGet("amountTopics")]
        public IActionResult GetUserAmountTopics()
        {
            var userId = int.Parse(HttpContext.GetUserIdFromToken());

            var count = _repository.Count(t => t.OwnerId == userId || t.OwnerId == null);

            return Ok(count);
        }

        [HttpPost("forAll")]
        public async Task<IActionResult> CreateTopicForAllUsers([FromBody] TopicForCreateDto topicForCreateDto)
        {
            var ar = _authorizationService.AuthorizeAsync(HttpContext.User, "Permission");

            var topic = _mapper.Map<Topic>(topicForCreateDto);

            topic.OwnerId = null;

            await _repository.Add(topic);
            await _repository.SaveChangesAsync();

            var topicDto = _mapper.Map<TopicDto>(topic);

            return CreatedAtAction(nameof(GetTopic), new { id = topic.Id }, topicForCreateDto);
        }

        [HttpGet("userTopicsForList")]
        public IActionResult GetUserTopicsForList()
        {
            var userId = int.Parse(HttpContext.GetUserIdFromToken());
            var topics = _repository.Where(t => t.OwnerId == userId || t.OwnerId == null).ToList();

            var topicsForList = new List<TopicForListDto>();

            topics.ForEach(t =>
            {
                var topic = _mapper.Map<TopicForListDto>(t);
                var count = _repository.GetCountTopicExpenses(userId, t.Id);
                topic.CountExpenses = count;

                topicsForList.Add(topic);
            });

            return Ok(topicsForList);
        }
    }
}
