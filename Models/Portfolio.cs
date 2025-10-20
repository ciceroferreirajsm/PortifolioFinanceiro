namespace PortifolioFinanceiro.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal TotalInvestment { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Position> Positions { get; set; } = new List<Position>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
