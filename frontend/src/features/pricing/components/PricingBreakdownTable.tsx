import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Badge } from '@/components/ui/badge'
import { formatCurrency } from '@/shared/utils/formatCurrency'
import type { PricingBreakdownItemDto } from '../types/pricingTypes'
import { cn } from '@/lib/utils'

interface Props {
  items: PricingBreakdownItemDto[]
}

const typeColor: Record<string, string> = {
  Cost: 'bg-stone-100 text-stone-600',
  Fee: 'bg-blue-50 text-blue-600',
  Discount: 'bg-emerald-50 text-emerald-600',
  Markup: 'bg-indigo-50 text-indigo-600',
}

export function PricingBreakdownTable({ items }: Props) {
  const sorted = [...items].sort((a, b) => a.sortOrder - b.sortOrder)

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Item</TableHead>
          <TableHead>Type</TableHead>
          <TableHead className="text-right">Amount</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {sorted.map((item) => (
          <TableRow key={item.sortOrder}>
            <TableCell>
              <p className="text-sm font-medium text-stone-800">{item.name}</p>
              <p className="text-xs text-stone-400">{item.description}</p>
            </TableCell>
            <TableCell>
              <Badge className={cn('text-xs font-normal', typeColor[item.type] ?? '')}>
                {item.type}
              </Badge>
            </TableCell>
            <TableCell className="text-right font-medium text-stone-800">
              {formatCurrency(item.amount)}
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  )
}
