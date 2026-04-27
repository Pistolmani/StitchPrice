import { useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { settingsSchema, type SettingsFormValues } from '../schemas/settingsSchema'
import type { PricingSettingsDto } from '../types/settingsTypes'
import { useUpdateSettings } from '../hooks/useSettings'
import { toast } from 'sonner'

interface Props {
  defaultValues: PricingSettingsDto
}

function Field({ label, id, error, children }: { label: string; id: string; error?: string; children: React.ReactNode }) {
  return (
    <div>
      <Label htmlFor={id}>{label}</Label>
      <div className="mt-1.5">{children}</div>
      {error && <p className="mt-1 text-xs text-red-500">{error}</p>}
    </div>
  )
}

export function SettingsForm({ defaultValues }: Props) {
  const { mutate, isPending } = useUpdateSettings()

  const { register, handleSubmit, reset, watch, setValue, formState: { errors } } = useForm<SettingsFormValues>({
    resolver: zodResolver(settingsSchema),
    defaultValues,
  })

  useEffect(() => { reset(defaultValues) }, [defaultValues, reset])

  const onSubmit = (data: SettingsFormValues) => {
    mutate(data, {
      onSuccess: () => toast.success('Settings saved'),
      onError: () => toast.error('Failed to save settings'),
    })
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-5 max-w-lg">
      <Field label="Price per 1,000 Stitches (GEL)" id="pricePerThousandStitches" error={errors.pricePerThousandStitches?.message}>
        <Input id="pricePerThousandStitches" type="number" step="0.01" {...register('pricePerThousandStitches')} />
      </Field>
      <Field label="Setup Fee (GEL)" id="setupFee" error={errors.setupFee?.message}>
        <Input id="setupFee" type="number" step="0.01" {...register('setupFee')} />
      </Field>
      <Field label="Digitizing Fee (GEL)" id="digitizingFee" error={errors.digitizingFee?.message}>
        <Input id="digitizingFee" type="number" step="0.01" {...register('digitizingFee')} />
      </Field>
      <Field label="Urgency Multiplier" id="urgencyMultiplier" error={errors.urgencyMultiplier?.message}>
        <Input id="urgencyMultiplier" type="number" step="0.01" {...register('urgencyMultiplier')} />
      </Field>
      <Field label="Default Markup %" id="defaultMarkupPercentage" error={errors.defaultMarkupPercentage?.message}>
        <Input id="defaultMarkupPercentage" type="number" step="0.1" {...register('defaultMarkupPercentage')} />
      </Field>
      <Field label="Minimum Order Price (GEL)" id="minimumOrderPrice" error={errors.minimumOrderPrice?.message}>
        <Input id="minimumOrderPrice" type="number" step="0.01" {...register('minimumOrderPrice')} />
      </Field>
      <Field label="Color Complexity Fee / Color (GEL)" id="colorComplexityFeePerColor" error={errors.colorComplexityFeePerColor?.message}>
        <Input id="colorComplexityFeePerColor" type="number" step="0.01" {...register('colorComplexityFeePerColor')} />
      </Field>

      <label className="flex items-center gap-2 cursor-pointer select-none">
        <input
          type="checkbox"
          className="size-4 rounded border-stone-300"
          {...register('bulkDiscountEnabled')}
          checked={watch('bulkDiscountEnabled')}
          onChange={(e) => setValue('bulkDiscountEnabled', e.target.checked)}
        />
        <span className="text-sm text-stone-700">Enable Bulk Discount</span>
      </label>

      <Button type="submit" disabled={isPending}>
        {isPending ? 'Saving…' : 'Save Settings'}
      </Button>
    </form>
  )
}
