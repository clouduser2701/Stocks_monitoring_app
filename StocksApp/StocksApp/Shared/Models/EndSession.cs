using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.Models
{
    public class EndSession
    {
        public int EndSessionId { get; set; }
        public string Status { get; set; }
        public string From { get; set; }
        public string Symbol { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
        public double AfterHours { get; set; }
        public double PreMarket { get; set; }

        public ICollection<TickerEndSession> TickerEndSessions { get; set; } = new HashSet<TickerEndSession>();
    }
}
