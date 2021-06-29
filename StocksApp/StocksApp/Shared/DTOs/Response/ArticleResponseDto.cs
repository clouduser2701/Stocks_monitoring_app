using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Shared.DTOs.Response
{
    public class ArticleResponseDto
    {
        
        public string Publisher { get; set; }
        public string Title { get; set; }
        public DateTime DatePublished { get; set; }
    }
}
