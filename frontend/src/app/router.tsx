import { createBrowserRouter, Navigate } from 'react-router-dom'
import { AppLayout } from '@/shared/components/AppLayout'
import { LandingPage } from '@/pages/LandingPage'
import { CalculatorPage } from '@/pages/CalculatorPage'
import { QuoteHistoryPage } from '@/pages/QuoteHistoryPage'
import { QuoteDetailsPage } from '@/pages/QuoteDetailsPage'
import { PricingSettingsPage } from '@/pages/PricingSettingsPage'
import { ProductProfilesPage } from '@/pages/ProductProfilesPage'

export const router = createBrowserRouter([
  { path: '/', element: <LandingPage /> },
  {
    element: <AppLayout />,
    children: [
      { path: '/calculator', element: <CalculatorPage /> },
      { path: '/quotes', element: <QuoteHistoryPage /> },
      { path: '/quotes/:id', element: <QuoteDetailsPage /> },
      { path: '/admin/settings', element: <PricingSettingsPage /> },
      { path: '/admin/product-profiles', element: <ProductProfilesPage /> },
      { path: '*', element: <Navigate to="/" replace /> },
    ],
  },
])
