using Microsoft.EntityFrameworkCore;
using PortifolioFinanceiro.Infrastructure;
using PortifolioFinanceiro.Models;
using PortifolioFinanceiro.Models.DTOs;
using System.Text.Json;

namespace PortifolioFinanceiro.Services
{
    public class SeedDataService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SeedDataService> _logger;

        public SeedDataService(AppDbContext context, ILogger<SeedDataService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync()
        {
            if (await DatabaseHasDataAsync())
            {
                _logger.LogInformation("Database already contains data. Skipping seed.");
                return;
            }

            _logger.LogInformation("Starting database seeding...");

            var seedData = await LoadSeedDataAsync();
            if (seedData == null)
            {
                _logger.LogError("Seed data could not be loaded.");
                return;
            }

            await SeedUsersAsync(seedData);
            await SeedAssetsAsync(seedData);
            await SeedPriceHistoryAsync(seedData);
            await SeedPortfoliosAsync(seedData);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Database seeding completed successfully.");
        }

        private async Task<bool> DatabaseHasDataAsync()
        {
            return await _context.Assets.AnyAsync();
        }

        private async Task<SeedDataDto?> LoadSeedDataAsync()
        {
            var seedDataPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData.json");
            if (!File.Exists(seedDataPath))
            {
                _logger.LogError("SeedData.json file not found at: {Path}", seedDataPath);
                return null;
            }

            var jsonContent = await File.ReadAllTextAsync(seedDataPath);
            var seedData = JsonSerializer.Deserialize<SeedDataDto>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (seedData == null)
                _logger.LogError("Failed to deserialize SeedData.json");

            return seedData;
        }

        private async Task SeedUsersAsync(SeedDataDto seedData)
        {
            var userIds = seedData.Portfolios.Select(p => p.UserId).Distinct();
            foreach (var userId in userIds)
            {
                _context.Users.Add(new User
                {
                    Id = userId,
                    Name = GetUserNameFromId(userId)
                });
            }
            _logger.LogInformation("Seeded {Count} users", userIds.Count());
        }

        private async Task SeedAssetsAsync(SeedDataDto seedData)
        {
            foreach (var assetDto in seedData.Assets)
            {
                _context.Assets.Add(new Asset
                {
                    Symbol = assetDto.Symbol,
                    Name = assetDto.Name,
                    Type = assetDto.Type,
                    Sector = assetDto.Sector,
                    CurrentPrice = assetDto.CurrentPrice,
                    LastUpdated = assetDto.LastUpdated
                });
            }
            await _context.SaveChangesAsync(); // Após adicionar os Assets
            _logger.LogInformation("Seeded {Count} assets", seedData.Assets.Count);
        }

        private async Task SeedPriceHistoryAsync(SeedDataDto seedData)
        {
            var assets = await _context.Assets.ToListAsync();
            foreach (var (symbol, priceHistory) in seedData.PriceHistory)
            {
                var asset = assets.FirstOrDefault(a => a.Symbol == symbol);
                if (asset == null)
                {
                    _logger.LogWarning("Asset not found for price history: {Symbol}", symbol);
                    continue;
                }

                foreach (var priceDto in priceHistory)
                {
                    if (DateTime.TryParse(priceDto.Date, out var date))
                    {
                        _context.PriceHistories.Add(new PriceHistory
                        {
                            AssetId = asset.Id,
                            AssetSymbol = symbol,
                            Date = date,
                            Price = priceDto.Price
                        });
                    }
                }
            }
            _logger.LogInformation("Seeded price history for {Count} assets", seedData.PriceHistory.Count);
        }

        private async Task SeedPortfoliosAsync(SeedDataDto seedData)
        {
            var assets = await _context.Assets.ToListAsync();
            foreach (var portfolioDto in seedData.Portfolios)
            {
                var portfolio = new Portfolio
                {
                    Name = portfolioDto.Name,
                    UserId = portfolioDto.UserId,
                    TotalInvestment = portfolioDto.TotalInvestment,
                    CreatedAt = portfolioDto.CreatedAt
                };
                _context.Portfolios.Add(portfolio);

                foreach (var positionDto in portfolioDto.Positions)
                {
                    var asset = assets.FirstOrDefault(a => a.Symbol == positionDto.AssetSymbol);
                    if (asset == null)
                    {
                        _logger.LogWarning("Asset not found for position: {Symbol}", positionDto.AssetSymbol);
                        continue;
                    }

                    _context.Positions.Add(new Position
                    {
                        Portfolio = portfolio,
                        AssetId = asset.Id,
                        AssetSymbol = positionDto.AssetSymbol,
                        Quantity = positionDto.Quantity,
                        AveragePrice = positionDto.AveragePrice,
                        TargetAllocation = positionDto.TargetAllocation,
                        LastTransaction = positionDto.LastTransaction
                    });
                }
            }
            _logger.LogInformation("Seeded {Count} portfolios", seedData.Portfolios.Count);
        }

        private static string GetUserNameFromId(string userId) => userId switch
        {
            "user-001" => "Investidor Conservador",
            "user-002" => "Investidor de Crescimento",
            "user-003" => "Investidor de Dividendos",
            _ => $"Usuário {userId}"
        };
    }
}