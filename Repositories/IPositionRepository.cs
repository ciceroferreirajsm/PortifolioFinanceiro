using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Repositories
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> GetByPortfolioIdAsync(int portfolioId);
        Task<Position?> GetByIdAsync(int id);
        Task<Position> CreateAsync(Position position);
        Task<Position> UpdateAsync(Position position);
        Task DeleteAsync(int id);
    }
}
