import { useParams, Link } from 'react-router-dom'
import { PageHeader } from '@/shared/components/PageHeader'
import { LoadingState } from '@/shared/components/LoadingState'
import { ErrorState } from '@/shared/components/ErrorState'
import { Button } from '@/components/ui/button'
import { PricingResultCard } from '@/features/pricing/components/PricingResultCard'
import { useQuote } from '@/features/pricing/hooks/usePricing'
import { ArrowLeft } from 'lucide-react'

export function QuoteDetailsPage() {
  const { id } = useParams<{ id: string }>()
  const { data: quote, isLoading, error } = useQuote(id ?? '')

  return (
    <div>
      <div className="mb-4">
        <Button asChild variant="ghost" size="sm" className="text-stone-500 -ml-2">
          <Link to="/quotes"><ArrowLeft className="size-4 mr-1" />Back to Quotes</Link>
        </Button>
      </div>
      <PageHeader
        title="Quote Details"
        description={quote ? `${quote.productType} · ${quote.placementType} · ${new Date(quote.createdAtUtc).toLocaleDateString()}` : ''}
      />

      {isLoading && <LoadingState rows={8} />}
      {error && <ErrorState error={error} />}
      {quote && (
        <div className="max-w-2xl">
          <PricingResultCard quote={quote} />
          {quote.note && (
            <div className="mt-4 rounded-lg border border-stone-200 bg-stone-50 px-4 py-3 text-sm text-stone-600">
              <span className="font-medium text-stone-700">Note: </span>{quote.note}
            </div>
          )}
        </div>
      )}
    </div>
  )
}
