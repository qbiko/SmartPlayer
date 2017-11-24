using AutoMapper;
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
using SmartPlayerAPI.Extensions;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using SmartPlayerAPI.Repository.Locations;
using SmartPlayerAPI.ViewModels.Pitch;

namespace SmartPlayerAPI.Controllers
{
    [Route("api/[controller]")]
    public class SensorsController : Controller
    {
        private readonly SmartPlayerContext _smartPlayerContext;
        private readonly IAccelerometerAndGyroscopeRepository _accelerometerAndGyroscopeRepository;
        private readonly IGPSLocationRepository GPSLocationRepository;
        private readonly IPlayerInGameRepository _playerInGameRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IMapper _mapper;
        private readonly IPitchRepository _pitchRepository;
        private readonly IGameRepository _gameRepository;
        private readonly GPSService _gpsService;
        public SensorsController(SmartPlayerContext smartPlayerContext,
            IAccelerometerAndGyroscopeRepository accelerometerAndGyroscopeRepository,
            IGPSLocationRepository gpsLocationRepository,
            IPlayerInGameRepository playerInGameRepository,
            IModuleRepository moduleRepository,
            IMapper mapper,
            IPitchRepository pitchRepository,
            IGameRepository gameRepository)
        {
            _smartPlayerContext = smartPlayerContext;
            _accelerometerAndGyroscopeRepository = accelerometerAndGyroscopeRepository;
            GPSLocationRepository = gpsLocationRepository;
            _playerInGameRepository = playerInGameRepository;
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _pitchRepository = pitchRepository;
            _gameRepository = gameRepository;
            _gpsService = new GPSService();
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
                    TimeOfOccur = DateTimeOffset.UtcNow,
                    PlayerInGameId = playerInGame.Id
                });

                await _smartPlayerContext.SaveChangesAsync();
                return Ok(result.Entity);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
                var playerInGame = await _smartPlayerContext.Set<PlayerInGame>().AsQueryable().SingleOrDefaultAsync(i => i.PlayerId == pulseSensorIn.PlayerId && i.GameId == pulseSensorIn.GameId).ConfigureAwait(false);
                if (playerInGame == null)
                {
                    return BadRequest("Bad Game or PlayerId");
                }
                var game = await _gameRepository.FindById(pulseSensorIn.GameId);
                if (game == null)
                    return BadRequest("Bad game id");

                PulseSensorBatch<PulseSensorOutBatch> result = new PulseSensorBatch<PulseSensorOutBatch>();
                result.PlayerId = pulseSensorIn.PlayerId;
                result.GameId = pulseSensorIn.GameId;
                foreach (var p in pulseSensorIn.PulseList)
                {
                    var time = game.TimeOfStart.AddMilliseconds(p.TimeOfOccurLong);
                    var res =  await _smartPlayerContext.AddAsync(new PulseSensorResult()
                    {
                        Value = p.Value,
                        TimeOfOccur = time,
                        PlayerInGameId = playerInGame.Id,
                        
                    });
                    await _smartPlayerContext.SaveChangesAsync();
                    result.PulseList.Add(new PulseSensorOutBatch() {  Id = res.Entity.Id, TimeOfOccur = time, Value = p.Value});
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("locationBatch")]
        [ProducesResponseType(200, Type = typeof(GPSBatch<CartesianPointsInTime>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SaveGPSBatch([FromBody]GPSBatch<GeoPointsInTime> locationBatch)
        {
            try
            {
                var playerInGame = await _playerInGameRepository.FindWithInclude(i => i.PlayerId == locationBatch.PlayerId && i.GameId == locationBatch.GameId, i => i.Game);
                if (playerInGame == null)
                    return BadRequest("Bad playerId or gameId");

                var pitchId = playerInGame.Game.PitchId;
                if (pitchId == null)
                    return BadRequest("Pitch is not recognized or null");

                var pitch = await _pitchRepository.FindById(pitchId.GetValueOrDefault());
                if (pitch == null)
                    return BadRequest("Pitch is not recognized or null");

                var pitchCornersPoints = _mapper.Map<PitchCornersPoints>(pitch);
                if (pitchCornersPoints == null)
                    return BadRequest("error in mapping");

                var game = await _gameRepository.FindById(locationBatch.GameId);
                if (game == null)
                    return BadRequest("Bad game id");

                GPSBatch<CartesianPointsInTime> result = new GPSBatch<CartesianPointsInTime>();
                result.PlayerId = locationBatch.PlayerId;
                result.GameId = locationBatch.GameId;
                foreach (var p in locationBatch.ListOfPositions)
                {
                    var time = game.TimeOfStart.AddMilliseconds(p.TimeOfOccurLong);

                    if (p.Lat == null || p.Lng == null)
                        return BadRequest("lat or lng is null");

                    var xy = _gpsService.GetCartesianPoint(pitchCornersPoints, new GPSPoint(p.Lat, p.Lng));
                    if (xy == null || double.IsNaN(xy.X) || double.IsNaN(xy.Y) || double.IsInfinity(xy.X) || double.IsInfinity(xy.Y))
                        return BadRequest("cannot calculate distance between points. Value is Nan or infinity");

                    var location = await GPSLocationRepository.AddAsync(new GPSLocation() { Lat = p.Lat, Lng = p.Lng, TimeOfOccur = game.TimeOfStart.AddMilliseconds(p.TimeOfOccurLong), PlayerInGameId = playerInGame.Id, X = xy.X, Y = xy.Y });
                    if (location == null)
                        return BadRequest("Error during saving location coordinates in database");
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
                    .SingleOrDefaultAsync(i => i.PlayerId == playerId && i.GameId == gameId)
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

        [HttpGet("pulseBatchWithScope")]
        [ProducesResponseType(200, Type = typeof(List<PulseSensorViewModel>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPulseForUserInMatch(int playerId, int gameId, int scope)
        {
            try
            {
                var playerInGame = await _smartPlayerContext
                    .Set<PlayerInGame>()
                    .AsQueryable()
                    .Include(i => i.PulseSensorResults)
                    .SingleOrDefaultAsync(i => i.PlayerId == playerId && i.GameId == gameId)
                    .ConfigureAwait(false);

                if (playerInGame == null)
                    return BadRequest("check PlayerId or GameId");

                if (playerInGame.PulseSensorResults == null)
                    return BadRequest("No pulse sensor results");

                var list = playerInGame.PulseSensorResults
                    .ToList()
                    .OrderBy(i => i.TimeOfOccur)
                    .Select(i => new PulseSensorViewModel { Id = i.Id, TimeOfOccur = i.TimeOfOccur, Value = i.Value })
                    .TakeLast(scope);

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        [HttpGet("pulseBatchWithStartDate")]
        [ProducesResponseType(200, Type = typeof(List<PulseSensorViewModel>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPulseBatchWithStartDate(int playerId, int gameId, string startDateString)
        {

            try
            {
                DateTimeOffset startDate;
                if (!DateTimeOffset.TryParse(startDateString, out startDate))
                    return BadRequest("Bad format of startDateString, cannot parse");

                var playerInGame = await _smartPlayerContext
                    .Set<PlayerInGame>()
                    .AsQueryable()
                    .Include(i => i.PulseSensorResults)
                    .Include(i=>i.Game)
                    .SingleOrDefaultAsync(i => i.PlayerId == playerId && i.GameId == gameId )
                    .ConfigureAwait(false);

                if (playerInGame == null)
                    return BadRequest("check PlayerId or GameId");

                if (playerInGame.PulseSensorResults == null)
                    return BadRequest("No pulse sensor results");

                var list = playerInGame.PulseSensorResults
                    .ToList()
                    .Where(i => i.TimeOfOccur >= startDate)
                    .OrderBy(i => i.TimeOfOccur)
                    .Select(i => new PulseSensorViewModel { Id = i.Id, TimeOfOccur = i.TimeOfOccur, Value = i.Value });

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        [HttpPost("location")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SaveLocation([FromBody]PointWithCredentials viewModel)
        {
            try
            {
                var playerInGame = await _playerInGameRepository.FindWithInclude(i => i.PlayerId == viewModel.PlayerId && i.GameId == viewModel.GameId, i => i.Game);
                if (playerInGame == null)
                    return BadRequest("Bad playerId or gameId");

                var pitchId = playerInGame.Game.PitchId;
                if (pitchId == null)
                    return BadRequest("Pitch is not recognized or null");

                var pitch = await _pitchRepository.FindById(pitchId.GetValueOrDefault());
                if (pitch == null)
                    return BadRequest("Pitch is not recognized or null");

                var pitchCornersPoints = _mapper.Map<PitchCornersPoints>(pitch);
                if (pitchCornersPoints == null)
                    return BadRequest("error in mapping");

                if (viewModel.Lat == null || viewModel.Lng == null)
                    return BadRequest("lat or lng is null");

                var xy = _gpsService.GetCartesianPoint(pitchCornersPoints, new GPSPoint(viewModel.Lat, viewModel.Lng));
                if (xy == null || double.IsNaN(xy.X) || double.IsNaN(xy.Y) || double.IsInfinity(xy.X) || double.IsInfinity(xy.Y))
                    return BadRequest("cannot calculate distance between points. Value is Nan or infinity");

                var location = await GPSLocationRepository.AddAsync(new GPSLocation() { Lat = viewModel.Lat, Lng = viewModel.Lng, TimeOfOccur = DateTimeOffset.UtcNow, PlayerInGameId = playerInGame.Id, X = xy.X, Y = xy.Y });
                if (location == null)
                    return BadRequest("Error during saving location coordinates in database");

                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest("something wrong");
            }

        }

        [HttpGet("locationsBatch")]
        [ProducesResponseType(200, Type = typeof(List<GPSBatch<CartesianPointsInTime>>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetLocationsBatch(int gameId, string startDateString, params int[] playerIds)
        {
            try
            {
                DateTimeOffset startDate;
                if (!DateTimeOffset.TryParse(startDateString, out startDate))
                    return BadRequest("Bad format of startDateString, cannot parse");

                var gpsBatch = new List<GPSBatch<CartesianPointsInTime>>();
                foreach(var playerId in playerIds)
                {
                    var playerInGame = await _playerInGameRepository.FindWithInclude(i => i.PlayerId == playerId && i.GameId == gameId, i => i.GPSLocations);
                    if (playerInGame == null)
                        return BadRequest("Bad playerId or gameId");

                    var listOfCoordinates = playerInGame.GPSLocations
                        .Where(i => i.TimeOfOccur >= startDate)
                        .OrderBy(i => i.TimeOfOccur)
                        .Select(i => new CartesianPointsInTime() { X = i.X, Y = i.Y, TimeOfOccur = i.TimeOfOccur })
                        .ToList();

                    gpsBatch.Add(new GPSBatch<CartesianPointsInTime>() { PlayerId = playerId, GameId = gameId, ListOfPositions = listOfCoordinates });
                }

                return Ok(gpsBatch);
            }
            catch(Exception e)
            {
                return BadRequest("something wrong");
            }
        }
    }
}
