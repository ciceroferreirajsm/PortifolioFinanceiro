namespace PortifolioFinanceiro.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public string AssetSymbol { get; set; } // Para facilitar o mapeamento
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; } // BUY/SELL
    }
}
