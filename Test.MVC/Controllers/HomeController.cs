using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Test.MVC.Models;

namespace Test.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IConfiguration configuration,
            ILogger<HomeController> logger
        )
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secure()
        {
            _logger.LogInformation("Access granted to Secure page for: " + User.Identity.Name);

            HttpRequestMessage getWeatherRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_configuration.GetValue<string>("API") + "/weatherforecast")
            };

            string accessToken = await HttpContext.GetTokenAsync("access_token");
            getWeatherRequest.Headers.Add("Authorization", $"Bearer {accessToken}");

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage getWeatherResponse = await httpClient.SendAsync(getWeatherRequest);

            string getWeatherResponseJson = await getWeatherResponse.Content.ReadAsStringAsync();

            // Prettify json response
            var getWeatherResponseJsonObject = JsonSerializer.Deserialize<object>(getWeatherResponseJson);
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            string prettyGetWeatherResponseJson = JsonSerializer.Serialize(getWeatherResponseJsonObject, options);

            ViewData["WeatherForecast"] = prettyGetWeatherResponseJson;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
