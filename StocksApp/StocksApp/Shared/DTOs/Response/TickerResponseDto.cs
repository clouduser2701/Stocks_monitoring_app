using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.DTOs.Response
{
    public class TickerResponseDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Country { get; set; }
        public string CEO { get; set; }
        public string Tags { get; set; }
        public string ExchangeSymbol { get; set; }
        public string Industry { get; set; }
        public string LogoLink { get; set; }
    }
}
