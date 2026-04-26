using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Entities;

public class PricingBreakdownItem
{
    public int Id { get; set; }
    public Guid PricingQuoteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PricingAdjustmentType Type { get; set; }
    public int SortOrder { get; set; }

    public PricingQuote? Quote { get; set; }
}
