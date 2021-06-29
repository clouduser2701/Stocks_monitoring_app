using System;
using System.Collections.Generic;

namespace StocksApp.Shared.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        public DateTime DatePublished { get; set; }

        public ICollection<TickerArticle> TickerArticles { get; set; } = new HashSet<TickerArticle>();
    }
}
