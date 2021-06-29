using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Server.Services;
using StocksApp.Shared.DTOs;
using StocksApp.Shared.Models;
using System;
using System.Net.Http;

using System.Threading.Tasks;

namespace StocksApp.Server.Controllers
{
    [Route("api/end-sessions")]
    [Authorize]
    [ApiController]
    public class EndSessionsController : ControllerBase
    {
        //  /prev-day


        private readonly IDbService _dbService;
        private readonly IHttpService _httpService;
        public readonly string ERROR_STATUS = "ERROR";
        public readonly string NOT_FOUND_STATUS = "NOT_FOUND";

        public EndSessionsController(IDbService dbService, IHttpService httpService)
        {
            _dbService = dbService;
            _httpService = httpService;
        }


        [HttpGet("prev-day/{tickerSymbol}")]
        public async Task<IActionResult> GetEndession(string tickerSymbol)
        {
            var dateYesterday = DateTime.Now.AddDays(-1);
            var dateYesterdayString = dateYesterday.ToString(@"yyyy-MM-dd");
            EndSession endSession;

            try
            {
                endSession = await _dbService.GetEndsessionByDate(tickerSymbol, dateYesterdayString);

                if (endSession == null)
                {
                    dateYesterday = DateTime.Now.AddDays(-2);
                    dateYesterdayString = dateYesterday.ToString(@"yyyy-MM-dd");

                    endSession = await _dbService.GetEndsessionByDate(tickerSymbol, dateYesterdayString);

                }

                if (endSession == null)
                {
                    EndSession newEndsession;
                    HttpClient httpClient = new();

                    int subDays = 1;
                    bool something_found = false;
                    while (subDays < 5 && !something_found)
                    {
                        dateYesterday = DateTime.Now.AddDays(-subDays);
                        dateYesterdayString = dateYesterday.ToString(@"yyyy-MM-dd");
                        TickerEndSessionDto tickerEndsession = await _httpService.GetTickerEndSession(tickerSymbol, dateYesterdayString);

                        if (tickerEndsession.Status != ERROR_STATUS && tickerEndsession.Status != NOT_FOUND_STATUS && tickerEndsession != null)
                        {
                            newEndsession = await _dbService.AddEndSession(tickerEndsession);
                            something_found = true;
                        }
                        subDays++;

                    }
                }
            }
            catch
            {
                return NotFound();
            }

            if (endSession == null)
            {
                return NotFound();
            }
            return Ok(endSession);

        }
    }
}
