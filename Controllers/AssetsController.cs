using Microsoft.AspNetCore.Mvc;
using PortifolioFinanceiro.Models.DTOs;
using PortifolioFinanceiro.Services;
using PortifolioFinanceiro.Extensions;

namespace PortifolioFinanceiro.Controllers
{
    [ApiController]
    [Route("api/assets")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetService assetService, ILogger<AssetsController> logger)
        {
            _assetService = assetService;
            _logger = logger;
        }

        /// <summary>
        /// Retorna todos os ativos disponíveis na plataforma
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            try
            {
                var assets = await _assetService.GetAllAssetsAsync();
                var assetsDto = assets.Select(MappingService.MapToAssetDto).ToList();
                
                _logger.LogInformation("Retrieved {Count} assets", assetsDto.Count);
                return this.ApiOk(assetsDto, $"Successfully retrieved {assetsDto.Count} assets");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all assets");
                return this.ApiInternalServerError("Failed to retrieve assets");
            }
        }

        /// <summary>
        /// Retorna um ativo específico pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(int id)
        {
            try
            {
                var asset = await _assetService.GetAssetByIdAsync(id);
                if (asset == null)
                {
                    return this.ApiNotFound($"Asset with ID {id} not found");
                }

                var assetDto = MappingService.MapToAssetDto(asset);
                return this.ApiOk(assetDto, "Asset retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving asset with ID {AssetId}", id);
                return this.ApiInternalServerError("Failed to retrieve asset");
            }
        }

        /// <summary>
        /// Busca um ativo pelo símbolo (ex: PETR4)
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchBySymbol([FromQuery] string symbol)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(symbol))
                {
                    return this.ApiBadRequest("Symbol parameter is required");
                }

                var asset = await _assetService.GetAssetBySymbolAsync(symbol);
                if (asset == null)
                {
                    return this.ApiNotFound($"Asset with symbol {symbol} not found");
                }

                var assetDto = MappingService.MapToAssetDto(asset);
                return this.ApiOk(assetDto, $"Asset {symbol} found successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching asset with symbol {Symbol}", symbol);
                return this.ApiInternalServerError("Failed to search asset");
            }
        }

        /// <summary>
        /// Cria um novo ativo
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetDto createAssetDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.ApiBadRequest("Invalid asset data provided");
                }

                var asset = MappingService.MapToAsset(createAssetDto);
                var createdAsset = await _assetService.CreateAssetAsync(asset);
                var assetDto = MappingService.MapToAssetDto(createdAsset);

                return this.ApiCreated(nameof(GetAssetById), new { id = createdAsset.Id }, assetDto, $"Asset {createdAsset.Symbol} created successfully");
            }
            catch (ArgumentException ex)
            {
                return this.ApiBadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating asset");
                return this.ApiInternalServerError("Failed to create asset");
            }
        }

        /// <summary>
        /// Atualiza o preço de um ativo
        /// </summary>
        [HttpPut("{id}/price")]
        public async Task<IActionResult> UpdatePrice(int id, [FromBody] UpdatePriceDto updatePriceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.ApiBadRequest("Invalid price data provided");
                }

                var updatedAsset = await _assetService.UpdateAssetPriceAsync(id, updatePriceDto.Price);
                var assetDto = MappingService.MapToAssetDto(updatedAsset);

                return this.ApiOk(assetDto, $"Price updated successfully for {updatedAsset.Symbol}");
            }
            catch (KeyNotFoundException ex)
            {
                return this.ApiNotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return this.ApiBadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating price for asset ID {AssetId}", id);
                return this.ApiInternalServerError("Failed to update asset price");
            }
        }
    }
}
