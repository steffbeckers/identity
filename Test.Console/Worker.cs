using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Console
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Worker> _logger;
        private readonly IDiscoveryCache _disco;
        private readonly HttpClient _httpClient;

        public Worker(
            IConfiguration configuration,
            ILogger<Worker> logger
        )
        {
            _configuration = configuration;
            _logger = logger;
            _disco = new DiscoveryCache(_configuration.GetValue<string>("IdentityServer"));
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await AuthenticateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while authenticating: " + ex.Message, ex);
                throw;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await GetWeatherForecastAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while retrieving the weather forecast: " + ex.Message, ex);
                    throw;
                }
                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task GetWeatherForecastAsync()
        {
            HttpRequestMessage getWeatherRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_configuration.GetValue<string>("API") + "/weatherforecast")
            };

            HttpResponseMessage getWeatherResponse = await _httpClient.SendAsync(getWeatherRequest);
            getWeatherResponse.EnsureSuccessStatusCode();

            string getWeatherResponseJson = await getWeatherResponse.Content.ReadAsStringAsync();

            // Prettify json response
            var getWeatherResponseJsonObject = JsonSerializer.Deserialize<object>(getWeatherResponseJson);
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            string prettyGetWeatherResponseJson = JsonSerializer.Serialize(getWeatherResponseJsonObject, options);

            _logger.LogInformation(prettyGetWeatherResponseJson);
        }

        private async Task AuthenticateAsync()
        {
            var authorizeResponse = await RequestAuthorizationAsync();
            var tokenResponse = await RequestTokenAsync(authorizeResponse);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        }

        private async Task<DeviceAuthorizationResponse> RequestAuthorizationAsync()
        {
            var disco = await _disco.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new HttpClient();

            var response = await client.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
            {
                Address = disco.DeviceAuthorizationEndpoint,
                ClientId = "device",
                ClientSecret = "SuperSecretPassword",
                Scope = "test.api.read"
            });

            if (response.IsError) throw new Exception(response.Error);

            System.Console.WriteLine($"User code:    {response.UserCode}");
            System.Console.WriteLine($"Device code:  {response.DeviceCode}");
            System.Console.WriteLine($"URL:          {response.VerificationUri}");
            System.Console.WriteLine($"Complete URL: {response.VerificationUriComplete}");

            System.Console.WriteLine($"\nPress enter to launch browser ({response.VerificationUriComplete})");
            System.Console.ReadLine();

            Process.Start(new ProcessStartInfo(response.VerificationUriComplete) { UseShellExecute = true });

            return response;
        }

        private async Task<TokenResponse> RequestTokenAsync(DeviceAuthorizationResponse authorizeResponse)
        {
            var disco = await _disco.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new HttpClient();

            while (true)
            {
                var response = await client.RequestDeviceTokenAsync(new DeviceTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "device",
                    ClientSecret = "SuperSecretPassword",
                    DeviceCode = authorizeResponse.DeviceCode
                });

                if (response.IsError)
                {
                    if (response.Error == OidcConstants.TokenErrors.AuthorizationPending || response.Error == OidcConstants.TokenErrors.SlowDown)
                    {
                        System.Console.WriteLine($"{response.Error}...waiting.");
                        await Task.Delay(authorizeResponse.Interval * 1000);
                    }
                    else
                    {
                        throw new Exception(response.Error);
                    }
                }
                else
                {
                    return response;
                }
            }
        }
    }
}
