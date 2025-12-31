# Lab Patient Accounting - Blazor Migration Brief

## Vision
Replace the existing Windows Forms (WinForms) desktop application with a modern Blazor Server web application, enabling browser-based access while maintaining all current functionality and leveraging existing business logic from `LabBilling.Core`.

## Current State

### Existing WinForms Application
- **Main Entry:** `Lab PA WinForms UI` project (.NET 8.0, x64)
- **Architecture:** MDI (Multiple Document Interface) with tabbed windows
- **Forms Count:** ~35+ forms covering billing, dictionaries, reports, and administration
- **Custom Controls:** CurrencyTextBox, DateTextBox, InsuranceLookup, ProviderLookup, LookupBox
- **Authentication:** SQL-based login via `LoginForm`
- **Data Binding:** Direct DataGridView binding to models/services

### Existing Blazor Application (LabOutreachUI)
- **Framework:** Blazor Server (.NET 8.0)
- **Purpose:** Limited client outreach features (Random Drug Screen, Client Viewer)
- **Authentication:** Windows Auth (production) / Development auth (debug)
- **Patterns:** Component-based, Bootstrap 5, scoped services via DI
- **Services:** Reuses `LabBilling.Core` services (DictionaryService, RandomDrugScreenService)

### Shared Business Logic (LabBilling.Core)
- 43+ services (AccountService, Billing837Service, ClaimGeneratorService, etc.)
- 96+ domain models
- Repository pattern with UnitOfWork
- PetaPoco ORM for SQL Server

## Goals

1. **Full Feature Parity** - All WinForms functionality accessible via Blazor
2. **Leverage Existing Patterns** - Extend LabOutreachUI's architecture and auth
3. **Responsive Design** - Bootstrap 5 responsive layouts for various screen sizes
4. **Modern UX** - Replace MDI tabs with SPA navigation, modern form controls
5. **Maintainability** - Component-based architecture, reusable UI components

## Scope

### In Scope - WinForms Features to Migrate

**Billing Module (9 forms)**
- AccountForm - Patient account management with 10 tabs (Summary, Demographics, Diagnosis, Insurance x3, Charges, Payments, Notes, Billing Activity)
- WorkListForm - Account worklist with filtering and tree-based queue selection
- ChargeEntryForm / BatchChargeEntryForm - Single and batch charge entry
- PaymentAdjustmentEntryForm - Payment/adjustment posting
- BatchRemittance / PostRemittanceForm / ProcessRemittanceForm - Remittance processing
- ClaimsManagementForm - Claim batch management and generation
- ClientInvoiceForm - Client invoice generation
- PatientCollectionsForm - Bad debt/collections management

**Dictionary Maintenance (8 forms)**
- ChargeMasterMaintenance / ChargeMasterEditForm - CDM management
- ClientMaintenanceForm / ClientMaintenanceEditForm - Client management
- HealthPlanMaintenanceForm / HealtPlanMaintenanceEditForm - Insurance plans
- PhysicianMaintenanceForm / PhysicianMaintenanceEditForm - Physicians/Pathologists
- AuditReportMaintenanceForm - Audit reports configuration

**System Administration (6 forms)**
- UserSecurity - User management and permissions
- SystemParametersForm - Application parameters
- InterfaceMapping - HL7 interface mapping
- InterfaceMonitor - HL7 interface monitoring
- LogViewerForm - System log viewer
- AccountLocksForm - View/manage account locks

**Supporting Forms**
- LoginForm - Authentication (will use existing LabOutreachUI auth)
- DashboardForm - Home dashboard
- NewAccountForm - New account creation
- PersonSearchForm - Patient search
- Lookup forms (CDM, Client, Insurance, Provider)

### Out of Scope (Current Migration)
- Reports (SQL Reporting Services integration - phase 2)
- LabBillingConsole functionality
- Windows Service (LabBillingService)
- Job Scheduler
- Mobile-specific optimizations

## Technical Decisions

### Architecture
- Extend existing `LabOutreachUI` project (not create new project)
- Maintain Blazor Server model (not WebAssembly)
- Use existing authentication/authorization infrastructure
- Create modular page structure with components

### UI Framework
- Bootstrap 5 (already in LabOutreachUI)
- Open Iconic icons (already in LabOutreachUI)
- Custom CSS for specific needs

### Component Strategy
- Create reusable form input components (CurrencyInput, DateInput, etc.)
- Build lookup/autocomplete components based on existing AutocompleteInput
- Create data grid component for consistent table display
- Build modal dialogs for edit forms

### Navigation
- Replace MDI tabs with SPA routing
- Sidebar navigation with module sections
- Breadcrumb navigation for context
- Recent accounts quick access

### State Management
- Scoped services for user context
- Component parameters for data passing
- Cascading values where appropriate
- No external state library (YAGNI)

## Success Criteria

1. All WinForms billing operations achievable in Blazor
2. All dictionary maintenance operations achievable
3. User authentication and authorization working
4. Data integrity maintained (same business logic)
5. Responsive UI functional on 1080p+ screens
6. Performance acceptable for typical workflows

## Constraints

- Must maintain SQL Server compatibility
- Must work with existing LabBilling.Core without changes
- Must support Windows Authentication in production
- Must run under IIS in production environment
