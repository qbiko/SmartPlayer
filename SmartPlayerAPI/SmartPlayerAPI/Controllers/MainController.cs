﻿using Microsoft.AspNetCore.Mvc;
using SmartPlayerAPI.Models;
using SmartPlayerAPI.Persistance;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [HttpPost("dbtest")]
        [ProducesResponseType(200, Type = typeof(Mock))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Post([FromBody]MockViewModel mock)
        {
            var result = _smartPlayerContext.Add(new Mock() { Text = mock.Text, Number = mock.Number, CreatedAt = DateTimeOffset.Now });
            await _smartPlayerContext.SaveChangesAsync();
            return Ok(result.Entity);
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
        [ProducesResponseType(200, Type = typeof(ClubViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Register([FromBody]ClubViewModel newClub)
        {
            try
            {
                var club = _smartPlayerContext.Add(new Club() {
                    Name = newClub.ClubName,
                    DateOfCreate = DateTimeOffset.Now,
                    PasswordHash = Convert.ToBase64String(Encoding.ASCII.GetBytes(newClub.Password))
                });
                await _smartPlayerContext.SaveChangesAsync();

                return Ok(club.Entity);
            }
            catch(Exception e)
            {
                return Ok(e);
            }

        }

        [HttpGet("login")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400, Type = typeof(Error))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login(string clubName, string password)
        {
            try
            {
                var findClub = _smartPlayerContext.Club.FirstOrDefault(i => i.Name == clubName);
                if(findClub==null)
                {
                    return BadRequest(new Error() { Success = false, Message = "Club doesn't exist"});
                }
                if (findClub.PasswordHash.Equals(Convert.ToBase64String(Encoding.ASCII.GetBytes(password))))
                {
                    return Ok(new { Success = true, id = findClub.Id});
                }
                return BadRequest(new Error() { Success = false, Message = "Bad password" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpPost("sensors")]
        [ProducesResponseType(200, Type = typeof(SensorsDataViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateDataFromSensors([FromBody] SensorsDataViewModel coordinates)
        {

            return Ok(coordinates);
        }


    }
}
