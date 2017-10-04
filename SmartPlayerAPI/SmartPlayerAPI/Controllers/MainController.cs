using Microsoft.AspNetCore.Mvc;
using SmartPlayerAPI.Models;
using SmartPlayerAPI.Persistance;
using SmartPlayerAPI.Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Controllers
{
    [Route("api/[controller]")]
    public class MainController : Controller
    {
        private readonly SmartPlayerContext _smartPlayerContext;
        public MainController(SmartPlayerContext smartPlayerContext)
        {
            _smartPlayerContext = smartPlayerContext;
        }
        [HttpPost("decimal")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Post(decimal value)
        {

            return Ok(value);
        }

        [HttpPost("string")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> PostString(string value)
        {

            return Ok(value);
        }

        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Register([FromBody]User newUser)
        {
            var user = _smartPlayerContext.Add(new User() { CreatedAt = DateTimeOffset.UtcNow, Nick = newUser.Nick });
            await _smartPlayerContext.SaveChangesAsync();

            return Ok(user.Entity);
        }

        [HttpPost("coordiantes")]
        [ProducesResponseType(200, Type = typeof(BatchValues))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> PostString([FromBody] BatchValues coordinates)
        {

            return Ok(coordinates);
        }

    }
}
