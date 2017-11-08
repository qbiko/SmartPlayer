using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPlayerAPI.Persistance;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.Repository.Interfaces;
using SmartPlayerAPI.ViewModels;
using SmartPlayerAPI.ViewModels.Sensors.GPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Controllers
{
    [Route("api/[controller]")]
    public class SensorsController : Controller
    {
        private readonly SmartPlayerContext _smartPlayerContext;
        private readonly IAccelerometerAndGyroscopeRepository _accelerometerAndGyroscopeRepository;
        private readonly IGPSLocationRepository GPSLocationRepository;
        private readonly IPlayerInGameRepository _playerInGameRepository;

        public SensorsController(SmartPlayerContext smartPlayerContext,
            IAccelerometerAndGyroscopeRepository accelerometerAndGyroscopeRepository,
            IGPSLocationRepository gpsLocationRepository,
            IPlayerInGameRepository playerInGameRepository)
        {
            _smartPlayerContext = smartPlayerContext;
            _accelerometerAndGyroscopeRepository = accelerometerAndGyroscopeRepository;
            GPSLocationRepository = gpsLocationRepository;
            _playerInGameRepository = playerInGameRepository;
        }

        [HttpPost("pulse")]
        [ProducesResponseType(200, Type = typeof(PulseSensorOut))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SavePulseValue([FromBody]PulseSensorIn pulseSensorIn)
        {
            try
            {
                var playerInGame = await _smartPlayerContext.Set<PlayerInGame>().AsQueryable().SingleOrDefaultAsync(i => i.GameId == pulseSensorIn.GameId && i.PlayerId == pulseSensorIn.PlayerId).ConfigureAwait(false);
                if (playerInGame == null)
                {
                    return BadRequest("Bad Game or PlayerId");
                }

                var result = _smartPlayerContext.Add(new PulseSensorResult()
                {
                    Value = pulseSensorIn.Value,
                    TimeOfOccur = DateTimeOffset.Now,
                    PlayerInGameId = playerInGame.Id
                });

                await _smartPlayerContext.SaveChangesAsync();
                return Ok(result.Entity);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("pulseBatch")]
        [ProducesResponseType(200, Type = typeof(PulseSensorBatch<PulseSensorOutBatch>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SavePulseValueFromBatch([FromBody]PulseSensorBatch<PulseSensorInBatch> pulseSensorIn)
        {
            try
            {
                var playerInGame = await _smartPlayerContext.Set<PlayerInGame>().AsQueryable().SingleOrDefaultAsync(i => i.GameId == pulseSensorIn.GameId && i.PlayerId == pulseSensorIn.PlayerId).ConfigureAwait(false);
                if (playerInGame == null)
                {
                    return BadRequest("Bad Game or PlayerId");
                }

                PulseSensorBatch<PulseSensorOutBatch> result = new PulseSensorBatch<PulseSensorOutBatch>();
                result.GameId = pulseSensorIn.GameId;
                result.PlayerId = pulseSensorIn.PlayerId;
                foreach (var p in pulseSensorIn.PulseList)
                {
                    var res =  await _smartPlayerContext.AddAsync(new PulseSensorResult()
                    {
                        Value = p.Value,
                        TimeOfOccur = p.TimeOfOccur,
                        PlayerInGameId = playerInGame.Id
                    });
                    await _smartPlayerContext.SaveChangesAsync();
                    result.PulseList.Add(new PulseSensorOutBatch() {  Id = res.Entity.Id, TimeOfOccur = p.TimeOfOccur, Value = p.Value});
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("pulseBatch")]
        [ProducesResponseType(200, Type = typeof(List<PulseSensorViewModel>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPulseForUserInMatch(int playerId, int gameId)
        {
            try
            {
                var playerInGame = await _smartPlayerContext
                    .Set<PlayerInGame>()
                    .AsQueryable()
                    .Include(i => i.PulseSensorResults)
                    .SingleOrDefaultAsync(i => i.GameId == gameId && i.PlayerId == playerId)
                    .ConfigureAwait(false);

                if (playerInGame == null)
                    return BadRequest("check PlayerId or GameId");

                if (playerInGame.PulseSensorResults == null)
                    return BadRequest("No pulse sensor results");

                var list = playerInGame.PulseSensorResults
                    .ToList()
                    .OrderBy(i => i.TimeOfOccur)
                    .Select(i => new PulseSensorViewModel { Id = i.Id, TimeOfOccur = i.TimeOfOccur, Value = i.Value });

                return Ok(list);
            }
            catch(Exception e)
            {
                return BadRequest();
            }

        }

        [HttpPost("location")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SaveLocation([FromBody]GPSViewModel viewModel)
        {

            var playerInGame = await _playerInGameRepository.FindByCriteria(i => i.PlayerId == viewModel.PlayerId && i.GameId == viewModel.GameId);
            if (playerInGame == null)
                return BadRequest("Bad playerId or gameId");

            var location = await GPSLocationRepository.AddAsync(new GPSLocation() { Lat = viewModel.Lat, Lng = viewModel.Lng, TimeOfOccur = DateTimeOffset.Now, PlayerInGameId = playerInGame.Id });
            if (location == null)
                return BadRequest("Error during saving location coordinates in database");

            return Ok(true);

        }

        [HttpGet("locationsForPlayer")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetLocationsForPlayer([FromBody]GPSPlayerInGame viewModel)
        {
            var playerInGame = await _playerInGameRepository.FindWithInclude(i => i.PlayerId == viewModel.PlayerId && i.GameId == viewModel.GameId, i => i.GPSLocations);
            var listOfCoordinates = playerInGame.GPSLocations;

            return Ok();

        }
    }
}
