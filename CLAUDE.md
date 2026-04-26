# StitchPrice — Claude Instructions

## Project Overview

StitchPrice is a full-stack pricing engine for custom embroidery businesses. It calculates
transparent, profitable quotes based on stitch count, garment cost, design complexity,
color count, quantity discounts, urgency fees, digitizing fees, and configurable business rules.

**Portfolio project.** Code must be clean, professional, maintainable, and interview-ready.
No AI features. No payments. No multi-tenant SaaS. Focus: a strong, finished pricing engine.

**Tech stack:**
- Backend: .NET 9 Clean Architecture Web API (PostgreSQL + EF Core + MediatR + FluentValidation)
- Frontend: React + TypeScript + Vite + TanStack Query + Zod + Tailwind CSS
- Tests: xUnit + FluentAssertions
- Local infra: Docker Compose

---

## Repository Layout

```
StitchPrice/
├── src/
│   ├── StitchPrice.Domain         # Entities, enums, pricing engine — zero external deps
│   ├── StitchPrice.Application    # CQRS commands/queries, DTOs, validators, interfaces
│   ├── StitchPrice.Infrastructure # EF Core DbContext, repositories, migrations
│   └── StitchPrice.Api            # Controllers, DI wiring, middleware, OpenAPI (Scalar)
├── tests/
│   ├── StitchPrice.UnitTests      # Domain + Application unit tests
│   └── StitchPrice.IntegrationTests # API integration tests (WebApplicationFactory)
└── frontend/                      # React + Vite app (not yet scaffolded)
```

Dependency direction: **Domain ← Application ← Infrastructure / Api**. Domain has no external
package references. Application references MediatR and FluentValidation only.

---

## Architecture Decisions

### Pricing Engine (Domain layer)

The engine uses two interfaces that must stay distinct — do not merge them:

**`IPricingRule`** — stateless transformation: given inputs + running subtotal → one adjustment.
- `Apply(PricingContext context, decimal runningSubtotal)` — `runningSubtotal` is the sum of
  all adjustments applied by previous rules. Rules that depend on accumulated state (discount,
  urgency, markup) use this parameter. Rules that don't (garment, stitches, fees) ignore it.

**`IPricingFinalizer`** — post-calculation constraint: given the final accumulated total → optional
  top-up adjustment. Only `MinimumOrderFinalizer` uses this today.

**Why the separation matters:** Minimum order is not a pricing rule — it's a constraint on the
output. Forcing it into `IPricingRule` caused a Liskov violation (fake IsMatch + throwing Apply).
The two interfaces reflect two genuinely different questions: "what is your contribution?" vs
"is the result acceptable?"

### Rule Execution Order

```
GarmentCost(1) → StitchCount(2) → ColorComplexity(3) → Digitizing(4) → Setup(5)
  → BulkDiscount(6) → UrgencyFee(7) → Markup(8)
  → [MinimumOrderFinalizer(9)]
```

**BulkDiscount runs before UrgencyFee.** This is a deliberate business decision: the bulk
discount applies to the base production cost; the rush surcharge applies to the discounted
amount. Changing this order changes customer-facing pricing — update tests if you change it.

### PricingEngine composition

`PricingEngine` accepts rules and finalizers via constructor injection (DIP). Use
`PricingEngine.CreateDefault()` when DI is not available (unit tests, seed scripts).
In the API, register rules as `IEnumerable<IPricingRule>` in DI and inject the engine.

### Money

Use `decimal` everywhere for monetary values. Never `double` or `float`. All rounding uses
`decimal.Round(value, 2)`.

### Dates

All timestamps are stored and returned as UTC (`DateTime.UtcNow`, column type `timestamptz`).

---

## What Is Built

### ✅ Phase 1 — Solution scaffold
- `StitchPrice.sln` with 6 projects, project references wired
- `global.json` (.NET 9.0.313), `Directory.Build.props` (nullable + TreatWarningsAsErrors)
- `.gitignore`

### ✅ Phase 2 — Domain layer
- **Enums:** `ProductType`, `PlacementType`, `FabricType`, `QuoteStatus`, `PricingAdjustmentType`
- **Entities:** `PricingQuote`, `PricingBreakdownItem`, `PricingSettings` (with `Default()`),
  `ProductPricingProfile`
- **Pricing contracts:** `IPricingRule`, `IPricingFinalizer`, `PricingContext`, `PricingResult`,
  `PricingAdjustment`
- **Rules:** `GarmentCostRule`, `StitchCountRule`, `ColorComplexityRule`, `DigitizingFeeRule`,
  `SetupFeeRule`, `BulkDiscountRule`, `UrgencyFeeRule`, `MarkupRule`, `MinimumOrderFinalizer`
- **`PricingEngine`** — orchestrates rules sequentially, passes running total to each rule

### ✅ Phase 3 — Unit tests (44/44 passing, 0 warnings)
- Rule tests for all 9 rules + finalizer
- End-to-end `PricingEngineTests` covering: worked example, single item, urgent order,
  urgent+bulk interaction, minimum enforcement, sort order, DIP (custom rule injection)

---

## What Needs To Be Built

### ✅ Phase 4 — Application layer
Files to create under `src/StitchPrice.Application/`:

```
Features/
  Pricing/
    Commands/
      CalculatePricingQuoteCommand.cs     # record + IRequest<PricingQuoteDto>
      CalculatePricingQuoteHandler.cs     # calls PricingEngine, persists via IQuoteRepository
    Queries/
      GetPricingQuoteByIdQuery.cs
      GetPricingQuoteByIdHandler.cs
      GetPricingQuotesQuery.cs
      GetPricingQuotesHandler.cs
    DTOs/
      PricingQuoteDto.cs
      PricingBreakdownItemDto.cs
      CalculatePricingQuoteRequest.cs
    Validators/
      CalculatePricingQuoteValidator.cs   # FluentValidation rules from spec
  Settings/
    Commands/
      UpdatePricingSettingsCommand.cs
      UpdatePricingSettingsHandler.cs
    Queries/
      GetPricingSettingsQuery.cs
      GetPricingSettingsHandler.cs
    DTOs/
      PricingSettingsDto.cs
  ProductProfiles/
    Commands/
      CreateProductProfileCommand.cs
      UpdateProductProfileCommand.cs
    Queries/
      GetProductProfilesQuery.cs
    DTOs/
      ProductProfileDto.cs
Interfaces/
  IQuoteRepository.cs
  IPricingSettingsRepository.cs
  IProductProfileRepository.cs
DependencyInjection.cs                   # AddApplication(services) extension
```

Built:
- Repository interfaces: `IQuoteRepository`, `IPricingSettingsRepository`, `IProductProfileRepository`
- DTOs: `PricingQuoteDto`, `PricingBreakdownItemDto`, `PricingSettingsDto`, `ProductProfileDto`
- Commands + Handlers: `CalculatePricingQuoteCommand`, `UpdatePricingSettingsCommand`,
  `CreateProductProfileCommand`, `UpdateProductProfileCommand`
- Queries + Handlers: `GetPricingQuoteByIdQuery`, `GetPricingQuotesQuery`,
  `GetPricingSettingsQuery`, `GetProductProfilesQuery`
- Validators: `CalculatePricingQuoteValidator`, `UpdatePricingSettingsValidator`,
  `CreateProductProfileValidator`, `UpdateProductProfileValidator`
- `ValidationBehavior<TRequest, TResponse>` MediatR pipeline behavior
- `NotFoundException` for 404 cases
- `DependencyInjection.AddApplication()` registers MediatR + validators + pipeline + PricingEngine

Note on MediatR 12: `RequestHandlerDelegate<T>` takes no arguments — call `await next()` not `await next(ct)`.

### Phase 5 — Infrastructure layer
Files to create under `src/StitchPrice.Infrastructure/`:

```
Persistence/
  StitchPriceDbContext.cs           # PostgreSQL via Npgsql
  Configurations/
    PricingQuoteConfiguration.cs    # EF fluent config
    PricingBreakdownItemConfiguration.cs
    PricingSettingsConfiguration.cs
    ProductPricingProfileConfiguration.cs
  Repositories/
    QuoteRepository.cs
    PricingSettingsRepository.cs
    ProductProfileRepository.cs
  Migrations/                       # generated via dotnet ef migrations add
Seed/
  PricingSettingsSeed.cs            # insert default PricingSettings row on startup
DependencyInjection.cs              # AddInfrastructure(services, connectionString) extension
```

Notes:
- PricingSettings table should have exactly one row (singleton settings). Use `Id = 1` convention.
- PricingQuote.Id is `Guid` (mapped to PostgreSQL `uuid`).
- Run `dotnet ef migrations add InitialCreate -p src/StitchPrice.Infrastructure -s src/StitchPrice.Api`.

### Phase 6 — API layer
Files under `src/StitchPrice.Api/`:

```
Controllers/
  PricingController.cs      # POST /api/pricing/calculate, GET /api/pricing/quotes, GET /api/pricing/quotes/{id}
  SettingsController.cs     # GET/PUT /api/admin/pricing-settings
  ProductProfilesController.cs  # GET/POST/PUT /api/admin/product-profiles
Middleware/
  ExceptionHandlingMiddleware.cs   # catches ValidationException → 400, NotFoundException → 404
Program.cs                 # DI wiring, OpenAPI (Scalar), CORS for React dev server
```

Scalar UI endpoint: `/scalar/v1`. Do NOT use Swashbuckle — this project uses
`Microsoft.AspNetCore.OpenApi` + `Scalar.AspNetCore`.

CORS: allow `http://localhost:5173` (Vite default) in Development.

### Phase 7 — Frontend (React + Vite)
Scaffold under `frontend/`:

```
src/
  app/
    App.tsx
    router.tsx                        # React Router v6
  pages/
    LandingPage.tsx
    CalculatorPage.tsx
    QuoteHistoryPage.tsx
    QuoteDetailsPage.tsx
    PricingSettingsPage.tsx
    ProductProfilesPage.tsx
  features/
    pricing/
      components/
        PricingForm.tsx               # React Hook Form + Zod
        PricingResultCard.tsx
        PricingBreakdownTable.tsx
      api/pricingApi.ts               # TanStack Query mutations
      schemas/pricingSchema.ts        # Zod schema mirroring CalculatePricingQuoteRequest
      types/pricingTypes.ts
    settings/
      (same structure)
  shared/
    components/
      Button.tsx, Input.tsx, Select.tsx, Card.tsx, PageHeader.tsx
      LoadingState.tsx, ErrorState.tsx
    api/httpClient.ts                 # axios or fetch wrapper with base URL from env
    utils/formatCurrency.ts           # formats as "745.00 GEL"
```

UI style: modern SaaS dashboard, white/warm light background, clean typography, rounded cards.
NOT a toy calculator — it must look like a real business tool.

Currency: format as GEL (e.g. `123.50 GEL`). Do not use `₾` symbol — it has inconsistent
font support.

### Phase 8 — Docker + README + Polish
- `docker-compose.yml` — PostgreSQL 16 + API service
- `docker-compose.override.yml` — dev overrides (volume mounts, env vars)
- `README.md` with: overview, features, architecture diagram, pricing engine explanation,
  example calculation, screenshots section, API endpoints, how to run, testing, future work
- Seed data: insert one `PricingSettings` row and default `ProductPricingProfile` rows on
  first run (use `HasData` in EF configuration or a hosted service)

---

## Integration Tests (Phase 5+)

Integration tests live in `tests/StitchPrice.IntegrationTests/` and use `WebApplicationFactory`.
They require a real PostgreSQL instance. Use `appsettings.Test.json` with a test DB connection
string, and run `EnsureCreated()` in the test fixture setup.

Target scenarios:
- `POST /api/pricing/calculate` with valid payload returns 200 with correct breakdown
- `POST /api/pricing/calculate` with invalid payload returns 400 with validation errors
- `GET /api/pricing/quotes` returns list of saved quotes
- `PUT /api/admin/pricing-settings` persists updated settings

---

## Commands Reference

```bash
# Build everything
dotnet build

# Run unit tests
dotnet test tests/StitchPrice.UnitTests

# Run integration tests (requires PostgreSQL)
dotnet test tests/StitchPrice.IntegrationTests

# Run API locally
dotnet run --project src/StitchPrice.Api

# Add EF migration
dotnet ef migrations add <Name> -p src/StitchPrice.Infrastructure -s src/StitchPrice.Api

# Apply migrations
dotnet ef database update -p src/StitchPrice.Infrastructure -s src/StitchPrice.Api

# Run with Docker
docker-compose up
```

---

## Constraints

- All monetary values: `decimal`, never `double`
- All datetimes: UTC
- No logic in controllers — controllers dispatch to MediatR only
- No EF Core references in Domain or Application layers
- `PricingEngine` must remain testable without a database — it takes `PricingContext`
  (pure inputs), not repository calls
- `TreatWarningsAsErrors = true` — zero warnings policy
