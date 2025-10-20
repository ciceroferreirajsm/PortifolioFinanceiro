using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Repositories
{
    public interface IPortfolioRepository
    {
        Task<IEnumerable<Portfolio>> GetAllAsync();
        Task<IEnumerable<Portfolio>> GetByUserIdAsync(string userId);
        Task<Portfolio?> GetByIdAsync(int id);
        Task<Portfolio?> GetByIdWithPositionsAsync(int id);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> UpdateAsync(Portfolio portfolio);
        Task DeleteAsync(int id);
    }
}
