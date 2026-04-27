export interface PricingSettingsDto {
  pricePerThousandStitches: number
  setupFee: number
  digitizingFee: number
  urgencyMultiplier: number
  defaultMarkupPercentage: number
  minimumOrderPrice: number
  colorComplexityFeePerColor: number
  bulkDiscountEnabled: boolean
  updatedAtUtc: string
}

export type UpdatePricingSettingsRequest = Omit<PricingSettingsDto, 'updatedAtUtc'>
