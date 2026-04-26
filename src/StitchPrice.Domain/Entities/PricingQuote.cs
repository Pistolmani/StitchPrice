using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Entities;

public class PricingQuote
{
    public Guid Id { get; set; }
    public ProductType ProductType { get; set; }
    public PlacementType PlacementType { get; set; }
    public FabricType FabricType { get; set; }
    public int Quantity { get; set; }
    public int StitchCount { get; set; }
    public int ColorCount { get; set; }
    public decimal GarmentCostPerItem { get; set; }
    public bool RequiresDigitizing { get; set; }
    public bool IsUrgent { get; set; }
    public decimal SetupFee { get; set; }
    public decimal DigitizingFee { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal MarkupAmount { get; set; }
    public decimal FinalPrice { get; set; }
    public decimal PricePerItem { get; set; }
    public decimal ProfitMarginPercentage { get; set; }
    public QuoteStatus Status { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<PricingBreakdownItem> BreakdownItems { get; set; } = [];
}
