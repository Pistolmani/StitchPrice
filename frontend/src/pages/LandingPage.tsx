import { Link } from 'react-router-dom'
import { Button } from '@/components/ui/button'
import { Scissors, Calculator, BarChart3, Settings } from 'lucide-react'

export function LandingPage() {
  return (
    <div className="min-h-screen bg-stone-50 flex flex-col">
      <header className="border-b border-stone-200 bg-white px-8 py-4 flex items-center gap-2">
        <Scissors className="size-5 text-indigo-600" />
        <span className="font-semibold text-stone-900">StitchPrice</span>
      </header>

      <main className="flex-1 flex flex-col items-center justify-center px-8 text-center">
        <div className="max-w-2xl">
          <div className="inline-flex items-center gap-1.5 rounded-full bg-indigo-50 px-3 py-1 text-xs font-medium text-indigo-600 mb-6">
            <Scissors className="size-3" />
            Embroidery Pricing Engine
          </div>
          <h1 className="text-5xl font-bold text-stone-900 leading-tight mb-4">
            Transparent quotes,<br />profitable pricing
          </h1>
          <p className="text-lg text-stone-500 mb-8 leading-relaxed">
            Calculate accurate embroidery quotes in seconds. Factor in stitch count, garment cost,
            digitizing, bulk discounts, and urgency — all in one place.
          </p>
          <div className="flex items-center justify-center gap-3">
            <Button asChild size="lg" className="px-8">
              <Link to="/calculator">Open Calculator</Link>
            </Button>
            <Button asChild size="lg" variant="outline">
              <Link to="/quotes">View Quotes</Link>
            </Button>
          </div>
        </div>

        <div className="grid grid-cols-3 gap-6 mt-20 max-w-3xl w-full">
          {[
            { icon: Calculator, title: 'Smart Calculator', desc: 'Multi-rule pricing engine with stitch count, fabric, color complexity and more.' },
            { icon: BarChart3, title: 'Quote History', desc: 'Every quote is saved and searchable. Track trends and revisit past orders.' },
            { icon: Settings, title: 'Configurable Rules', desc: 'Adjust fees, markup, bulk discounts, and urgency multipliers to match your business.' },
          ].map(({ icon: Icon, title, desc }) => (
            <div key={title} className="rounded-xl border border-stone-200 bg-white p-5 text-left shadow-sm">
              <div className="mb-3 inline-flex rounded-lg bg-indigo-50 p-2">
                <Icon className="size-5 text-indigo-600" />
              </div>
              <h3 className="font-semibold text-stone-900 mb-1">{title}</h3>
              <p className="text-sm text-stone-500">{desc}</p>
            </div>
          ))}
        </div>
      </main>
    </div>
  )
}
