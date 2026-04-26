using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Entities;

public class ProductPricingProfile
{
    public int Id { get; set; }
    public ProductType ProductType { get; set; }
    public decimal DefaultGarmentCost { get; set; }
    public decimal DefaultMarkupPercentage { get; set; }
    public decimal DifficultyMultiplier { get; set; }
    public bool IsActive { get; set; }
}
