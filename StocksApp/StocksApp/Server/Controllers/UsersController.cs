using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Server.Services;
using StocksApp.Shared.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StocksApp.Server.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IDbService _dbService;

        public UsersController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetCLientId()
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var claimIdUser = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            return Ok(claimIdUser.Value);

        }

        [HttpPost("add-ticker-to-user/{tickerSymbol}")]
        public async Task<IActionResult> PostUserTicker(string tickerSymbol)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var claimIdUser = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                var ticker = await _dbService.GetTickerBySymbol(tickerSymbol);
                if (ticker == null)
                {
                    return BadRequest();
                }

                await _dbService.AddUserTicker(claimIdUser, ticker);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("delete-ticker-from-user/{tickerSymbol}")]
        public async Task<IActionResult> DeleteUserTicker(string tickerSymbol)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var claimIdUser = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                var ticker = await _dbService.GetTickerBySymbol(tickerSymbol);
                if (ticker == null)
                {
                    return BadRequest();
                }

                await _dbService.DeleteUserTicker(claimIdUser, ticker);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }


        [HttpGet("get-user-tickers")]
        public async Task<IActionResult> GetUsersTickers()
        {
            List<TickerResponseDto> tickersResponse;

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var claimIdUser = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                tickersResponse = await _dbService.GetUserTickers(claimIdUser);
                if (tickersResponse == null)
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }


            return Ok(tickersResponse);
        }
    }
}
