namespace PortifolioFinanceiro.Models
{
    public class User
    {
        public string Id { get; set; } // String para corresponder ao formato "user-001" do SeedData
        public string Name { get; set; }
        public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
