using StocksApp.Shared.DTOs.Response;
using StocksApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StocksApp.Client.Services
{

    public interface IApiHttpService
    {
        Task<List<TickerResponseDto>> GetAllTickers();
        Task<TickerResponseDto> GetTickerResponseBySymbol(string tickerSymbol);
        Task<EndSession> GetEndsessionForTicker(string tickerSymbol);
        Task<List<ArticleResponseDto>> GetLastArticlesForTicker(string tickerSymbol);
        Task<List<StocksInfo>> GetLastStocksInfoForTicker(string tickerSymbol);
        Task AddTickerToWatchList(string tickerSymbol);
        Task DeleteTickerFromUser(string tickerSymbol);
        Task<List<TickerResponseDto>> GetTickersOfUser();
    }
    public class ApiHttpService : IApiHttpService
    {

        private readonly HttpClient _httpClient;
        readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        private readonly string API_TICKERS_URL = "https://localhost:44354/api/tickers";
        private readonly string API_ENDSESSION_URL = "https://localhost:44354/api/end-sessions";
        private readonly string API_ARTICLE_URL = "https://localhost:44354/api/articles";
        private readonly string API_STOCKSINFO_URL = "https://localhost:44354/api/stocksinfos";
        private readonly string API_USERS_URL = "https://localhost:44354/api/users";

        public ApiHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TickerResponseDto>> GetAllTickers()
        {

            var response = await _httpClient.GetAsync(API_TICKERS_URL);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TickerResponseDto>>(responseString, options);
        }

        public async Task<TickerResponseDto> GetTickerResponseBySymbol(string tickerSymbol)
        {
            var response = await _httpClient.GetAsync($"{API_TICKERS_URL}/{tickerSymbol}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TickerResponseDto>(responseString, options);
        }

        public async Task<EndSession> GetEndsessionForTicker(string tickerSymbol)
        {
            var response = await _httpClient.GetAsync($"{API_ENDSESSION_URL}/prev-day/{tickerSymbol}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EndSession>(responseString, options);
        }

        public async Task<List<ArticleResponseDto>> GetLastArticlesForTicker(string tickerSymbol)
        {
            var response = await _httpClient.GetAsync($"{API_ARTICLE_URL}/last-5/{tickerSymbol}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ArticleResponseDto>>(responseString, options);
        }

        public async Task<List<StocksInfo>> GetLastStocksInfoForTicker(string tickerSymbol)
        {
            var response = await _httpClient.GetAsync($"{API_STOCKSINFO_URL}/last-3-months/{tickerSymbol}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<StocksInfo>>(responseString, options);
        }

        public async Task AddTickerToWatchList(string tickerSymbol)
        {
            await _httpClient.PostAsync($"{API_USERS_URL}/add-ticker-to-user/{tickerSymbol}", null);
        }

        public async Task DeleteTickerFromUser(string tickerSymbol)
        {
            await _httpClient.DeleteAsync($"{API_USERS_URL}/delete-ticker-from-user/{tickerSymbol}");
        }

        public async Task<List<TickerResponseDto>> GetTickersOfUser()
        {
            var response = await _httpClient.GetAsync($"{API_USERS_URL}/get-user-tickers");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TickerResponseDto>>(responseString, options);
        }
    }
}
