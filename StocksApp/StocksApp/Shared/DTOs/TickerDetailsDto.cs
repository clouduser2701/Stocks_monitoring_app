using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.DTOs
{
    public class TickerDetailsDto
    {
        public string Logo { get; set; }
        public string Listdate { get; set; }
        public string Cik { get; set; }
        public string Bloomberg { get; set; }
        public object Figi { get; set; }
        public object Lei { get; set; }
        public int Sic { get; set; }
        public string Country { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }
        public long Marketcap { get; set; }
        public int Employees { get; set; }
        public string Phone { get; set; }
        public string Ceo { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Exchange { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public string HqAddress { get; set; }
        public string HqState { get; set; }
        public string HqCountry { get; set; }
        public string Type { get; set; }
        public string Updated { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Similar { get; set; }
        public bool Active { get; set; }
    }
}
