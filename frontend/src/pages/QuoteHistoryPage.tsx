import { Link } from 'react-router-dom'
import { PageHeader } from '@/shared/components/PageHeader'
import { LoadingState } from '@/shared/components/LoadingState'
import { ErrorState } from '@/shared/components/ErrorState'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Card, CardContent } from '@/components/ui/card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { formatCurrency } from '@/shared/utils/formatCurrency'
import { useQuotes } from '@/features/pricing/hooks/usePricing'
import { Calculator, ExternalLink } from 'lucide-react'

export function QuoteHistoryPage() {
  const { data: quotes, isLoading, error } = useQuotes()

  return (
    <div>
      <PageHeader
        title="Quote History"
        description="All saved pricing quotes."
        action={
          <Button asChild size="sm">
            <Link to="/calculator"><Calculator className="size-4 mr-1.5" />New Quote</Link>
          </Button>
        }
      />

      {isLoading && <LoadingState rows={6} />}
      {error && <ErrorState error={error} />}

      {!isLoading && !error && quotes?.length === 0 && (
        <Card className="rounded-xl border border-dashed shadow-none">
          <CardContent className="flex flex-col items-center justify-center py-16 text-center">
            <Calculator className="size-10 text-stone-300 mb-3" />
            <p className="font-medium text-stone-600">No quotes yet</p>
            <p className="text-sm text-stone-400 mt-1 mb-5">Create your first quote using the calculator.</p>
            <Button asChild size="sm"><Link to="/calculator">Open Calculator</Link></Button>
          </CardContent>
        </Card>
      )}

      {quotes && quotes.length > 0 && (
        <Card className="rounded-xl border border-stone-200/60 shadow-sm overflow-hidden">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Product</TableHead>
                <TableHead>Qty</TableHead>
                <TableHead>Stitches</TableHead>
                <TableHead>Final Price</TableHead>
                <TableHead>Per Item</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Date</TableHead>
                <TableHead />
              </TableRow>
            </TableHeader>
            <TableBody>
              {quotes.map((q) => (
                <TableRow key={q.quoteId} className="hover:bg-stone-50/60">
                  <TableCell className="font-medium text-stone-800">
                    {q.productType} · {q.placementType}
                  </TableCell>
                  <TableCell>{q.quantity}</TableCell>
                  <TableCell>{q.stitchCount.toLocaleString()}</TableCell>
                  <TableCell className="font-semibold">{formatCurrency(q.finalPrice)}</TableCell>
                  <TableCell>{formatCurrency(q.pricePerItem)}</TableCell>
                  <TableCell>
                    <Badge className="bg-emerald-50 text-emerald-600 text-xs font-normal">{q.status}</Badge>
                  </TableCell>
                  <TableCell className="text-stone-400 text-xs">
                    {new Date(q.createdAtUtc).toLocaleDateString()}
                  </TableCell>
                  <TableCell>
                    <Link to={`/quotes/${q.quoteId}`} className="text-indigo-600 hover:text-indigo-800">
                      <ExternalLink className="size-4" />
                    </Link>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </Card>
      )}
    </div>
  )
}
