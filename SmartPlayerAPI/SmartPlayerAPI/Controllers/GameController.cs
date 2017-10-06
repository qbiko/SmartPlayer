using Microsoft.AspNetCore.Mvc;
using SmartPlayerAPI.Persistance;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly SmartPlayerContext _smartPlayerContext;
        public GameController(SmartPlayerContext smartPlayerContext)
        {
            _smartPlayerContext = smartPlayerContext;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(GameViewModelOut))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreatePlayer([FromBody]GameViewModelIn newGame)
        {
            try
            {
                var game = _smartPlayerContext.Add(new Game()
                {
                    NameOfGame = newGame.NameOfGame,
                    TimeOfStart = newGame.TimeOfStart,
                    ClubId = newGame.ClubId
                });
                await _smartPlayerContext.SaveChangesAsync();

                return Ok(game.Entity);
            }
            catch (Exception e)
            {
                return BadRequest("Check if club exists");
            }

        }
    }
}
