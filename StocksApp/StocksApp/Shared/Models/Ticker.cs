using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.Models
{
    public class Ticker
    {
        public int TickerId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Country { get; set; }
        public string CEO { get; set; }
        public string Tags { get; set; }
        public string ExchangeSymbol { get; set; }
        public string Industry { get; set; }
        public string LogoLink { get; set; }

        public ICollection<TickerEndSession> TickerEndSessions { get; set; } = new HashSet<TickerEndSession>();

        public ICollection<TickerStockInfo> TickerStocksInfos { get; set; } = new HashSet<TickerStockInfo>();

        public ICollection<UserTicker> UserTickers { get; set; } = new HashSet<UserTicker>();

        public ICollection<TickerArticle> TickerArticles { get; set; } = new HashSet<TickerArticle>();
    }
}
