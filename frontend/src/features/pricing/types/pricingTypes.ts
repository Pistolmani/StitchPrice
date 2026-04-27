export type ProductType = 'Hoodie' | 'TShirt' | 'Polo' | 'Cap' | 'Patch' | 'Sweater' | 'Jacket' | 'Custom'
export type PlacementType = 'LeftChest' | 'CenterChest' | 'Back' | 'Sleeve' | 'CapFront' | 'Patch' | 'Custom'
export type FabricType = 'Cotton' | 'Polyester' | 'CottonPolyBlend' | 'Fleece' | 'Denim' | 'Other'
export type QuoteStatus = 'Draft' | 'Calculated' | 'Saved' | 'Expired'
export type PricingAdjustmentType = 'Cost' | 'Fee' | 'Discount' | 'Markup'

export interface PricingBreakdownItemDto {
  name: string
  description: string
  amount: number
  type: PricingAdjustmentType
  sortOrder: number
}

export interface PricingQuoteDto {
  quoteId: string
  productType: ProductType
  placementType: PlacementType
  fabricType: FabricType
  quantity: number
  stitchCount: number
  colorCount: number
  garmentCostPerItem: number
  requiresDigitizing: boolean
  isUrgent: boolean
  note: string | null
  subtotal: number
  discountAmount: number
  markupAmount: number
  finalPrice: number
  pricePerItem: number
  profitMarginPercentage: number
  status: QuoteStatus
  createdAtUtc: string
  breakdown: PricingBreakdownItemDto[]
}

export interface CalculatePricingQuoteRequest {
  productType: ProductType
  placementType: PlacementType
  fabricType: FabricType
  quantity: number
  stitchCount: number
  colorCount: number
  garmentCostPerItem: number
  requiresDigitizing: boolean
  isUrgent: boolean
  note?: string
}
