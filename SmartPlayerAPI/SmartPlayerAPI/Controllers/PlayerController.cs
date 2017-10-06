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
    public class PlayerController : Controller
    {
        private readonly SmartPlayerContext _smartPlayerContext;
        public PlayerController(SmartPlayerContext smartPlayerContext)
        {
            _smartPlayerContext = smartPlayerContext;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(PlayerViewModelOut))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreatePlayer([FromBody]PlayerViewModelIn newPlayer)
        {
            //dodać obsługe czy club istnieje!
            try
            {
                var player = _smartPlayerContext.Add(new Player()
                {
                    FirstName = newPlayer.FirstName,
                    LastName = newPlayer.LastName,
                    DateOfBirth = newPlayer.DateOfBirth,
                    HeighOfUser = newPlayer.HeighOfUser,
                    WeightOfUser = newPlayer.WeightOfUser,
                    ClubId = newPlayer.ClubId
                });
                await _smartPlayerContext.SaveChangesAsync();

                return Ok(player.Entity);
            }
            catch(Exception e)
            {
                return BadRequest("Check if club exists");
            }

        }

        [HttpPost("addToGame")]
        [ProducesResponseType(200, Type = typeof(PlayerInGameViewModelOut))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddToGame([FromBody]PlayerInGameViewModelIn newGame)
        {
            //dodać obsługe czy gameId i PlayerId istnieje!
            try
            {
                var player = _smartPlayerContext.Add(new PlayerInGame()
                {
                    Position = newGame.Position,
                    Number = newGame.Number,
                    Active = newGame.Active,
                    GameId = newGame.GameId,
                    PlayerId = newGame.PlayerId
                });
                await _smartPlayerContext.SaveChangesAsync();

                return Ok(player.Entity);
            }
            catch (Exception e)
            {
                return BadRequest("Check if gameId or PlayerId exists in database");
            }

        }

    }
}
