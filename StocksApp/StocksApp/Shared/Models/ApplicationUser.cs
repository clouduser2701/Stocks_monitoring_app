using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace StocksApp.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserTicker> UserTickers { get; set; } = new HashSet<UserTicker>();
    }
}
