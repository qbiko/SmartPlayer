using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.Repository.Interfaces;
using SmartPlayerAPI.ViewModels.Pitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPlayerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PitchController : Controller
    {
        private readonly IPitchRepository _pitchRepository;
        private readonly IMapper _mapper;
        public PitchController(IPitchRepository pitchRepository, IMapper mapper)
        {
            _pitchRepository = pitchRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PitchOut))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreatePitch([FromBody]PitchIn pitchIn)
        {
            try
            {
                var mapPitch = _mapper.Map<Pitch>(pitchIn);
                if(mapPitch==null)
                    return BadRequest("Error during mapper");

                var newPitch = await _pitchRepository.AddAsync(mapPitch);
                if (newPitch == null)
                    return BadRequest("Error during save in database");

                var pitchOut = _mapper.Map<PitchOut>(newPitch);
                if (pitchOut == null)
                    return BadRequest("Error during save in database");

                return Ok(pitchOut);
            }
            catch (Exception e)
            {
                return BadRequest("Check if club exists");
            }

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<PitchOut>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetPitches()
        {
            try
            {
                var pitches = await _pitchRepository.GetAll();
                if (pitches == null)
                    return BadRequest("zero clubs");

                var list = new  List<PitchOut>();
                foreach(var p in pitches)
                {
                    var r = _mapper.Map<PitchOut>(p);
                    if (r == null)
                        return BadRequest("Error during mapper");
                    list.Add(r);
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest("some errors");
            }

        }
    }
}
