using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Test.API.Controllers
{
    [Route("")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Test()
        {
            string apiEndpoint = _configuration.GetValue<string>("API");
            string identityServerEndpoint = _configuration.GetValue<string>("IdentityServer");

            return Ok(new
            {
                name = "Test API",
                securedEndpoint = $"{apiEndpoint}/weatherforecast",
                tokenEndpoint = $"{identityServerEndpoint}/connect/token"
            });
        }
    }
}
