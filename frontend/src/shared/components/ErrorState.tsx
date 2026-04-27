import { AlertCircle } from 'lucide-react'
import { isApiError } from '@/shared/api/httpClient'

interface ErrorStateProps {
  error: unknown
}

export function ErrorState({ error }: ErrorStateProps) {
  const message = isApiError(error)
    ? error.detail ?? error.title
    : 'Something went wrong. Please try again.'

  return (
    <div className="flex items-center gap-3 rounded-xl border border-red-200 bg-red-50 p-4 text-red-700">
      <AlertCircle className="size-5 shrink-0" />
      <p className="text-sm">{message}</p>
    </div>
  )
}
