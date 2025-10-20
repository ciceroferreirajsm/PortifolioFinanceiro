namespace PortifolioFinanceiro.Models.DTOs
{
    public class SeedDataDto
    {
        public List<AssetSeedDto> Assets { get; set; } = new();
        public List<PortfolioSeedDto> Portfolios { get; set; } = new();
        public Dictionary<string, List<PriceHistorySeedDto>> PriceHistory { get; set; } = new();
        public MarketDataDto MarketData { get; set; } = new();
        public List<TestScenarioDto> TestScenarios { get; set; } = new();
    }

    public class AssetSeedDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Sector { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class PortfolioSeedDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public decimal TotalInvestment { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PositionSeedDto> Positions { get; set; } = new();
    }

    public class PositionSeedDto
    {
        public string AssetSymbol { get; set; }
        public int Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TargetAllocation { get; set; }
        public DateTime? LastTransaction { get; set; }
    }

    public class PriceHistorySeedDto
    {
        public string Date { get; set; }
        public decimal Price { get; set; }
    }

    public class MarketDataDto
    {
        public decimal SelicRate { get; set; }
        public IndexPerformanceDto IndexPerformance { get; set; } = new();
        public List<SectorDto> Sectors { get; set; } = new();
    }

    public class IndexPerformanceDto
    {
        public IBOVDto IBOV { get; set; } = new();
    }

    public class IBOVDto
    {
        public decimal CurrentValue { get; set; }
        public decimal DailyChange { get; set; }
        public decimal MonthlyChange { get; set; }
        public decimal YearToDate { get; set; }
    }

    public class SectorDto
    {
        public string Name { get; set; }
        public decimal AverageReturn { get; set; }
        public decimal Volatility { get; set; }
        public List<string> Assets { get; set; } = new();
    }

    public class TestScenarioDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PortfolioSeedDto Portfolio { get; set; } = new();
        public ExpectedResultsDto ExpectedResults { get; set; } = new();
    }

    public class ExpectedResultsDto
    {
        public decimal? TotalValue { get; set; }
        public Dictionary<string, decimal>? Allocations { get; set; }
        public bool? RebalancingNeeded { get; set; }
        public List<SuggestedActionDto>? SuggestedActions { get; set; }
        public decimal? ConcentrationRisk { get; set; }
        public string? RiskLevel { get; set; }
        public List<string>? Alerts { get; set; }
        public decimal? TotalReturn { get; set; }
        public decimal? AnnualizedReturn { get; set; }
        public decimal? SharpeRatio { get; set; }
        public decimal? Volatility { get; set; }
    }

    public class SuggestedActionDto
    {
        public string Action { get; set; }
        public string Asset { get; set; }
        public decimal Value { get; set; }
    }
}
