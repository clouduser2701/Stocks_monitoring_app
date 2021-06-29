using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Server.Services;
using StocksApp.Shared.DTOs;
using StocksApp.Shared.DTOs.Response;
using StocksApp.Shared.Models;

namespace StocksApp.Server.Controllers
{
    [Route("api/tickers")]
    [Authorize]
    [ApiController]
    public class TickersController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly IHttpService _httpService;

        public TickersController(IDbService dbService, IHttpService httpService)
        {
            _dbService = dbService;
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickers()
        {
            return Ok(await _dbService.GetTickers());
        }



        [HttpGet("{symbol}")]
        public async Task<ActionResult> GetTicker(string symbol)
        {

            TickerResponseDto ticker = await _dbService.GetTickerResponseBySymbol(symbol);

            if (ticker == null)
            {
                return NotFound();
            }

            return Ok(ticker);
        }

        [HttpPost("{tickerSymbol}")]
        public async Task<ActionResult<Ticker>> PostTicker(string tickerSymbol)
        {
            try
            {
                TickerDetailsDto resp = await _httpService.GetTickerBySymbol(tickerSymbol);

                await _dbService.AddTicker(resp);
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }

        

       


    }
}
