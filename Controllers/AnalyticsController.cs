using Microsoft.AspNetCore.Mvc;
using PortifolioFinanceiro.Services;

namespace PortifolioFinanceiro.Controllers
{
    [ApiController]
    [Route("api/portfolios/{id}")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(IPortfolioService portfolioService, ILogger<AnalyticsController> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        /// <summary>
        /// Retorna análise de performance do portfólio
        /// </summary>
        [HttpGet("performance")]
        public async Task<IActionResult> GetPerformance(int id)
        {
            try
            {
                var portfolio = await _portfolioService.GetPortfolioDetailsAsync(id);
                if (portfolio == null)
                {
                    return NotFound($"Portfolio with ID {id} not found");
                }

                var portfolioDto = MappingService.MapToPortfolioDto(portfolio);
                
                var performance = new
                {
                    portfolioId = id,
                    portfolioName = portfolio.Name,
                    totalInvestment = portfolio.TotalInvestment,
                    currentValue = portfolioDto.CurrentValue,
                    totalReturn = portfolioDto.TotalReturn,
                    returnPercentage = portfolioDto.ReturnPercentage,
                    createdAt = portfolio.CreatedAt,
                    positions = portfolioDto.Positions.Select(p => new
                    {
                        asset = p.AssetSymbol,
                        quantity = p.Quantity,
                        averagePrice = p.AveragePrice,
                        currentPrice = p.CurrentPrice,
                        investedValue = p.InvestedValue,
                        currentValue = p.CurrentValue,
                        positionReturn = p.Return,
                        returnPercentage = p.ReturnPercentage
                    }).ToList()
                };

                return Ok(performance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving performance for portfolio {PortfolioId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retorna análise de risco do portfólio
        /// </summary>
        [HttpGet("risk-analysis")]
        public async Task<IActionResult> GetRiskAnalysis(int id)
        {
            try
            {
                var portfolio = await _portfolioService.GetPortfolioDetailsAsync(id);
                if (portfolio == null)
                {
                    return NotFound($"Portfolio with ID {id} not found");
                }

                var portfolioDto = MappingService.MapToPortfolioDto(portfolio);
                
                var riskAnalysis = new
                {
                    portfolioId = id,
                    portfolioName = portfolio.Name,
                    diversification = new
                    {
                        totalAssets = portfolioDto.Positions.Count,
                        sectorDistribution = portfolioDto.Positions
                            .GroupBy(p => p.AssetSector)
                            .Select(g => new
                            {
                                sector = g.Key,
                                assetsCount = g.Count(),
                                totalAllocation = g.Sum(p => p.CurrentAllocation)
                            }).ToList(),
                        concentrationRisk = portfolioDto.Positions.Count > 0 
                            ? portfolioDto.Positions.Max(p => p.CurrentAllocation)
                            : 0
                    },
                    allocations = portfolioDto.Positions.Select(p => new
                    {
                        asset = p.AssetSymbol,
                        sector = p.AssetSector,
                        targetAllocation = p.TargetAllocation,
                        currentAllocation = p.CurrentAllocation,
                        deviation = Math.Abs(p.TargetAllocation - p.CurrentAllocation)
                    }).ToList()
                };

                return Ok(riskAnalysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving risk analysis for portfolio {PortfolioId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retorna sugestões de rebalanceamento do portfólio
        /// </summary>
        [HttpGet("rebalancing")]
        public async Task<IActionResult> GetRebalancingSuggestion(int id)
        {
            try
            {
                var portfolio = await _portfolioService.GetPortfolioDetailsAsync(id);
                if (portfolio == null)
                {
                    return NotFound($"Portfolio with ID {id} not found");
                }

                var portfolioDto = MappingService.MapToPortfolioDto(portfolio);
                var rebalancingThreshold = 0.05m; // 5% de tolerância

                var suggestions = portfolioDto.Positions
                    .Where(p => Math.Abs(p.TargetAllocation - p.CurrentAllocation) > rebalancingThreshold)
                    .Select(p =>
                    {
                        var deviation = p.CurrentAllocation - p.TargetAllocation;
                        var targetValue = portfolioDto.CurrentValue * p.TargetAllocation;
                        var adjustmentValue = targetValue - p.CurrentValue;

                        return new
                        {
                            asset = p.AssetSymbol,
                            currentAllocation = p.CurrentAllocation,
                            targetAllocation = p.TargetAllocation,
                            deviation = deviation,
                            action = adjustmentValue > 0 ? "BUY" : "SELL",
                            adjustmentValue = Math.Abs(adjustmentValue),
                            suggestedQuantity = p.CurrentPrice > 0 
                                ? Math.Abs(adjustmentValue) / p.CurrentPrice 
                                : 0
                        };
                    })
                    .ToList();

                var rebalancing = new
                {
                    portfolioId = id,
                    portfolioName = portfolio.Name,
                    currentValue = portfolioDto.CurrentValue,
                    rebalancingNeeded = suggestions.Any(),
                    threshold = rebalancingThreshold,
                    suggestions = suggestions
                };

                return Ok(rebalancing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rebalancing suggestions for portfolio {PortfolioId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
