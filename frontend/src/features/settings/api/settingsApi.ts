import { httpClient } from '@/shared/api/httpClient'
import type { PricingSettingsDto, UpdatePricingSettingsRequest } from '../types/settingsTypes'

export const settingsApi = {
  getSettings: () =>
    httpClient.get<PricingSettingsDto>('/api/admin/pricing-settings').then((r) => r.data),

  updateSettings: (payload: UpdatePricingSettingsRequest) =>
    httpClient.put<PricingSettingsDto>('/api/admin/pricing-settings', payload).then((r) => r.data),
}
