namespace StocksApp.Shared.Models
{
    public class UserTicker
    {
        public int UserTickerId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser UserIdNavigation { get; set; }
        public int TickerId { get; set; }
        public Ticker TickerIdNavigation { get; set; }
    }
}
