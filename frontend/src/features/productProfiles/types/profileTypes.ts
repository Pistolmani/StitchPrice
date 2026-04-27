import type { ProductType } from '@/features/pricing/types/pricingTypes'

export interface ProductProfileDto {
  id: number
  productType: ProductType
  defaultGarmentCost: number
  defaultMarkupPercentage: number
  difficultyMultiplier: number
  isActive: boolean
}

export interface CreateProductProfileRequest {
  productType: ProductType
  defaultGarmentCost: number
  defaultMarkupPercentage: number
  difficultyMultiplier: number
  isActive: boolean
}

export interface UpdateProductProfileRequest {
  id: number
  defaultGarmentCost: number
  defaultMarkupPercentage: number
  difficultyMultiplier: number
  isActive: boolean
}
