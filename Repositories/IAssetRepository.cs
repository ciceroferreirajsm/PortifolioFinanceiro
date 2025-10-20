using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Repositories
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<Asset?> GetByIdAsync(int id);
        Task<Asset?> GetBySymbolAsync(string symbol);
        Task<Asset> CreateAsync(Asset asset);
        Task<Asset> UpdateAsync(Asset asset);
        Task DeleteAsync(int id);
    }
}
