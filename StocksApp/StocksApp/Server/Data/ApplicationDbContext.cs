using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StocksApp.Shared.Models;

namespace StocksApp.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {

        public DbSet<Ticker> Tickers { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<EndSession> EndSessions { get; set; }
        public DbSet<StocksInfo> StocksInfos { get; set; }
        public DbSet<TickerArticle> TickerArticles { get; set; }
        public DbSet<TickerEndSession> TickerEndSessions { get; set; }
        public DbSet<TickerStockInfo> TickerStockInfos { get; set; }
        public DbSet<UserTicker> UserTickers { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticker>(entity =>
            {
                entity.ToTable(nameof(Ticker));

                entity.HasKey(e => e.TickerId);
                entity.Property(e => e.TickerId).ValueGeneratedOnAdd();

                entity.HasIndex(e => e.Symbol).IsUnique();
                entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);

                entity.Property(e => e.Country).IsRequired().HasMaxLength(100);

                entity.Property(e => e.CEO).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Tags).IsRequired().HasMaxLength(200);

                entity.Property(e => e.LogoLink).IsRequired();

            });

            builder.Entity<Article>(entity =>
            {
                entity.ToTable(nameof(Article));

                entity.HasKey(e => e.ArticleId);
                entity.Property(e => e.ArticleId).ValueGeneratedOnAdd();

                entity.Property(e => e.Publisher).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

                entity.Property(e => e.DatePublished).HasColumnType("datetime").IsRequired();
            });

            builder.Entity<StocksInfo>(entity =>
            {
                entity.ToTable(nameof(StocksInfo));

                entity.HasKey(e => e.StockInfoId);
                entity.Property(e => e.StockInfoId).ValueGeneratedOnAdd();

                //entity.Property(e => e.V).HasColumnType("decimal(10,4)").IsRequired();

                entity.Property(e => e.Vw).HasColumnType("decimal(10,4)").IsRequired();

                entity.Property(e => e.O).HasColumnType("decimal(10,4)").IsRequired();

                entity.Property(e => e.C).HasColumnType("decimal(10,4)").IsRequired();

                entity.Property(e => e.H).HasColumnType("decimal(10,4)").IsRequired();

                entity.Property(e => e.L).HasColumnType("decimal(10,4)").IsRequired();

                entity.Property(e => e.DateString).HasMaxLength(20).IsRequired();

                entity.Property(e => e.N).IsRequired();

                entity.Property(e => e.ActualDate).HasColumnType("datetime").IsRequired();

            });

            builder.Entity<EndSession>(entity =>
            {
                entity.ToTable(nameof(EndSession));

                entity.HasKey(e => e.EndSessionId);
                entity.Property(e => e.EndSessionId).ValueGeneratedOnAdd();

                entity.Property(e => e.Status).IsRequired().HasMaxLength(100);

                entity.Property(e => e.From).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Symbol).IsRequired().HasMaxLength(20);

                entity.Property(e => e.Open).HasColumnType("decimal(10,2)").IsRequired();

                entity.Property(e => e.High).HasColumnType("decimal(10,2)").IsRequired();

                entity.Property(e => e.Low).HasColumnType("decimal(10,2)").IsRequired();

                entity.Property(e => e.Close).HasColumnType("decimal(10,2)").IsRequired();

                entity.Property(e => e.Volume).IsRequired().HasColumnType("bigint");

                entity.Property(e => e.AfterHours).HasColumnType("decimal(10,2)").IsRequired();

                entity.Property(e => e.PreMarket ).HasColumnType("decimal(10,2)").IsRequired();

            });

            builder.Entity<UserTicker>(entity =>
            {
                entity.ToTable(nameof(UserTicker));

                entity.HasKey(e => e.UserTickerId);
                entity.Property(e => e.UserTickerId).ValueGeneratedOnAdd();

                entity.HasOne(e => e.TickerIdNavigation)
                    .WithMany(t => t.UserTickers)
                    .HasForeignKey(e => e.TickerId);

                entity.HasOne(e => e.UserIdNavigation)
                    .WithMany(u => u.UserTickers)
                    .HasForeignKey(e => e.UserId);
            });

            builder.Entity<TickerArticle>(entity =>
            {
                entity.ToTable(nameof(TickerArticle));

                entity.HasKey(e => e.TickerArticleId);
                entity.Property(e => e.TickerArticleId).ValueGeneratedOnAdd();

                entity.HasOne(e => e.ArticleIdNavigation)
                    .WithMany(a => a.TickerArticles)
                    .HasForeignKey(e => e.ArticleId);

                entity.HasOne(e => e.TickerIdNavigation)
                    .WithMany(t => t.TickerArticles)
                    .HasForeignKey(e => e.TickerId);
            });

            builder.Entity<TickerEndSession>(entity =>
            {
                entity.ToTable(nameof(TickerEndSession));

                entity.HasKey(e => e.TickerEndSessionId);
                entity.Property(e => e.TickerEndSessionId).ValueGeneratedOnAdd();

                entity.HasOne(e => e.TickerIdNavigation)
                    .WithMany(a => a.TickerEndSessions)
                    .HasForeignKey(e => e.TickerId);

                entity.HasOne(e => e.EndSessionIdNavigation)
                    .WithMany(t => t.TickerEndSessions)
                    .HasForeignKey(e => e.EndSessionId);
            });

            builder.Entity<TickerStockInfo>(entity =>
            {
                entity.ToTable(nameof(TickerStockInfo));

                entity.HasKey(e => e.TickerStockInfoId);
                entity.Property(e => e.TickerStockInfoId).ValueGeneratedOnAdd();

                entity.HasOne(e => e.TickerIdNavigation)
                    .WithMany(a => a.TickerStocksInfos)
                    .HasForeignKey(e => e.TickerId);

                entity.HasOne(e => e.StocksInfoIdNavigation)
                    .WithMany(t => t.TickerStocksInfos)
                    .HasForeignKey(e => e.StocksInfoId);
            });

        }
    }
}
