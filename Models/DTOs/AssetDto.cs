using System.ComponentModel.DataAnnotations;

namespace PortifolioFinanceiro.Models.DTOs
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<PriceHistoryDto>? PriceHistory { get; set; }
    }

    public class CreateAssetDto
    {
        [Required(ErrorMessage = "Symbol is required")]
        [StringLength(10, ErrorMessage = "Symbol must be at most 10 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type must be at most 50 characters")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sector is required")]
        [StringLength(50, ErrorMessage = "Sector must be at most 50 characters")]
        public string Sector { get; set; } = string.Empty;

        [Required(ErrorMessage = "Current price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Current price must be greater than zero")]
        public decimal CurrentPrice { get; set; }
    }

    public class UpdatePriceDto
    {
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
    }

    public class PriceHistoryDto
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
