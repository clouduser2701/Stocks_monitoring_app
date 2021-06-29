using Microsoft.EntityFrameworkCore;
using StocksApp.Server.Data;
using StocksApp.Shared.DTOs;
using StocksApp.Shared.DTOs.Response;
using StocksApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StocksApp.Server.Services
{

    public interface IDbService
    {
        Task AddTicker(TickerDetailsDto tickerDto);
        Task<List<TickerResponseDto>> GetTickers();
        Task<Ticker> GetTickerBySymbol(string symbol);
        Task<EndSession> GetEndsessionByDate(string tickerName, string dateYesterday);
        Task<EndSession> AddEndSession(TickerEndSessionDto resp);
        Task AddArticles(ArticleListDto articleListDto, string tickerSymbol);
        Task AddUserTicker(Claim claimIdUser, Ticker ticker);
        Task AddStocksInfos(TickerStocksInfoDto stocksInfoResp, string tickerSymbol);
        Task<List<StocksInfo>> GetLast90StocksInfos(string tickerSymbol);
        Task<List<ArticleResponseDto>> GetLast5ArticlesForTicker(string tickerSymbol);
        Task<TickerResponseDto> GetTickerResponseBySymbol(string symbol);
        Task<List<TickerResponseDto>> GetUserTickers(Claim claimIdUser);
        Task DeleteUserTicker(Claim claimIdUser, Ticker ticker);
    }
    public class DbService : IDbService
    {
        private readonly ApplicationDbContext _context;

        public DbService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddArticles(ArticleListDto articleListDto, string tickerSymbol)
        {


            try
            {

                List<Article> newArticles = new();
                List<ArticleResponseDto> articlesResponseDto = new();
                Ticker ticker = await GetTickerBySymbol(tickerSymbol);
                if (ticker == null)
                {
                    return;
                }

                List<Article> articlesOfGivenTicker = await _context.Tickers.AsNoTracking()
                                        .Where(t => t.Symbol == tickerSymbol)
                                        .Include(t => t.TickerArticles)
                                            .ThenInclude(tsi => tsi.ArticleIdNavigation)
                                        .Select(t => t.TickerArticles.Select(tsi => tsi.ArticleIdNavigation).ToList())
                                        .FirstAsync();



                foreach (Result result in articleListDto.Results)
                {
                    var alreadyExists = articlesOfGivenTicker.Where(si => si.DatePublished == result.PublishedUtc.UtcDateTime).Any();

                    if (alreadyExists)
                    {
                        continue;
                    }
                    newArticles.Add(
                        new Article
                        {
                            Publisher = result.Publisher.Name,
                            Title = result.Title,
                            DatePublished = result.PublishedUtc.UtcDateTime
                        }
                    );
                }

                await _context.Articles.AddRangeAsync(newArticles);
                await _context.SaveChangesAsync();


                foreach (Article article in newArticles)
                {
                    await _context.TickerArticles.AddAsync(
                        new TickerArticle
                        {
                            TickerId = ticker.TickerId,
                            ArticleId = article.ArticleId
                        }
                    );

                    articlesResponseDto.Add(new ArticleResponseDto
                    {
                        Publisher = article.Publisher,
                        Title = article.Title,
                        DatePublished = article.DatePublished
                    });
                }

                await _context.SaveChangesAsync();


            }
            catch
            {
                return;
            }



        }

        public async Task<EndSession> AddEndSession(TickerEndSessionDto resp)
        {
            EndSession newSession;
            try
            {
                newSession = new()
                {
                    Status = resp.Status,
                    From = resp.From,
                    Symbol = resp.Symbol,
                    Open = resp.Open,
                    High = resp.High,
                    Low = resp.Low,
                    Close = resp.Close,
                    Volume = resp.Volume,
                    AfterHours = resp.AfterHours,
                    PreMarket = resp.PreMarket
                };

                await _context.EndSessions.AddAsync(newSession);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return null;

            }
            return newSession;

        }

        public async Task AddTicker(TickerDetailsDto tickerDto)
        {
            try
            {
                Ticker newTicker = new()
                {
                    Name = tickerDto.Name,
                    Symbol = tickerDto.Symbol,
                    Country = tickerDto.Country,
                    CEO = tickerDto.Ceo,
                    ExchangeSymbol = tickerDto.ExchangeSymbol,
                    Industry = tickerDto.Industry,
                    Tags = string.Join(", ", tickerDto.Tags),
                    LogoLink = tickerDto.Logo
                };

                await _context.Tickers.AddAsync(newTicker);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task<Ticker> GetTickerBySymbol(string symbol)
        {
            try
            {
                return await _context.Tickers.Where(t => t.Symbol == symbol).FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<EndSession> GetEndsessionByDate(string tickerSymbol, string dateYesterday)
        {

            EndSession endSession;
            try
            {
                endSession = await _context.EndSessions.AsNoTracking()
                    .Where(e =>
                            e.Symbol == tickerSymbol
                            &&
                            (dateYesterday.CompareTo(e.From) >= 0)
                        )
                    .FirstOrDefaultAsync();
            }
            catch
            {

                return null;
            }

            return endSession;
        }

        public async Task<List<TickerResponseDto>> GetTickers()
        {
            try
            {
                var tickers = await _context.Tickers.Select(t =>
                        new TickerResponseDto
                        {
                            Name = t.Name,
                            Symbol = t.Symbol,
                            Country = t.Country,
                            CEO = t.CEO,
                            Tags = t.Tags,
                            ExchangeSymbol = t.ExchangeSymbol,
                            Industry = t.Industry,
                            LogoLink = t.LogoLink
                        }
                        ).ToListAsync();

                return tickers;
            }
            catch
            {
                return null;
            }
        }

        public async Task AddUserTicker(Claim claimIdUser, Ticker ticker)
        {
            try
            {
                string userClaimId = claimIdUser.Value;
                bool alredyExists = await _context.UserTickers
                    .Where(ut => ut.UserId == userClaimId && ut.TickerId == ticker.TickerId)
                    .AnyAsync();
                if (alredyExists)
                {
                    return;
                }

                await _context.UserTickers.AddAsync(
                    new UserTicker
                    {
                        UserId = userClaimId,
                        TickerId = ticker.TickerId
                    }
                    );
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task AddStocksInfos(TickerStocksInfoDto stocksInfoResp, string tickerSymbol)
        {
            try
            {
                Ticker ticker = await _context.Tickers.Where(t => t.Symbol == tickerSymbol).FirstOrDefaultAsync();
                if (ticker == null)
                {
                    return;
                }

                List<StocksInfo> infos = new();
                List<StocksInfo> infosOfGivenTicker = await _context.Tickers.AsNoTracking()
                                        .Where(t => t.Symbol == tickerSymbol)
                                        .Include(t => t.TickerStocksInfos)
                                            .ThenInclude(tsi => tsi.StocksInfoIdNavigation)
                                        .Select(t => t.TickerStocksInfos.Select(tsi => tsi.StocksInfoIdNavigation).ToList())
                                        .FirstAsync();



                var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                foreach (var result in stocksInfoResp.Results)
                {
                    var actualDate = unixStart.AddMilliseconds(result.T);

                    var alreadyExists = infosOfGivenTicker.Where(si => si.ActualDate == actualDate).Any();

                    if (alreadyExists)
                    {
                        continue;
                    }

                    infos.Add(new StocksInfo
                    {
                        Vw = result.Vw,
                        O = result.O,
                        C = result.C,
                        H = result.H,
                        L = result.L,
                        DateString = actualDate.ToString("dd MMMM"),
                        N = result.N,
                        ActualDate = actualDate
                    });
                }


                await _context.StocksInfos.AddRangeAsync(infos);
                await _context.SaveChangesAsync();

                foreach (var info in infos)
                {
                    await _context.TickerStockInfos.AddAsync(
                        new TickerStockInfo
                        {
                            TickerId = ticker.TickerId,
                            StocksInfoId = info.StockInfoId
                        });
                }

                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task<List<StocksInfo>> GetLast90StocksInfos(string tickerSymbol)
        {
            try
            {
                List<StocksInfo> infos = await _context.Tickers.AsNoTracking()
                    .Where(t => t.Symbol == tickerSymbol)
                    .Include(t => t.TickerStocksInfos)
                        .ThenInclude(ts => ts.StocksInfoIdNavigation)
                    .Select(
                        t => t.TickerStocksInfos.Select(si => si.StocksInfoIdNavigation).OrderBy(si => si.ActualDate).ToList()
                    ).FirstAsync();

                if (infos.Count > 90)
                {
                    infos = infos.Skip(infos.Count - 90).ToList();
                }

                return infos;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ArticleResponseDto>> GetLast5ArticlesForTicker(string tickerSymbol)
        {
            try
            {
                List<ArticleResponseDto> articles = await _context.Tickers.AsNoTracking()
                    .Where(t => t.Symbol == tickerSymbol)
                    .Include(t => t.TickerArticles)
                        .ThenInclude(ta => ta.ArticleIdNavigation)
                    .Select(
                        t => t.TickerArticles.Select(ta =>
                        new ArticleResponseDto
                        {
                            Publisher = ta.ArticleIdNavigation.Publisher,
                            Title = ta.ArticleIdNavigation.Title,
                            DatePublished = ta.ArticleIdNavigation.DatePublished
                        }).ToList()
                    ).FirstAsync();

                if (articles.Count > 5)
                {
                    articles = articles.Skip(articles.Count - 5).ToList();
                }

                return articles;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<TickerResponseDto>> GetUserTickers(Claim claimIdUser)
        {
            try
            {
                string userClaimId = claimIdUser.Value;

                var user = await _context.Users.Where(u => u.Id == userClaimId).FirstOrDefaultAsync();

                if (user == null)
                {
                    return null;
                }

                List<TickerResponseDto> tickersResponseList = await _context.Users.AsNoTracking()
                    .Where(u => u.Id == userClaimId)
                    .Include(u => u.UserTickers)
                        .ThenInclude(ut => ut.TickerIdNavigation)
                    .Select(
                        u => u.UserTickers.Select(u =>
                        new TickerResponseDto
                        {
                            Name = u.TickerIdNavigation.Name,
                            Symbol = u.TickerIdNavigation.Symbol,
                            Country = u.TickerIdNavigation.Country,
                            CEO = u.TickerIdNavigation.CEO,
                            Tags = u.TickerIdNavigation.Tags,
                            ExchangeSymbol = u.TickerIdNavigation.ExchangeSymbol,
                            Industry = u.TickerIdNavigation.Industry,
                            LogoLink = u.TickerIdNavigation.LogoLink
                        }
                        ).ToList()
                      ).FirstOrDefaultAsync();

                return tickersResponseList;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TickerResponseDto> GetTickerResponseBySymbol(string symbol)
        {
            try
            {
                Ticker ticker = await GetTickerBySymbol(symbol);
                if (ticker == null)
                {
                    return null;
                }

                var tickerResponse = new TickerResponseDto
                {
                    Name = ticker.Name,
                    Symbol = ticker.Symbol,
                    Country = ticker.Country,
                    CEO = ticker.CEO,
                    Tags = ticker.Tags,
                    ExchangeSymbol = ticker.ExchangeSymbol,
                    Industry = ticker.Industry,
                    LogoLink = ticker.LogoLink
                };

                return tickerResponse;
            }
            catch
            {
                return null;
            }

        }

        public async Task DeleteUserTicker(Claim claimIdUser, Ticker ticker)
        {
            try
            {
                string userClaimId = claimIdUser.Value;
                var userTicker = await _context.UserTickers.Where(
                    ut => ut.TickerId == ticker.TickerId && ut.UserId == userClaimId
                    ).FirstOrDefaultAsync();

                _context.UserTickers.Remove(userTicker);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }
    }
}
