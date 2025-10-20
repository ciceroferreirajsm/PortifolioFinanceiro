using PortifolioFinanceiro.Models;
using PortifolioFinanceiro.Models.DTOs;

namespace PortifolioFinanceiro.Services
{
    public class MappingService
    {
        public static AssetDto MapToAssetDto(Asset asset)
        {
            return new AssetDto
            {
                Id = asset.Id,
                Symbol = asset.Symbol,
                Name = asset.Name,
                Type = asset.Type,
                Sector = asset.Sector,
                CurrentPrice = asset.CurrentPrice,
                LastUpdated = asset.LastUpdated,
                PriceHistory = asset.PriceHistories?.Select(ph => new PriceHistoryDto
                {
                    Date = ph.Date,
                    Price = ph.Price
                }).OrderBy(ph => ph.Date).ToList()
            };
        }

        public static Asset MapToAsset(CreateAssetDto createAssetDto)
        {
            return new Asset
            {
                Symbol = createAssetDto.Symbol,
                Name = createAssetDto.Name,
                Type = createAssetDto.Type,
                Sector = createAssetDto.Sector,
                CurrentPrice = createAssetDto.CurrentPrice,
                LastUpdated = DateTime.UtcNow
            };
        }

        public static PortfolioSummaryDto MapToPortfolioSummaryDto(Portfolio portfolio)
        {
            var currentValue = CalculatePortfolioCurrentValue(portfolio);
            var returnPercentage = portfolio.TotalInvestment > 0 
                ? ((currentValue - portfolio.TotalInvestment) / portfolio.TotalInvestment) * 100 
                : 0;

            return new PortfolioSummaryDto
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                TotalInvestment = portfolio.TotalInvestment,
                CurrentValue = currentValue,
                ReturnPercentage = returnPercentage,
                CreatedAt = portfolio.CreatedAt,
                PositionsCount = portfolio.Positions?.Count ?? 0
            };
        }

        public static PortfolioDto MapToPortfolioDto(Portfolio portfolio)
        {
            var currentValue = CalculatePortfolioCurrentValue(portfolio);
            var totalReturn = currentValue - portfolio.TotalInvestment;
            var returnPercentage = portfolio.TotalInvestment > 0 
                ? (totalReturn / portfolio.TotalInvestment) * 100 
                : 0;

            return new PortfolioDto
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                UserId = portfolio.UserId,
                UserName = portfolio.User?.Name ?? "",
                TotalInvestment = portfolio.TotalInvestment,
                CurrentValue = currentValue,
                TotalReturn = totalReturn,
                ReturnPercentage = returnPercentage,
                CreatedAt = portfolio.CreatedAt,
                Positions = portfolio.Positions?.Select(p => MapToPositionDto(p, currentValue)).ToList() ?? new List<PositionDto>()
            };
        }

        public static Portfolio MapToPortfolio(CreatePortfolioDto createPortfolioDto)
        {
            return new Portfolio
            {
                Name = createPortfolioDto.Name,
                UserId = createPortfolioDto.UserId,
                TotalInvestment = createPortfolioDto.TotalInvestment,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static PositionDto MapToPositionDto(Position position, decimal portfolioCurrentValue = 0)
        {
            var currentValue = position.Quantity * (position.Asset?.CurrentPrice ?? 0);
            var investedValue = position.Quantity * position.AveragePrice;
            var positionReturn = currentValue - investedValue;
            var returnPercentage = investedValue > 0 ? (positionReturn / investedValue) * 100 : 0;
            var currentAllocation = portfolioCurrentValue > 0 ? (currentValue / portfolioCurrentValue) : 0;

            return new PositionDto
            {
                Id = position.Id,
                AssetSymbol = position.AssetSymbol,
                AssetName = position.Asset?.Name ?? "",
                AssetSector = position.Asset?.Sector ?? "",
                Quantity = position.Quantity,
                AveragePrice = position.AveragePrice,
                CurrentPrice = position.Asset?.CurrentPrice ?? 0,
                CurrentValue = currentValue,
                InvestedValue = investedValue,
                Return = positionReturn,
                ReturnPercentage = returnPercentage,
                TargetAllocation = position.TargetAllocation,
                CurrentAllocation = currentAllocation,
                LastTransaction = position.LastTransaction
            };
        }

        public static Position MapToPosition(CreatePositionDto createPositionDto)
        {
            return new Position
            {
                AssetSymbol = createPositionDto.AssetSymbol,
                Quantity = createPositionDto.Quantity,
                AveragePrice = createPositionDto.AveragePrice,
                TargetAllocation = createPositionDto.TargetAllocation,
                LastTransaction = DateTime.UtcNow
            };
        }

        private static decimal CalculatePortfolioCurrentValue(Portfolio portfolio)
        {
            return portfolio.Positions?.Sum(p => p.Quantity * (p.Asset?.CurrentPrice ?? 0)) ?? 0;
        }
    }
}
