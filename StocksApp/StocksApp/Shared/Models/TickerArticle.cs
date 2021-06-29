using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.Models
{
    public class TickerArticle
    {
        public int TickerArticleId { get; set; }
        public int TickerId { get; set; }
        public Ticker TickerIdNavigation { get; set; }
        public int ArticleId { get; set; }
        public Article ArticleIdNavigation { get; set; }
    }
}
