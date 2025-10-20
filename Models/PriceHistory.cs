namespace PortifolioFinanceiro.Models
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public string AssetSymbol { get; set; } // Para facilitar o mapeamento com o SeedData
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
