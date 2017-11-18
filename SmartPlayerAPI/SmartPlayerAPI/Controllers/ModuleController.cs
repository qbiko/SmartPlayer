using Microsoft.AspNetCore.Mvc;
using SmartPlayerAPI.Repository.Interfaces;
using SmartPlayerAPI.ViewModels.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.ViewModels.Sensors;

namespace SmartPlayerAPI.Controllers
{
    [Route("api/controller")]
    public class ModuleController : Controller
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IPlayerInGameRepository _playerInGameRepository;
        private readonly IMapper _mapper;
        public ModuleController(IModuleRepository moduleRepository, IMapper mapper, IPlayerInGameRepository playerInGameRepository)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _playerInGameRepository = playerInGameRepository;
        }

        [HttpPost("add")]
        [ProducesResponseType(200, Type = typeof(ModuleOut))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SaveModule([FromBody]ModuleIn moduleIn)
        {
            try
            {
                var module = await _moduleRepository.AddAsync(new Persistance.Models.Module() { MACAddress = moduleIn.MACAddress, ClubId = moduleIn.ClubId});
                if (module != null)
                {
                    var result = _mapper.Map<ModuleOut>(module);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("getCredentials")]
        [ProducesResponseType(200, Type = typeof(PlayerInGameViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SaveModule(string mac)
        {
            try
            {
                var module = await _moduleRepository.FindByCriteria(i=>i.MACAddress.Equals(mac));
                if (module == null)
                    return BadRequest();
                var playerInGame = ( await _playerInGameRepository.GetListWithInclude(i => i.ModuleId == module.Id, i => i.Game).ConfigureAwait(false)).Last();
                if (playerInGame == null)
                    return BadRequest("Bad module mac");

                var now = DateTimeOffset.UtcNow;
                var endOfMatchTime = playerInGame.Game.TimeOfStart.AddHours(3).UtcDateTime;
                var startOfMatch = playerInGame.Game.TimeOfStart.UtcDateTime;
                if (startOfMatch.Ticks< now.Ticks && now.Ticks <  endOfMatchTime.Ticks) //dodac drugi parametr czas zakonczenia
                {
                    return Ok(new PlayerInGameViewModel() { GameId = playerInGame.GameId, PlayerId = playerInGame.PlayerId, ServerTime = (now.ToUnixTimeMilliseconds() - playerInGame.Game.TimeOfStart.ToUnixTimeMilliseconds()), Now = now, StartGame = playerInGame.Game.TimeOfStart });
                }
                return BadRequest("Game is ended: "+$" Now:{now} | Start game:{startOfMatch} | End game: {endOfMatchTime}");

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("modules")]
        [ProducesResponseType(200, Type = typeof(List<ModuleOut>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetModules(string clubId)
        {
            try
            {
                int id = 0;
                if (!int.TryParse(clubId, out id))
                    return BadRequest();

                var modules = await _moduleRepository.GetAll(id);
                if (modules != null)
                {
                    var response = new List<ModuleOut>();
                    foreach(var m in modules)
                    {
                        response.Add(_mapper.Map<ModuleOut>(m));
                    }
                 
                    return Ok(response);
                }

                return BadRequest("Club have not any modules");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
