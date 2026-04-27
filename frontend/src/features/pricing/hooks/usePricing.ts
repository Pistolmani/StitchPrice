import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { pricingApi } from '../api/pricingApi'
import type { CalculatePricingQuoteRequest } from '../types/pricingTypes'

export const quotesKeys = {
  all: ['quotes'] as const,
  list: (page: number, pageSize: number) => ['quotes', 'list', page, pageSize] as const,
  detail: (id: string) => ['quotes', id] as const,
}

export function useCalculateQuote() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (payload: CalculatePricingQuoteRequest) => pricingApi.calculate(payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: quotesKeys.all }),
  })
}

export function useQuotes(page = 1, pageSize = 20) {
  return useQuery({
    queryKey: quotesKeys.list(page, pageSize),
    queryFn: () => pricingApi.getQuotes(page, pageSize),
  })
}

export function useQuote(id: string) {
  return useQuery({
    queryKey: quotesKeys.detail(id),
    queryFn: () => pricingApi.getQuoteById(id),
    enabled: !!id,
  })
}
