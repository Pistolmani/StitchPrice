import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { profilesApi } from '../api/profilesApi'
import type { CreateProductProfileRequest, UpdateProductProfileRequest } from '../types/profileTypes'

export const profileKeys = {
  all: ['profiles'] as const,
}

export function useProfiles() {
  return useQuery({
    queryKey: profileKeys.all,
    queryFn: profilesApi.getProfiles,
  })
}

export function useCreateProfile() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (payload: CreateProductProfileRequest) => profilesApi.createProfile(payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: profileKeys.all }),
  })
}

export function useUpdateProfile() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, ...rest }: UpdateProductProfileRequest) => profilesApi.updateProfile(id, rest),
    onSuccess: () => qc.invalidateQueries({ queryKey: profileKeys.all }),
  })
}
