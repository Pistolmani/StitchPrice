import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { pricingSchema, type PricingFormValues } from '../schemas/pricingSchema'
import type { PricingQuoteDto } from '../types/pricingTypes'
import { useCalculateQuote } from '../hooks/usePricing'
import { isApiError } from '@/shared/api/httpClient'
import { toast } from 'sonner'
import { useNavigate } from 'react-router-dom'

interface PricingFormProps {
  onResult: (quote: PricingQuoteDto) => void
}

function FieldError({ message }: { message?: string }) {
  if (!message) return null
  return <p className="mt-1 text-xs text-red-500">{message}</p>
}

export function PricingForm({ onResult }: PricingFormProps) {
  const navigate = useNavigate()
  const { mutate, isPending } = useCalculateQuote()

  const {
    register,
    handleSubmit,
    setValue,
    watch,
    setError,
    formState: { errors },
  } = useForm<PricingFormValues>({
    resolver: zodResolver(pricingSchema),
    defaultValues: {
      quantity: 1,
      stitchCount: 5000,
      colorCount: 3,
      garmentCostPerItem: 20,
      requiresDigitizing: false,
      isUrgent: false,
    },
  })

  const onSubmit = (data: PricingFormValues) => {
    mutate(data, {
      onSuccess: (quote) => {
        onResult(quote)
        toast.success('Quote saved', {
          action: {
            label: 'View',
            onClick: () => navigate(`/quotes/${quote.quoteId}`),
          },
        })
      },
      onError: (err) => {
        if (isApiError(err) && err.errors) {
          Object.entries(err.errors).forEach(([field, messages]) => {
            const key = field.charAt(0).toLowerCase() + field.slice(1)
            setError(key as keyof PricingFormValues, { message: messages[0] })
          })
        } else {
          toast.error('Failed to calculate quote')
        }
      },
    })
  }

  return (
    <Card className="rounded-xl border border-stone-200/60 shadow-sm">
      <CardHeader>
        <CardTitle className="text-base font-semibold">Order Details</CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
          <div className="grid grid-cols-3 gap-4">
            <div>
              <Label>Product Type</Label>
              <Select onValueChange={(v) => setValue('productType', v as PricingFormValues['productType'])}>
                <SelectTrigger className="mt-1.5">
                  <SelectValue placeholder="Select…" />
                </SelectTrigger>
                <SelectContent>
                  {(['Hoodie','TShirt','Polo','Cap','Patch','Sweater','Jacket','Custom'] as const).map((v) => (
                    <SelectItem key={v} value={v}>{v === 'TShirt' ? 'T-Shirt' : v}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
              <FieldError message={errors.productType?.message} />
            </div>

            <div>
              <Label>Placement</Label>
              <Select onValueChange={(v) => setValue('placementType', v as PricingFormValues['placementType'])}>
                <SelectTrigger className="mt-1.5">
                  <SelectValue placeholder="Select…" />
                </SelectTrigger>
                <SelectContent>
                  {(['LeftChest','CenterChest','Back','Sleeve','CapFront','Patch','Custom'] as const).map((v) => (
                    <SelectItem key={v} value={v}>{v.replace(/([A-Z])/g, ' $1').trim()}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
              <FieldError message={errors.placementType?.message} />
            </div>

            <div>
              <Label>Fabric Type</Label>
              <Select onValueChange={(v) => setValue('fabricType', v as PricingFormValues['fabricType'])}>
                <SelectTrigger className="mt-1.5">
                  <SelectValue placeholder="Select…" />
                </SelectTrigger>
                <SelectContent>
                  {(['Cotton','Polyester','CottonPolyBlend','Fleece','Denim','Other'] as const).map((v) => (
                    <SelectItem key={v} value={v}>{v.replace(/([A-Z])/g, ' $1').trim()}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
              <FieldError message={errors.fabricType?.message} />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <Label htmlFor="quantity">Quantity</Label>
              <Input id="quantity" type="number" min={1} className="mt-1.5" {...register('quantity')} />
              <FieldError message={errors.quantity?.message} />
            </div>
            <div>
              <Label htmlFor="stitchCount">Stitch Count</Label>
              <Input id="stitchCount" type="number" min={1} className="mt-1.5" {...register('stitchCount')} />
              <FieldError message={errors.stitchCount?.message} />
            </div>
            <div>
              <Label htmlFor="colorCount">Color Count</Label>
              <Input id="colorCount" type="number" min={1} max={15} className="mt-1.5" {...register('colorCount')} />
              <FieldError message={errors.colorCount?.message} />
            </div>
            <div>
              <Label htmlFor="garmentCostPerItem">Garment Cost / Item (GEL)</Label>
              <Input id="garmentCostPerItem" type="number" step="0.01" min={0} className="mt-1.5" {...register('garmentCostPerItem')} />
              <FieldError message={errors.garmentCostPerItem?.message} />
            </div>
          </div>

          <div className="flex gap-6">
            <label className="flex items-center gap-2 cursor-pointer select-none">
              <input
                type="checkbox"
                className="size-4 rounded border-stone-300"
                {...register('requiresDigitizing')}
                checked={watch('requiresDigitizing')}
              />
              <span className="text-sm text-stone-700">Requires Digitizing</span>
            </label>
            <label className="flex items-center gap-2 cursor-pointer select-none">
              <input
                type="checkbox"
                className="size-4 rounded border-stone-300"
                {...register('isUrgent')}
                checked={watch('isUrgent')}
              />
              <span className="text-sm text-stone-700">Urgent Order</span>
            </label>
          </div>

          <div>
            <Label htmlFor="note">Note (optional)</Label>
            <textarea
              id="note"
              rows={2}
              placeholder="Any special instructions…"
              className="mt-1.5 w-full rounded-md border border-stone-200 bg-white px-3 py-2 text-sm placeholder:text-stone-400 focus:outline-none focus:ring-2 focus:ring-indigo-500/30 resize-none"
              {...register('note')}
            />
          </div>

          <Button type="submit" className="w-full" disabled={isPending}>
            {isPending ? 'Calculating…' : 'Calculate Price'}
          </Button>
        </form>
      </CardContent>
    </Card>
  )
}
