using PortifolioFinanceiro.Models;
using PortifolioFinanceiro.Repositories;

namespace PortifolioFinanceiro.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<PortfolioService> _logger;

        public PortfolioService(
            IPortfolioRepository portfolioRepository,
            IPositionRepository positionRepository,
            IAssetRepository assetRepository,
            ILogger<PortfolioService> logger)
        {
            _portfolioRepository = portfolioRepository;
            _positionRepository = positionRepository;
            _assetRepository = assetRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            try
            {
                return await _portfolioRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all portfolios");
                throw;
            }
        }

        public async Task<IEnumerable<Portfolio>> GetUserPortfoliosAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return Enumerable.Empty<Portfolio>();

                return await _portfolioRepository.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving portfolios for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Portfolio?> GetPortfolioByIdAsync(int id)
        {
            try
            {
                return await _portfolioRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving portfolio with ID {PortfolioId}", id);
                throw;
            }
        }

        public async Task<Portfolio?> GetPortfolioDetailsAsync(int id)
        {
            try
            {
                return await _portfolioRepository.GetByIdWithPositionsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving portfolio details for ID {PortfolioId}", id);
                throw;
            }
        }

        public async Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio)
        {
            try
            {
                ValidatePortfolio(portfolio);
                portfolio.CreatedAt = DateTime.UtcNow;
                
                return await _portfolioRepository.CreateAsync(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating portfolio {PortfolioName}", portfolio?.Name);
                throw;
            }
        }

        public async Task<Position> AddPositionAsync(int portfolioId, Position position)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetByIdAsync(portfolioId);
                if (portfolio == null)
                    throw new KeyNotFoundException($"Portfolio with ID {portfolioId} not found");

                ValidatePosition(position);
                
                // Verificar se o ativo existe
                var asset = await _assetRepository.GetBySymbolAsync(position.AssetSymbol);
                if (asset == null)
                    throw new KeyNotFoundException($"Asset with ID {position.AssetId} not found");

                position.PortfolioId = portfolioId;
                position.AssetSymbol = asset.Symbol;
                position.LastTransaction = DateTime.UtcNow;

                return await _positionRepository.CreateAsync(position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding position to portfolio {PortfolioId}", portfolioId);
                throw;
            }
        }

        public async Task<Position> UpdatePositionAsync(int portfolioId, int positionId, Position position)
        {
            try
            {
                var existingPosition = await _positionRepository.GetByIdAsync(positionId);
                if (existingPosition == null)
                    throw new KeyNotFoundException($"Position with ID {positionId} not found");

                if (existingPosition.PortfolioId != portfolioId)
                    throw new ArgumentException("Position does not belong to the specified portfolio");

                ValidatePosition(position);

                existingPosition.Quantity = position.Quantity;
                existingPosition.AveragePrice = position.AveragePrice;
                existingPosition.TargetAllocation = position.TargetAllocation;
                existingPosition.LastTransaction = DateTime.UtcNow;

                return await _positionRepository.UpdateAsync(existingPosition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating position {PositionId} in portfolio {PortfolioId}", positionId, portfolioId);
                throw;
            }
        }

        public async Task RemovePositionAsync(int portfolioId, int positionId)
        {
            try
            {
                var position = await _positionRepository.GetByIdAsync(positionId);
                if (position == null)
                    throw new KeyNotFoundException($"Position with ID {positionId} not found");

                if (position.PortfolioId != portfolioId)
                    throw new ArgumentException("Position does not belong to the specified portfolio");

                await _positionRepository.DeleteAsync(positionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing position {PositionId} from portfolio {PortfolioId}", positionId, portfolioId);
                throw;
            }
        }

        public async Task<IEnumerable<Position>> AddMultiplePositionsAsync(int portfolioId, IEnumerable<Position> positions)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetByIdAsync(portfolioId);
                if (portfolio == null)
                    throw new KeyNotFoundException($"Portfolio with ID {portfolioId} not found");

                var createdPositions = new List<Position>();

                foreach (var position in positions)
                {
                    ValidatePosition(position);
                    
                    // Verificar se o ativo existe
                    var asset = await _assetRepository.GetBySymbolAsync(position.AssetSymbol);
                    if (asset == null)
                        throw new KeyNotFoundException($"Asset with symbol {position.AssetSymbol} not found");

                    position.PortfolioId = portfolioId;
                    position.AssetSymbol = asset.Symbol;
                    position.LastTransaction = DateTime.UtcNow;

                    var createdPosition = await _positionRepository.CreateAsync(position);
                    createdPositions.Add(createdPosition);
                }

                return createdPositions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding multiple positions to portfolio {PortfolioId}", portfolioId);
                throw;
            }
        }

        private static void ValidatePortfolio(Portfolio portfolio)
        {
            if (portfolio == null)
                throw new ArgumentNullException(nameof(portfolio));

            if (string.IsNullOrWhiteSpace(portfolio.Name))
                throw new ArgumentException("Portfolio name is required", nameof(portfolio.Name));

            if (string.IsNullOrWhiteSpace(portfolio.UserId))
                throw new ArgumentException("User ID is required", nameof(portfolio.UserId));

            if (portfolio.TotalInvestment < 0)
                throw new ArgumentException("Total investment cannot be negative", nameof(portfolio.TotalInvestment));
        }

        private static void ValidatePosition(Position position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            if (position.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(position.Quantity));

            if (position.AveragePrice <= 0)
                throw new ArgumentException("Average price must be greater than zero", nameof(position.AveragePrice));

            if (position.TargetAllocation < 0 || position.TargetAllocation > 1)
                throw new ArgumentException("Target allocation must be between 0 and 1", nameof(position.TargetAllocation));
        }
    }
}
