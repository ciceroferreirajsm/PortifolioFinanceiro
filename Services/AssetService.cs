using PortifolioFinanceiro.Models;
using PortifolioFinanceiro.Repositories;

namespace PortifolioFinanceiro.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<AssetService> _logger;

        public AssetService(IAssetRepository assetRepository, ILogger<AssetService> logger)
        {
            _assetRepository = assetRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        {
            try
            {
                return await _assetRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all assets");
                throw;
            }
        }

        public async Task<Asset?> GetAssetByIdAsync(int id)
        {
            try
            {
                return await _assetRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving asset with ID {AssetId}", id);
                throw;
            }
        }

        public async Task<Asset?> GetAssetBySymbolAsync(string symbol)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(symbol))
                    return null;

                return await _assetRepository.GetBySymbolAsync(symbol.ToUpper());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving asset with symbol {Symbol}", symbol);
                throw;
            }
        }

        public async Task<Asset> CreateAssetAsync(Asset asset)
        {
            try
            {
                ValidateAsset(asset);
                asset.Symbol = asset.Symbol.ToUpper();
                asset.LastUpdated = DateTime.UtcNow;
                
                return await _assetRepository.CreateAsync(asset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating asset {Symbol}", asset?.Symbol);
                throw;
            }
        }

        public async Task<Asset> UpdateAssetPriceAsync(int id, decimal newPrice)
        {
            try
            {
                if (newPrice <= 0)
                    throw new ArgumentException("Price must be greater than zero", nameof(newPrice));

                var asset = await _assetRepository.GetByIdAsync(id);
                if (asset == null)
                    throw new KeyNotFoundException($"Asset with ID {id} not found");

                asset.CurrentPrice = newPrice;
                asset.LastUpdated = DateTime.UtcNow;

                return await _assetRepository.UpdateAsync(asset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating price for asset ID {AssetId}", id);
                throw;
            }
        }

        private static void ValidateAsset(Asset asset)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            if (string.IsNullOrWhiteSpace(asset.Symbol))
                throw new ArgumentException("Symbol is required", nameof(asset.Symbol));

            if (string.IsNullOrWhiteSpace(asset.Name))
                throw new ArgumentException("Name is required", nameof(asset.Name));

            if (asset.CurrentPrice <= 0)
                throw new ArgumentException("Current price must be greater than zero", nameof(asset.CurrentPrice));
        }
    }
}
