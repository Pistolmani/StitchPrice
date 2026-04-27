import { z } from 'zod'

export const settingsSchema = z.object({
  pricePerThousandStitches: z.coerce.number().gt(0, 'Must be greater than 0'),
  setupFee: z.coerce.number().min(0),
  digitizingFee: z.coerce.number().min(0),
  urgencyMultiplier: z.coerce.number().min(1, 'Must be at least 1'),
  defaultMarkupPercentage: z.coerce.number().min(0).max(300),
  minimumOrderPrice: z.coerce.number().min(0),
  colorComplexityFeePerColor: z.coerce.number().min(0),
  bulkDiscountEnabled: z.boolean(),
})

export type SettingsFormValues = z.infer<typeof settingsSchema>
