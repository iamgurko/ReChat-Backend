using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReChat.API.DTOs;
using ReChat.Data.Interface;
using ReChat.Domain.Parameters;

namespace ReChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IRechatRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(IRechatRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("id", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageRepo = await _repo.GetMessage(id);
            if (messageRepo == null)
                return NotFound();

            return Ok(messageRepo);
        }
        [HttpGet("id", Name = "GetMessages")]
        public async Task<IActionResult> GetMessagesForUser(int userId, MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;
            var messagesRepo = await _repo.GetMessagesForUser(messageParams);
            var messages = _mapper.Map<IEnumerable<MessagesListDto>>(messagesRepo);
            return Ok();

        }
    }
}