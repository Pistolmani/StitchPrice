import { z } from 'zod'

export const pricingSchema = z.object({
  productType: z.enum(['Hoodie', 'TShirt', 'Polo', 'Cap', 'Patch', 'Sweater', 'Jacket', 'Custom']),
  placementType: z.enum(['LeftChest', 'CenterChest', 'Back', 'Sleeve', 'CapFront', 'Patch', 'Custom']),
  fabricType: z.enum(['Cotton', 'Polyester', 'CottonPolyBlend', 'Fleece', 'Denim', 'Other']),
  quantity: z.coerce.number().int().min(1, 'Quantity must be at least 1'),
  stitchCount: z.coerce.number().int().min(1, 'Stitch count must be at least 1'),
  colorCount: z.coerce.number().int().min(1, 'Color count must be at least 1').max(15),
  garmentCostPerItem: z.coerce.number().min(0, 'Garment cost cannot be negative'),
  requiresDigitizing: z.boolean(),
  isUrgent: z.boolean(),
  note: z.string().optional(),
})

export type PricingFormValues = z.infer<typeof pricingSchema>
