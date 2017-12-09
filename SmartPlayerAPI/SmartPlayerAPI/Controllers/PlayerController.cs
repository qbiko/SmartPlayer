using Microsoft.AspNetCore.Mvc;
using SmartPlayerAPI.Persistance;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartPlayerAPI.Repository.Interfaces;

namespace SmartPlayerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly SmartPlayerContext _smartPlayerContext;
        private readonly IPlayerInGameRepository _playerInGameRepository;
        public PlayerController(SmartPlayerContext smartPlayerContext, IPlayerInGameRepository playerInGameRepository)
        {
            _smartPlayerContext = smartPlayerContext;
            _playerInGameRepository = playerInGameRepository;
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
                    HeighOfUser = newPlayer.HeightOfUser,
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
                    PlayerId = newGame.PlayerId,
                    ModuleId = newGame.ModuleId
                });
                await _smartPlayerContext.SaveChangesAsync();

                return Ok(player.Entity);
            }
            catch (Exception e)
            {
                return BadRequest("Check if gameId or PlayerId exists in database");
            }

        }

        [HttpDelete("removeFromGame")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> RemoveFromGame(string playerId, string gameId)
        {
            //dodać obsługe czy gameId i PlayerId istnieje!
            try
            {
                int playerIdInt = 0;
                int gameIdInt = 0;

                if(int.TryParse(playerId, out playerIdInt) && int.TryParse(gameId, out gameIdInt))
                {
                    var playerInGame = await _smartPlayerContext.Set<PlayerInGame>().FirstOrDefaultAsync(i => i.PlayerId == playerIdInt && i.GameId == gameIdInt);
                    if(playerInGame!=null)
                    {
                        playerInGame.Active = false;
                        var result = _playerInGameRepository.Update(playerInGame);
                    }


                    return Ok(true);
                }
                return BadRequest("Bad Player or Game id");

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
                    HeightOfUser = p.HeighOfUser,
                    LastName = p.LastName,
                    WeightOfUser = p.WeightOfUser
                });
            }
            return Ok(playersOut);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PlayerViewModelOut))]
        [ProducesResponseType(400, Type = typeof(Error))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPlayer(string playerId)
        {
            int id = 0;
            if (int.TryParse(playerId, out id))
            {
                var player = await _smartPlayerContext.Set<Player>().FirstOrDefaultAsync(i => i.Id == id);
                if(player!=null)
                {
                    return Ok(new PlayerViewModelOut()
                    {
                        Id = player.Id,
                        ClubId = player.ClubId,
                        DateOfBirth = player.DateOfBirth,
                        FirstName  = player.FirstName,
                        LastName = player.LastName,
                        HeightOfUser = player.HeighOfUser,
                        WeightOfUser = player.WeightOfUser
                    });
                }
                return BadRequest("Player doeasn't exists");
            }
            return BadRequest("Id of players is not properly");

        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(PlayerViewModelOut))]
        [ProducesResponseType(400, Type = typeof(Error))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UpdatePlayer([FromBody]UpdatePlayer newPlayer)
        {
            try
            {
                var player = await _smartPlayerContext.Set<Player>().FirstOrDefaultAsync(i=>i.Id == newPlayer.PlayerId);
                if (player == null)
                    return BadRequest("bad player id");

                if (newPlayer.HeightOfUser != null)
                    player.HeighOfUser = newPlayer.HeightOfUser.GetValueOrDefault();

                if (newPlayer.WeightOfUser != null)
                    player.WeightOfUser = newPlayer.WeightOfUser.GetValueOrDefault();

                var updatedPlayer = _smartPlayerContext.Set<Player>().Update(player);
                await _smartPlayerContext.SaveChangesAsync();
                if (updatedPlayer == null)
                    return BadRequest("something wrong");

                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest("something wrong");
            }

        }

        //[HttpGet("playerInGame")]
        //[ProducesResponseType(200, Type = typeof())]
        //[ProducesResponseType(400, Type = typeof(Error))]
        //[ProducesResponseType(401)]
        //public async Task<IActionResult> GetPlayerInGame(string playerId, string gameId)
        //{
        //    int playerIdint = 0;
        //    int gameIdint = 0;
        //    if (int.TryParse(playerId, out playerIdint) && int.TryParse(gameId, out gameIdint))
        //    {
        //        var player =  _smartPlayerContext.Set<Player>().FirstOrDefault(i => i.Id == playerIdint);
        //        var playerInGame = await _playerInGameRepository.FindByCriteria(i => i.PlayerId == playerIdint && i.GameId == gameIdint);
        //        if (player != null)
        //        {
        //            return Ok(new PlayerViewModelOut()
        //            {
        //                Id = player.Id,
        //                ClubId = player.ClubId,
        //                DateOfBirth = player.DateOfBirth,
        //                FirstName = player.FirstName,
        //                LastName = player.LastName,
        //                HeightOfUser = player.HeighOfUser,
        //                WeightOfUser = player.WeightOfUser
        //            });
        //        }
        //        return BadRequest("Player doeasn't exists");
        //    }
        //    return BadRequest("Id of players is not properly");

        //}

        [HttpGet("gameplayers")]
        [ProducesResponseType(200, Type = typeof(List<PlayerInGameViewModelOutExtend>))]
        [ProducesResponseType(400, Type = typeof(Error))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPlayersForGame(string gameId)
        {
            int id = 0;
            try
            {
                if (int.TryParse(gameId, out id))
                {
                    var game = await _smartPlayerContext.Set<Game>().AsQueryable().Include(i => i.PlayerInGames).FirstOrDefaultAsync(i => i.Id == id);
                    var playerInGmaes = game.PlayerInGames;
                    
                    var players = new List<PlayerInGameViewModelOutExtend>();
                    foreach(var p in playerInGmaes)
                    {
                        var player = await _smartPlayerContext.Set<Player>().FirstOrDefaultAsync(i => i.Id == p.PlayerId);
                        if (player != null && p.Active)
                        {
                            players.Add(new PlayerInGameViewModelOutExtend()
                            {
                                Position = p.Position,
                                Number = p.Number,
                                Active = p.Active,
                                GameId = p.GameId.GetValueOrDefault(),
                                PlayerId = p.PlayerId.GetValueOrDefault(),
                                Firstname = player.FirstName,
                                Lastname = player.LastName,
                                Id = player.Id,
                                ModuleId = p.ModuleId
                            });
                        }

                    }
                    if (players == null)
                    {
                        return BadRequest(new Error() { Success = false, Message = "No players" });
                    }
                    return Ok(players);
                }

            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return BadRequest(new Error() { Success = false, Message = "Bad id format, or something else" });
        }

    }
}
