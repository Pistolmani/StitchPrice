import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { settingsApi } from '../api/settingsApi'
import type { UpdatePricingSettingsRequest } from '../types/settingsTypes'

export const settingsKeys = {
  all: ['settings'] as const,
}

export function useSettings() {
  return useQuery({
    queryKey: settingsKeys.all,
    queryFn: settingsApi.getSettings,
  })
}

export function useUpdateSettings() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (payload: UpdatePricingSettingsRequest) => settingsApi.updateSettings(payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: settingsKeys.all }),
  })
}
