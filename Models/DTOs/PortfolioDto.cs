using System.ComponentModel.DataAnnotations;

namespace PortifolioFinanceiro.Models.DTOs
{
    public class PortfolioDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public decimal TotalInvestment { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal TotalReturn { get; set; }
        public decimal ReturnPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PositionDto> Positions { get; set; } = new();
    }

    public class PortfolioSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal TotalInvestment { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ReturnPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PositionsCount { get; set; }
    }

    public class CreatePortfolioDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID is required")]
        [StringLength(50, ErrorMessage = "User ID must be at most 50 characters")]
        public string UserId { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Total investment cannot be negative")]
        public decimal TotalInvestment { get; set; }
    }

    public class PositionDto
    {
        public int Id { get; set; }
        public string AssetSymbol { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public string AssetSector { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal InvestedValue { get; set; }
        public decimal Return { get; set; }
        public decimal ReturnPercentage { get; set; }
        public decimal TargetAllocation { get; set; }
        public decimal CurrentAllocation { get; set; }
        public DateTime? LastTransaction { get; set; }
    }

    public class CreatePositionDto
    {
        [Required(ErrorMessage = "AssetSymbol is required")]
        public string AssetSymbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Average price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Average price must be greater than zero")]
        public decimal AveragePrice { get; set; }

        [Required(ErrorMessage = "Target allocation is required")]
        [Range(0, 1, ErrorMessage = "Target allocation must be between 0 and 1")]
        public decimal TargetAllocation { get; set; }
    }

    public class UpdatePositionDto
    {
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Average price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Average price must be greater than zero")]
        public decimal AveragePrice { get; set; }

        [Required(ErrorMessage = "Target allocation is required")]
        [Range(0, 1, ErrorMessage = "Target allocation must be between 0 and 1")]
        public decimal TargetAllocation { get; set; }
    }

    public class CreateMultiplePositionsDto
    {
        [Required(ErrorMessage = "Positions list is required")]
        [MinLength(1, ErrorMessage = "At least one position is required")]
        public List<CreatePositionDto> Positions { get; set; } = new();
    }
}
