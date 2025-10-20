using Microsoft.AspNetCore.Mvc;
using PortifolioFinanceiro.Models.DTOs;
using PortifolioFinanceiro.Services;

namespace PortifolioFinanceiro.Controllers
{
    [ApiController]
    [Route("api/portfolios")]
    public class PortfoliosController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<PortfoliosController> _logger;

        public PortfoliosController(IPortfolioService portfolioService, ILogger<PortfoliosController> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        /// <summary>
        /// Retorna os portfólios de um usuário específico
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUserPortfolios([FromQuery] string? userId = null)
        {
            try
            {
                IEnumerable<PortifolioFinanceiro.Models.Portfolio> portfolios;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    // Se não especificar userId, retorna todos os portfólios
                    portfolios = await _portfolioService.GetAllPortfoliosAsync();
                }
                else
                {
                    portfolios = await _portfolioService.GetUserPortfoliosAsync(userId);
                }

                var portfoliosDto = portfolios.Select(MappingService.MapToPortfolioSummaryDto).ToList();
                
                _logger.LogInformation("Retrieved {Count} portfolios for user {UserId}", portfoliosDto.Count, userId ?? "all");
                return Ok(portfoliosDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving portfolios for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Cria um novo portfólio
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePortfolio([FromBody] CreatePortfolioDto createPortfolioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var portfolio = MappingService.MapToPortfolio(createPortfolioDto);
                var createdPortfolio = await _portfolioService.CreatePortfolioAsync(portfolio);
                var portfolioDto = MappingService.MapToPortfolioSummaryDto(createdPortfolio);

                return CreatedAtAction(nameof(GetPortfolioDetails), new { id = createdPortfolio.Id }, portfolioDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating portfolio");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retorna os detalhes completos de um portfólio, incluindo todas as posições
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPortfolioDetails(int id)
        {
            try
            {
                var portfolio = await _portfolioService.GetPortfolioDetailsAsync(id);
                if (portfolio == null)
                {
                    return NotFound($"Portfolio with ID {id} not found");
                }

                var portfolioDto = MappingService.MapToPortfolioDto(portfolio);
                return Ok(portfolioDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving portfolio details for ID {PortfolioId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adiciona uma nova posição ao portfólio
        /// </summary>
        [HttpPost("{id}/positions")]
        public async Task<IActionResult> AddPosition(int id, [FromBody] CreatePositionDto createPositionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var position = MappingService.MapToPosition(createPositionDto);
                var createdPosition = await _portfolioService.AddPositionAsync(id, position);
                var positionDto = MappingService.MapToPositionDto(createdPosition);

                return CreatedAtAction(nameof(GetPortfolioDetails), new { id }, positionDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding position to portfolio {PortfolioId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Atualiza uma posição existente do portfólio
        /// </summary>
        [HttpPut("{id}/positions/{positionId}")]
        public async Task<IActionResult> UpdatePosition(int id, int positionId, [FromBody] UpdatePositionDto updatePositionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var position = new PortifolioFinanceiro.Models.Position
                {
                    Quantity = updatePositionDto.Quantity,
                    AveragePrice = updatePositionDto.AveragePrice,
                    TargetAllocation = updatePositionDto.TargetAllocation
                };

                var updatedPosition = await _portfolioService.UpdatePositionAsync(id, positionId, position);
                var positionDto = MappingService.MapToPositionDto(updatedPosition);

                return Ok(positionDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating position {PositionId} in portfolio {PortfolioId}", positionId, id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Remove uma posição do portfólio
        /// </summary>
        [HttpDelete("{id}/positions/{positionId}")]
        public async Task<IActionResult> RemovePosition(int id, int positionId)
        {
            try
            {
                await _portfolioService.RemovePositionAsync(id, positionId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing position {PositionId} from portfolio {PortfolioId}", positionId, id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adiciona múltiplas posições ao portfólio
        /// </summary>
        [HttpPost("{id}/positions/batch")]
        public async Task<IActionResult> AddMultiplePositions(int id, [FromBody] CreateMultiplePositionsDto createMultiplePositionsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var positions = createMultiplePositionsDto.Positions.Select(MappingService.MapToPosition);
                var createdPositions = await _portfolioService.AddMultiplePositionsAsync(id, positions);
                
                // Obter o portfolio atualizado para calcular os DTOs corretamente
                var updatedPortfolio = await _portfolioService.GetPortfolioDetailsAsync(id);
                var portfolioDto = MappingService.MapToPortfolioDto(updatedPortfolio!);
                var positionsDto = portfolioDto.Positions;

                return CreatedAtAction(nameof(GetPortfolioDetails), new { id }, positionsDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding multiple positions to portfolio {PortfolioId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
