using Newtonsoft.Json;
using StocksApp.Shared.DTOs;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StocksApp.Server.Services
{
    public interface IHttpService
    {
        Task<TickerEndSessionDto> GetTickerEndSession(string tickerSymbol, string dateYesterdayString);
        Task<ArticleListDto> GetArticlesResponse(string tickerSymbol);

        Task<TickerStocksInfoDto> GetTickerStockInfo(string tickerSymbol);

        Task<TickerDetailsDto> GetTickerBySymbol(string tickerSymbol);
    }
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "iRpH4IWYoCYjfi7_gPS8U_z0DyvQ0dUB";
        public HttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<TickerEndSessionDto> GetTickerEndSession(string tickerSymbol, string dateYesterdayString)
        {
            var url = $"https://api.polygon.io/v1/open-close/{tickerSymbol.ToUpper()}/{dateYesterdayString}?unadjusted=true&apiKey={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            TickerEndSessionDto resp = JsonConvert.DeserializeObject<TickerEndSessionDto>(responseString);
            return resp;
        }

        public async Task<ArticleListDto> GetArticlesResponse(string tickerSymbol)
        {
            var url = $"https://api.polygon.io/v2/reference/news?limit=5&order=descending&sort=published_utc&ticker={tickerSymbol.ToUpper()}&apiKey={_apiKey}";
            var response = await new HttpClient().GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            ArticleListDto resp = JsonConvert.DeserializeObject<ArticleListDto>(responseString);
            return resp;
        }

        public async Task<TickerStocksInfoDto> GetTickerStockInfo(string tickerSymbol)
        {
            var dateTo = DateTime.Now.ToString(@"yyyy-MM-dd");
            var dateFrom = DateTime.Now.AddDays(-90).ToString(@"yyyy-MM-dd");
            var url = $"https://api.polygon.io/v2/aggs/ticker/{tickerSymbol.ToUpper()}/range/1/day/{dateFrom}/{dateTo}?unadjusted=true&sort=asc&limit=120&apiKey={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            TickerStocksInfoDto stocksInfoResp = JsonConvert.DeserializeObject<TickerStocksInfoDto>(responseString);
            return stocksInfoResp;
        }

        public async Task<TickerDetailsDto> GetTickerBySymbol(string tickerSymbol)
        {
            var response = await _httpClient.GetAsync($"https://api.polygon.io/v1/meta/symbols/{tickerSymbol.ToUpper()}/company?&apiKey={_apiKey}");
            var responseString = await response.Content.ReadAsStringAsync();
            TickerDetailsDto resp = JsonConvert.DeserializeObject<TickerDetailsDto>(responseString);
            return resp;
        }
    }
}
