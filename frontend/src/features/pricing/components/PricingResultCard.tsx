import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { formatCurrency } from '@/shared/utils/formatCurrency'
import type { PricingQuoteDto } from '../types/pricingTypes'
import { PricingBreakdownTable } from './PricingBreakdownTable'
import { TrendingUp, Tag, Layers } from 'lucide-react'

interface Props {
  quote: PricingQuoteDto
}

function StatRow({ label, value }: { label: string; value: string }) {
  return (
    <div className="flex items-center justify-between py-1.5 text-sm">
      <span className="text-stone-500">{label}</span>
      <span className="font-medium text-stone-800">{value}</span>
    </div>
  )
}

export function PricingResultCard({ quote }: Props) {
  return (
    <div className="space-y-4">
      <Card className="rounded-xl border border-stone-200/60 shadow-sm">
        <CardHeader className="pb-2">
          <CardTitle className="text-base font-semibold">Quote Summary</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="mb-5 text-center">
            <p className="text-4xl font-bold text-stone-900">{formatCurrency(quote.finalPrice)}</p>
            <p className="mt-1 text-sm text-stone-400">Total for {quote.quantity} item(s)</p>
          </div>

          <div className="grid grid-cols-3 gap-3 mb-5">
            <div className="flex flex-col items-center rounded-lg bg-stone-50 p-3">
              <Tag className="size-4 text-indigo-500 mb-1" />
              <p className="text-xs text-stone-400">Per Item</p>
              <p className="text-sm font-semibold text-stone-800">{formatCurrency(quote.pricePerItem)}</p>
            </div>
            <div className="flex flex-col items-center rounded-lg bg-stone-50 p-3">
              <TrendingUp className="size-4 text-emerald-500 mb-1" />
              <p className="text-xs text-stone-400">Margin</p>
              <p className="text-sm font-semibold text-stone-800">{quote.profitMarginPercentage.toFixed(1)}%</p>
            </div>
            <div className="flex flex-col items-center rounded-lg bg-stone-50 p-3">
              <Layers className="size-4 text-blue-500 mb-1" />
              <p className="text-xs text-stone-400">Subtotal</p>
              <p className="text-sm font-semibold text-stone-800">{formatCurrency(quote.subtotal)}</p>
            </div>
          </div>

          <div className="border-t border-stone-100 pt-3 divide-y divide-stone-100">
            <StatRow label="Discount" value={`-${formatCurrency(quote.discountAmount)}`} />
            <StatRow label="Markup" value={`+${formatCurrency(quote.markupAmount)}`} />
          </div>
        </CardContent>
      </Card>

      <Card className="rounded-xl border border-stone-200/60 shadow-sm">
        <CardHeader className="pb-2">
          <CardTitle className="text-base font-semibold">Breakdown</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <PricingBreakdownTable items={quote.breakdown} />
        </CardContent>
      </Card>
    </div>
  )
}
