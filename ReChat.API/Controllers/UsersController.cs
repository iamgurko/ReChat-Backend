using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReChat.API.DTOs;
using ReChat.API.Extensions;
using ReChat.Data.Interface;
using ReChat.Domain.Models;
using ReChat.Domain.Parameters;

namespace ReChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IRechatRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IRechatRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userRepo = await _repo.GetUser(currentUserId, true);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize,
                 users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var isCurrentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == id;

            var user = await _repo.GetUser(id, isCurrentUser);

            var userReturn = _mapper.Map<UserDetailDto>(user);

            return Ok(userReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userRepo = await _repo.GetUser(id, true);

            _mapper.Map(userUpdateDto, userRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Kullanıcı ID : {id} kayıt esnasında hata oluştu");
        }

        [HttpPost("{id}/like/{recipientId}")]
         public async Task<IActionResult> LikeUser(int id, int recipientId)
         {
             if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                 return Unauthorized();

             var like = await _repo.GetLike(id, recipientId);

             if (like != null)
                 return BadRequest("Bu kişiyi zaten beğendiniz");

             if (await _repo.GetUser(recipientId, false) == null)
                 return NotFound();

             like = new Like
             {
                 LikerId = id,
                 LikeeId = recipientId
             };

              _repo.Add<Like>(like);

              if (await _repo.SaveAll())
                 return Ok();

              return BadRequest("Kullanıcı beğenirken hata oluştu");
         }
    }
}
