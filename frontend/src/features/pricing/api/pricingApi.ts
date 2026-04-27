import { httpClient } from '@/shared/api/httpClient'
import type { CalculatePricingQuoteRequest, PricingQuoteDto } from '../types/pricingTypes'

export const pricingApi = {
  calculate: (payload: CalculatePricingQuoteRequest) =>
    httpClient.post<PricingQuoteDto>('/api/pricing/calculate', payload).then((r) => r.data),

  getQuotes: (page = 1, pageSize = 20) =>
    httpClient
      .get<PricingQuoteDto[]>('/api/pricing/quotes', { params: { page, pageSize } })
      .then((r) => r.data),

  getQuoteById: (id: string) =>
    httpClient.get<PricingQuoteDto>(`/api/pricing/quotes/${id}`).then((r) => r.data),
}
