using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Server.Services;
using StocksApp.Shared.DTOs;
using StocksApp.Shared.DTOs.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StocksApp.Server.Controllers
{
    [Authorize]
    [Route("api/articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private readonly IDbService _dbService;
        private readonly IHttpService _httpService;
        public readonly string ERROR_STATUS = "ERROR";
        public readonly string NOT_FOUND_STATUS = "NOT_FOUND";

        public ArticlesController(IDbService dbService, IHttpService httpService)
        {
            _dbService = dbService;
            _httpService = httpService;
        }


        [HttpGet("last-5/{tickerSymbol}")]
        public async Task<IActionResult> GetLastFiveArticles(string tickerSymbol)
        {
            List<ArticleResponseDto> articles = new();
            try
            {
                ArticleListDto articleList = await _httpService.GetArticlesResponse(tickerSymbol);

                if (articleList != null && articleList.Status != NOT_FOUND_STATUS && articleList.Status != ERROR_STATUS)
                {
                    await _dbService.AddArticles(articleList, tickerSymbol);
                }


                articles = await _dbService.GetLast5ArticlesForTicker(tickerSymbol);

                if (articles == null || articles.Count == 0)
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }

            return Ok(articles);

        }





    }
}

