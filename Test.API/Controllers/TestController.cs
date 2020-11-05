using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test.API.Controllers
{
    [Route("")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new
            {
                name = "Test API",
                securedEndpoint = "https://localhost:5001/api/weatherforecast",
                tokenEndpoint = "https://localhost:5000/connect/token"
            });
        }
    }
}
