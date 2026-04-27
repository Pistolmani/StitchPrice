import { NavLink, Outlet } from 'react-router-dom'
import { Calculator, ClipboardList, Settings, Package, Scissors } from 'lucide-react'
import { cn } from '@/lib/utils'

const navItems = [
  { section: 'Pricing', items: [
    { to: '/calculator', label: 'Calculator', icon: Calculator },
    { to: '/quotes', label: 'Quote History', icon: ClipboardList },
  ]},
  { section: 'Admin', items: [
    { to: '/admin/settings', label: 'Pricing Settings', icon: Settings },
    { to: '/admin/product-profiles', label: 'Product Profiles', icon: Package },
  ]},
]

export function AppLayout() {
  return (
    <div className="flex min-h-screen bg-stone-50">
      <aside className="w-60 shrink-0 border-r border-stone-200 bg-white px-3 py-5">
        <NavLink to="/" className="flex items-center gap-2 px-3 mb-8">
          <Scissors className="size-5 text-indigo-600" />
          <span className="font-semibold text-stone-900 text-sm">StitchPrice</span>
        </NavLink>

        <nav className="space-y-6">
          {navItems.map(({ section, items }) => (
            <div key={section}>
              <p className="px-3 mb-1.5 text-xs font-medium text-stone-400 uppercase tracking-wider">
                {section}
              </p>
              <ul className="space-y-0.5">
                {items.map(({ to, label, icon: Icon }) => (
                  <li key={to}>
                    <NavLink
                      to={to}
                      className={({ isActive }) =>
                        cn(
                          'flex items-center gap-2.5 rounded-lg px-3 py-2 text-sm transition-colors',
                          isActive
                            ? 'bg-indigo-50 text-indigo-700 font-medium'
                            : 'text-stone-600 hover:bg-stone-100 hover:text-stone-900',
                        )
                      }
                    >
                      <Icon className="size-4 shrink-0" />
                      {label}
                    </NavLink>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </nav>
      </aside>

      <main className="flex-1 min-w-0 p-8">
        <Outlet />
      </main>
    </div>
  )
}
