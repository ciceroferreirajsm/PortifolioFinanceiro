namespace PortifolioFinanceiro.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Symbol { get; set; } // Ex: PETR4
        public string Name { get; set; }
        public string Type { get; set; } // Ex: Stock, Bond, Fund
        public string Sector { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime LastUpdated { get; set; } // Campo presente no SeedData
        public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
        public ICollection<Position> Positions { get; set; } = new List<Position>();
    }
}
