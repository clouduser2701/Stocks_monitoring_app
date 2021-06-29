using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.Models
{
    public class StocksInfo
    {
        public int StockInfoId { get; set; }
        public double Vw { get; set; }
        public double O { get; set; }
        public double C { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public string DateString { get; set; }
        public int N { get; set; }
        public DateTime ActualDate { get; set; }

        public ICollection<TickerStockInfo> TickerStocksInfos { get; set; } = new HashSet<TickerStockInfo>();
    }
}
