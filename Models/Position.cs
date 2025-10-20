namespace PortifolioFinanceiro.Models
{
    public class Position
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public string AssetSymbol { get; set; } // Para relacionar com o Symbol do Asset
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public int Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TargetAllocation { get; set; } // Percentual de alocação desejada (0.0 a 1.0)
        public DateTime? LastTransaction { get; set; }
    }
}
