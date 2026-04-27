import { httpClient } from '@/shared/api/httpClient'
import type { CreateProductProfileRequest, ProductProfileDto, UpdateProductProfileRequest } from '../types/profileTypes'

export const profilesApi = {
  getProfiles: () =>
    httpClient.get<ProductProfileDto[]>('/api/admin/product-profiles').then((r) => r.data),

  createProfile: (payload: CreateProductProfileRequest) =>
    httpClient.post<ProductProfileDto>('/api/admin/product-profiles', payload).then((r) => r.data),

  updateProfile: (id: number, payload: Omit<UpdateProductProfileRequest, 'id'>) =>
    httpClient.put<ProductProfileDto>(`/api/admin/product-profiles/${id}`, { id, ...payload }).then((r) => r.data),
}
