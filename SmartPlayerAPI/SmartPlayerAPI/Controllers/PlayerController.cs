using Microsoft.AspNetCore.Mvc;
using SmartPlayerAPI.Persistance;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                    HeightOfUser = newPlayer.HeightOfUser,
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

        [HttpGet("clubplayers")]
        [ProducesResponseType(200, Type = typeof(List<PlayerInClubViewModelOut>))]
        [ProducesResponseType(400, Type = typeof(Error))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPlayersForClub(int clubId)
        {
            //var players = _smartPlayerContext.Players.Where(i => i.Club.Name.Equals(clubName));
            //if (players == null)
            //{
            //    return BadRequest(new Error() { Success = false, Message = "0 players" });
            //}
            //return Ok(players);
            var club = await _smartPlayerContext.Set<Club>().AsQueryable().Include(i => i.Players).FirstOrDefaultAsync(i=>i.Id == clubId);
            var players = club.Players;
            if (players == null)
            {
                return BadRequest(new Error() { Success = false, Message = "0 players" });
            }
            List<PlayerInClubViewModelOut> playersOut = new List<PlayerInClubViewModelOut>();
            foreach (var p in players)
            {
                playersOut.Add(new PlayerInClubViewModelOut() {
                    Id = p.Id,
                    DateOfBirth = p.DateOfBirth,
                    FirstName = p.FirstName,
                    HeightOfUser = p.HeightOfUser,
                    LastName = p.LastName,
                    WeightOfUser = p.WeightOfUser
                });
            }
            return Ok(playersOut);
        }

        [HttpGet("gameplayers")]
        [ProducesResponseType(200, Type = typeof(List<PlayerInGameViewModelOut>))]
        [ProducesResponseType(400, Type = typeof(Error))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPlayersForGame(string playerInGameId)
        {
            int id = 0;
            try
            {
                if(int.TryParse(playerInGameId, out id))
                {
                    var players = _smartPlayerContext.PlayerInGames.Where(i => i.Id == id).Select(i=>i.Player);
                    if (players == null)
                    {
                        return BadRequest(new Error() { Success = false, Message = "No players" });
                    }
                    return Ok(players);
                }

            }
            catch(Exception e)
            {
                return BadRequest();
            }

            return BadRequest(new Error() { Success = false, Message = "Bad id format, or something else" });
        }

        //[HttpGet("CoordiantesForPlayers")]
        //[ProducesResponseType(200, Type = typeof(List<Player>))]
        //[ProducesResponseType(400, Type = typeof(Error))]
        //[ProducesResponseType(401)]
        //public async Task<IActionResult> CoordinatesForPlayersInGame(string playerInGameId)
        //{
        //    return Ok();
        //}

    }
}
