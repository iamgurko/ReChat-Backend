using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReChat.Data.EntityFrameworkCore;
using ReChat.Domain.Models;

namespace ReChat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private AppDbContext context;
        public ValuesController(AppDbContext dbContext)
        {
            context = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {

                var results = await context.Values.ToListAsync();
                return Ok(results);
        }
    }
}
