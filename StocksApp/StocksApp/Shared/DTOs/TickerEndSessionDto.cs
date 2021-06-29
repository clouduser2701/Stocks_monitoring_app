using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.DTOs
{
    public class TickerEndSessionDto
    {
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
    }
}
