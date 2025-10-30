using Fomo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fomo.Infrastructure.Persistence
{
    public class EFCoreDbContext : DbContext
    {
        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.TradeResults)
                .WithOne(tr => tr.User)
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TradeResult>()
                .HasOne(tr => tr.TradeMethod)
                .WithOne(tm => tm.TradeResult)
                .HasForeignKey<TradeMethod>(tm => tm.TradeResultId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TradeResult>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Product_EntryPrice_NonNegative", "[EntryPrice] >= 0");
                    t.HasCheckConstraint("CK_Product_ExitPrice_NonNegative", "[ExitPrice] >= 0");
                    t.HasCheckConstraint("CK_Product_NumberOfStocks_NonNegative", "[NumberOfStocks] >= 0");
                });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TradeResult> TradeResults { get; set; }
        public DbSet<TradeMethod> TradeMethods { get; set; }
    }
}