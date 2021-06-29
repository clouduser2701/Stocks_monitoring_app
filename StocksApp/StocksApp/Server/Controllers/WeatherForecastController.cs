using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StocksApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using StocksApp.Shared.DTOs;

namespace StocksApp.Server.Controllers
{

    [ApiController]
    [Route("api/weather")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var response = await new HttpClient().GetAsync("https://api.polygon.io/v1/meta/symbols/TSLA/company?&apiKey=iRpH4IWYoCYjfi7_gPS8U_z0DyvQ0dUB");
            var responseString = await response.Content.ReadAsStringAsync();
            TickerDetailsDto resp = JsonSerializer.Deserialize<TickerDetailsDto>(responseString, options);
            return Ok(resp);
        }

    }
}
