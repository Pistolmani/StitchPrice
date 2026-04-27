import { PageHeader } from '@/shared/components/PageHeader'
import { LoadingState } from '@/shared/components/LoadingState'
import { ErrorState } from '@/shared/components/ErrorState'
import { Card, CardContent } from '@/components/ui/card'
import { SettingsForm } from '@/features/settings/components/SettingsForm'
import { useSettings } from '@/features/settings/hooks/useSettings'

export function PricingSettingsPage() {
  const { data: settings, isLoading, error } = useSettings()

  return (
    <div>
      <PageHeader title="Pricing Settings" description="Configure the rules that drive your pricing engine." />

      {isLoading && <LoadingState rows={8} />}
      {error && <ErrorState error={error} />}
      {settings && (
        <Card className="rounded-xl border border-stone-200/60 shadow-sm">
          <CardContent className="pt-6">
            <SettingsForm defaultValues={settings} />
          </CardContent>
        </Card>
      )}
    </div>
  )
}
