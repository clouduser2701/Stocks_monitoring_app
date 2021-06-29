namespace StocksApp.Shared.Models
{
    public class TickerStockInfo
    {
        public int TickerStockInfoId { get; set; }
        public int TickerId { get; set; }
        public Ticker TickerIdNavigation { get; set; }
        public int StocksInfoId { get; set; }
        public StocksInfo StocksInfoIdNavigation { get; set; }
    }
}
