import { useState } from 'react'
import { PageHeader } from '@/shared/components/PageHeader'
import { LoadingState } from '@/shared/components/LoadingState'
import { ErrorState } from '@/shared/components/ErrorState'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Card } from '@/components/ui/card'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { ProfileFormDialog } from '@/features/productProfiles/components/ProfileFormDialog'
import { useProfiles } from '@/features/productProfiles/hooks/useProfiles'
import type { ProductProfileDto } from '@/features/productProfiles/types/profileTypes'
import { Pencil, Plus } from 'lucide-react'
import { formatCurrency } from '@/shared/utils/formatCurrency'

export function ProductProfilesPage() {
  const { data: profiles, isLoading, error } = useProfiles()
  const [dialogOpen, setDialogOpen] = useState(false)
  const [editing, setEditing] = useState<ProductProfileDto | undefined>()

  const openCreate = () => { setEditing(undefined); setDialogOpen(true) }
  const openEdit = (p: ProductProfileDto) => { setEditing(p); setDialogOpen(true) }

  return (
    <div>
      <PageHeader
        title="Product Profiles"
        description="Default pricing settings per product type."
        action={
          <Button size="sm" onClick={openCreate}>
            <Plus className="size-4 mr-1.5" />Add Profile
          </Button>
        }
      />

      {isLoading && <LoadingState rows={5} />}
      {error && <ErrorState error={error} />}

      {profiles && (
        <Card className="rounded-xl border border-stone-200/60 shadow-sm overflow-hidden">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Product</TableHead>
                <TableHead>Default Cost</TableHead>
                <TableHead>Markup %</TableHead>
                <TableHead>Difficulty</TableHead>
                <TableHead>Status</TableHead>
                <TableHead />
              </TableRow>
            </TableHeader>
            <TableBody>
              {profiles.map((p) => (
                <TableRow key={p.id} className="hover:bg-stone-50/60">
                  <TableCell className="font-medium text-stone-800">
                    {p.productType === 'TShirt' ? 'T-Shirt' : p.productType}
                  </TableCell>
                  <TableCell>{formatCurrency(p.defaultGarmentCost)}</TableCell>
                  <TableCell>{p.defaultMarkupPercentage}%</TableCell>
                  <TableCell>{p.difficultyMultiplier}×</TableCell>
                  <TableCell>
                    <Badge className={p.isActive ? 'bg-emerald-50 text-emerald-600' : 'bg-stone-100 text-stone-400'}>
                      {p.isActive ? 'Active' : 'Inactive'}
                    </Badge>
                  </TableCell>
                  <TableCell>
                    <Button variant="ghost" size="sm" onClick={() => openEdit(p)}>
                      <Pencil className="size-3.5" />
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </Card>
      )}

      <ProfileFormDialog open={dialogOpen} onClose={() => setDialogOpen(false)} editing={editing} />
    </div>
  )
}
