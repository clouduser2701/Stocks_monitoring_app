using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.Models
{
    public class TickerEndSession
    {
        public int TickerEndSessionId { get; set; }
        public int TickerId { get; set; }
        public Ticker TickerIdNavigation { get; set; }
        public int EndSessionId { get; set; }
        public EndSession EndSessionIdNavigation { get; set; }
    }
}
