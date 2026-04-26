namespace StitchPrice.Domain.Entities;

public class PricingSettings
{
    public int Id { get; set; }
    public decimal PricePerThousandStitches { get; set; }
    public decimal SetupFee { get; set; }
    public decimal DigitizingFee { get; set; }
    public decimal UrgencyMultiplier { get; set; }
    public decimal DefaultMarkupPercentage { get; set; }
    public decimal MinimumOrderPrice { get; set; }
    public decimal ColorComplexityFeePerColor { get; set; }
    public bool BulkDiscountEnabled { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public static PricingSettings Default() => new()
    {
        PricePerThousandStitches = 10m,
        SetupFee = 20m,
        DigitizingFee = 30m,
        UrgencyMultiplier = 1.25m,
        DefaultMarkupPercentage = 40m,
        MinimumOrderPrice = 50m,
        ColorComplexityFeePerColor = 5m,
        BulkDiscountEnabled = true,
        CreatedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow
    };
}
