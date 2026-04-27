import axios, { AxiosError } from 'axios'

export interface ApiError {
  status: number
  title: string
  detail?: string
  errors?: Record<string, string[]>
}

export function isApiError(err: unknown): err is ApiError {
  return typeof err === 'object' && err !== null && 'status' in err && 'title' in err
}

export const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
})

httpClient.interceptors.response.use(
  (res) => res,
  (err: AxiosError) => {
    const data = err.response?.data as Record<string, unknown> | undefined
    const apiError: ApiError = {
      status: err.response?.status ?? 0,
      title: (data?.title as string) ?? 'Request failed',
      detail: data?.detail as string | undefined,
      errors: data?.errors as Record<string, string[]> | undefined,
    }
    return Promise.reject(apiError)
  },
)
