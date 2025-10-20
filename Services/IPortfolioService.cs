using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Services
{
    public interface IPortfolioService
    {
        Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync();
        Task<IEnumerable<Portfolio>> GetUserPortfoliosAsync(string userId);
        Task<Portfolio?> GetPortfolioByIdAsync(int id);
        Task<Portfolio?> GetPortfolioDetailsAsync(int id);
        Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio);
        Task<Position> AddPositionAsync(int portfolioId, Position position);
        Task<IEnumerable<Position>> AddMultiplePositionsAsync(int portfolioId, IEnumerable<Position> positions);
        Task<Position> UpdatePositionAsync(int portfolioId, int positionId, Position position);
        Task RemovePositionAsync(int portfolioId, int positionId);
    }
}
