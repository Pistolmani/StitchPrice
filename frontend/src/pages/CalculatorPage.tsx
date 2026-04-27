import { useState } from 'react'
import { PageHeader } from '@/shared/components/PageHeader'
import { PricingForm } from '@/features/pricing/components/PricingForm'
import { PricingResultCard } from '@/features/pricing/components/PricingResultCard'
import type { PricingQuoteDto } from '@/features/pricing/types/pricingTypes'

export function CalculatorPage() {
  const [result, setResult] = useState<PricingQuoteDto | null>(null)

  return (
    <div>
      <PageHeader title="Price Calculator" description="Get an instant quote for your embroidery order." />
      <div className="grid grid-cols-[3fr_2fr] gap-6 items-start">
        <PricingForm onResult={setResult} />
        {result ? (
          <div className="sticky top-8">
            <PricingResultCard quote={result} />
          </div>
        ) : (
          <div className="sticky top-8 rounded-xl border border-dashed border-stone-300 bg-stone-50/50 p-10 text-center text-stone-400 text-sm">
            Fill in the order details and click<br /><strong>Calculate Price</strong> to see your quote.
          </div>
        )}
      </div>
    </div>
  )
}
