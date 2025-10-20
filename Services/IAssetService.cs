using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<Asset>> GetAllAssetsAsync();
        Task<Asset?> GetAssetByIdAsync(int id);
        Task<Asset?> GetAssetBySymbolAsync(string symbol);
        Task<Asset> CreateAssetAsync(Asset asset);
        Task<Asset> UpdateAssetPriceAsync(int id, decimal newPrice);
    }
}
