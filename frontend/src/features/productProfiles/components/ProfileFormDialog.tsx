import { useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { profileSchema, type ProfileFormValues } from '../schemas/profileSchema'
import type { ProductProfileDto } from '../types/profileTypes'
import { useCreateProfile, useUpdateProfile } from '../hooks/useProfiles'
import { toast } from 'sonner'

interface Props {
  open: boolean
  onClose: () => void
  editing?: ProductProfileDto
}

export function ProfileFormDialog({ open, onClose, editing }: Props) {
  const isEdit = !!editing
  const { mutate: create, isPending: creating } = useCreateProfile()
  const { mutate: update, isPending: updating } = useUpdateProfile()

  const { register, handleSubmit, setValue, watch, reset, formState: { errors } } = useForm<ProfileFormValues>({
    resolver: zodResolver(profileSchema),
    defaultValues: { isActive: true, difficultyMultiplier: 1, defaultMarkupPercentage: 40 },
  })

  useEffect(() => {
    if (editing) {
      reset(editing)
    } else {
      reset({ isActive: true, difficultyMultiplier: 1, defaultMarkupPercentage: 40, defaultGarmentCost: 0 })
    }
  }, [editing, reset])

  const onSubmit = (data: ProfileFormValues) => {
    if (isEdit && editing) {
      update({ id: editing.id, ...data }, {
        onSuccess: () => { toast.success('Profile updated'); onClose() },
        onError: () => toast.error('Failed to update profile'),
      })
    } else {
      create(data, {
        onSuccess: () => { toast.success('Profile created'); onClose() },
        onError: () => toast.error('Failed to create profile'),
      })
    }
  }

  return (
    <Dialog open={open} onOpenChange={(o) => !o && onClose()}>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>{isEdit ? 'Edit Profile' : 'Add Product Profile'}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 py-2">
          {!isEdit && (
            <div>
              <Label>Product Type</Label>
              <Select onValueChange={(v) => setValue('productType', v as ProfileFormValues['productType'])}>
                <SelectTrigger className="mt-1.5">
                  <SelectValue placeholder="Select…" />
                </SelectTrigger>
                <SelectContent>
                  {(['Hoodie','TShirt','Polo','Cap','Patch','Sweater','Jacket','Custom'] as const).map((v) => (
                    <SelectItem key={v} value={v}>{v === 'TShirt' ? 'T-Shirt' : v}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
              {errors.productType && <p className="mt-1 text-xs text-red-500">{errors.productType.message}</p>}
            </div>
          )}
          <div>
            <Label htmlFor="defaultGarmentCost">Default Garment Cost (GEL)</Label>
            <Input id="defaultGarmentCost" type="number" step="0.01" className="mt-1.5" {...register('defaultGarmentCost')} />
            {errors.defaultGarmentCost && <p className="mt-1 text-xs text-red-500">{errors.defaultGarmentCost.message}</p>}
          </div>
          <div>
            <Label htmlFor="defaultMarkupPercentage">Default Markup %</Label>
            <Input id="defaultMarkupPercentage" type="number" step="0.1" className="mt-1.5" {...register('defaultMarkupPercentage')} />
            {errors.defaultMarkupPercentage && <p className="mt-1 text-xs text-red-500">{errors.defaultMarkupPercentage.message}</p>}
          </div>
          <div>
            <Label htmlFor="difficultyMultiplier">Difficulty Multiplier</Label>
            <Input id="difficultyMultiplier" type="number" step="0.01" className="mt-1.5" {...register('difficultyMultiplier')} />
            {errors.difficultyMultiplier && <p className="mt-1 text-xs text-red-500">{errors.difficultyMultiplier.message}</p>}
          </div>
          <label className="flex items-center gap-2 cursor-pointer select-none">
            <input
              type="checkbox"
              className="size-4 rounded border-stone-300"
              {...register('isActive')}
              checked={watch('isActive')}
              onChange={(e) => setValue('isActive', e.target.checked)}
            />
            <span className="text-sm text-stone-700">Active</span>
          </label>
          <DialogFooter>
            <Button type="button" variant="outline" onClick={onClose}>Cancel</Button>
            <Button type="submit" disabled={creating || updating}>
              {creating || updating ? 'Saving…' : isEdit ? 'Update' : 'Create'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}
