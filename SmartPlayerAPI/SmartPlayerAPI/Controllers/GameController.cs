using Microsoft.AspNetCore.Mvc;
using SmartPlayerAPI.Persistance;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.Repository.Interfaces;
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
        private readonly IGameRepository _gameRepository;
        public GameController(IGameRepository gameRepository, SmartPlayerContext smartPlayerContext)
        {
            _smartPlayerContext = smartPlayerContext;
            _gameRepository = gameRepository;
        }

        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(GameViewModelOut))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreatePlayer([FromBody]GameViewModelIn newGame)
        {
            try
            {
                var game = await _gameRepository.AddAsync(new Game()
                {
                    NameOfGame = newGame.NameOfGame,
                    TimeOfStart = newGame.TimeOfStart,
                    ClubId = newGame.ClubId,
                    PitchId = newGame.PitchId
                });

                return Ok(game);
            }
            catch (Exception e)
            {
                return BadRequest("Check if club exists");
            }

        }
    }
}
