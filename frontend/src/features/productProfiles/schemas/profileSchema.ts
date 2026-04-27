import { z } from 'zod'

export const profileSchema = z.object({
  productType: z.enum(['Hoodie', 'TShirt', 'Polo', 'Cap', 'Patch', 'Sweater', 'Jacket', 'Custom']),
  defaultGarmentCost: z.coerce.number().min(0),
  defaultMarkupPercentage: z.coerce.number().min(0).max(300),
  difficultyMultiplier: z.coerce.number().gt(0),
  isActive: z.boolean(),
})

export type ProfileFormValues = z.infer<typeof profileSchema>
