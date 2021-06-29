using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Server.Services;
using StocksApp.Shared.DTOs;
using StocksApp.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StocksApp.Server.Controllers
{
    [Route("api/stocksinfos")]
    [Authorize]
    [ApiController]
    public class StocksInfosController : ControllerBase
    {

        private readonly IDbService _dbService;
        private readonly IHttpService _httpService;
        public readonly string ERROR_STATUS = "ERROR";
        public readonly string NOT_FOUND_STATUS = "NOT_FOUND";

        public StocksInfosController(IDbService dbService, IHttpService httpService)
        {
            _dbService = dbService;
            _httpService = httpService;
        }




        [HttpGet("last-3-months/{tickerSymbol}")]
        public async Task<IActionResult> GetLast3MonthsStocksInfo(string tickerSymbol)
        {
            List<StocksInfo> stocksInfos = new();
            try
            {

                TickerStocksInfoDto stocksInfoResp = await _httpService.GetTickerStockInfo(tickerSymbol);

                if (stocksInfoResp.Status != ERROR_STATUS && stocksInfoResp.Status != NOT_FOUND_STATUS && stocksInfoResp != null)
                {
                    await _dbService.AddStocksInfos(stocksInfoResp, tickerSymbol);
                    
                }

                stocksInfos = await _dbService.GetLast90StocksInfos(tickerSymbol);
            }
            catch
            {
                return NotFound();
            }
            if (stocksInfos == null || stocksInfos.Count == 0)
            {
                return NotFound();
            }
            

            return Ok(stocksInfos);
        }






    }
}
