using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InzynierkaAPI.Controllers
{
    public class Error
    {
        public Error()
        {
            Errors = $"{{errors:{{ value: \"Should be positive number\",ss1byg4_f:{{name: \"cannot be empty\" }},41dddb2_f: \"cannot be empty\", 735dd22_f: \"should be yellow or white\" }}}}";
        }
        public dynamic Errors { get; set; }
    }
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Error))]
        public async Task<IActionResult> Get()
        {
            Error e = new Error() ;
          
            return Ok(e);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
