using Microsoft.EntityFrameworkCore;
using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da entidade User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Configuração da entidade Asset
            modelBuilder.Entity<Asset>()
                .HasKey(a => a.Id);
            
            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.Symbol)
                .IsUnique();

            // Configuração da entidade Portfolio
            modelBuilder.Entity<Portfolio>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.User)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração da entidade Position
            modelBuilder.Entity<Position>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.Portfolio)
                .WithMany(pf => pf.Positions)
                .HasForeignKey(p => p.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.Asset)
                .WithMany(a => a.Positions)
                .HasForeignKey(p => p.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da entidade Transaction
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Portfolio)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Asset)
                .WithMany()
                .HasForeignKey(t => t.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da entidade PriceHistory
            modelBuilder.Entity<PriceHistory>()
                .HasKey(ph => ph.Id);

            modelBuilder.Entity<PriceHistory>()
                .HasOne(ph => ph.Asset)
                .WithMany(a => a.PriceHistories)
                .HasForeignKey(ph => ph.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurações de precisão decimal
            modelBuilder.Entity<Asset>()
                .Property(a => a.CurrentPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Portfolio>()
                .Property(p => p.TotalInvestment)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Position>()
                .Property(p => p.AveragePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Position>()
                .Property(p => p.TargetAllocation)
                .HasPrecision(5, 4); // Para percentuais de 0.0000 a 1.0000

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PriceHistory>()
                .Property(ph => ph.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}