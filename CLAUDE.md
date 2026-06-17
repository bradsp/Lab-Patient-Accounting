# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build entire solution
dotnet build "Lab Billing.sln"

# Release build
dotnet build "Lab Billing.sln" -c Release

# Build specific project (WinForms UI)
dotnet build "Lab PA WinForms UI/Lab PA WinForms UI.csproj" -c Release -p:Platform=x64

# Run tests
dotnet test "Lab Billing.sln"

# Run specific test project
dotnet test "LabBillingCore.UnitTests/LabBillingCore.UnitTests.csproj"

# Run a single test by name (substring match)
dotnet test --filter "FullyQualifiedName~AccountServiceTests.MethodName"
```

**Platform target:** x64 is the default/enforced platform for the solution.

## Run Commands

```bash
# Blazor web UI (new billing UI target)
dotnet run --project "LabOutreachUI/LabOutreachUI.csproj"

# Console batch tool
dotnet run --project "LabBillingConsole/LabBillingConsole.csproj"
```

The WinForms client (`Lab PA WinForms UI`) and Windows service (`LabBillingService`) are launched from Visual Studio or their installed binaries — they are not `dotnet run` targets in normal use.

## Active Migration

Current branch `billing-ui-to-blazor` is an in-progress port of the billing UI from WinForms (`Lab PA WinForms UI`) to Blazor (`LabOutreachUI`). New billing UI work should target Blazor unless explicitly told otherwise. The legacy `MCL` data access layer has been deprecated — do not add to it; use `LabBilling Library` repositories instead. Blazor migration notes live alongside the deprecation commit and in `LabOutreachUI/IMPLEMENTATION_PLAN.md`.

## Architecture Overview

Lab Patient Accounting is a medical billing application for outreach laboratories. It processes HL7 messages, generates X12 837 insurance claims, and handles patient billing.

### Project Structure

| Project | Purpose |
|---------|---------|
| `Lab PA WinForms UI` | Main desktop client (WinForms, .NET 8.0) |
| `LabOutreachUI` | Web client (Blazor Server, .NET 8.0) |
| `LabBilling Library` | Core business logic, services, repositories, models |
| `LabBillingService` | Windows service for background processing (Topshelf) |
| `Lab Patient Accounting Job Scheduler` | Scheduled job runner (Quartz.NET) |
| `LabBillingConsole` | CLI for batch operations |
| `LabBilling Winforms Library` | WinForms-specific components and printing |
| `j4jayant.HL7.Parser` | HL7 message parsing |
| `Utilities` | Common utilities, SSH, JSON processing |

### Layered Architecture

```
Presentation    → Lab PA WinForms UI, LabOutreachUI, LabBillingConsole
Business Logic  → LabBilling Library/Services/ (AccountService, Billing837Service, etc.)
Data Access     → LabBilling Library/Repositories/ (Repository pattern + UnitOfWork)
Database        → SQL Server (PetaPoco ORM)
```

### Key Services (LabBilling Library/Services/)

- **AccountService** - Patient account CRUD, charges, payments, insurance (~2500 lines)
- **Billing837Service** - X12 837i/837p claim file generation
- **HL7ProcessorService** - Processes HL7 ADT (registration), DFT (charges), MFN (providers)
- **ClaimGeneratorService** - Claim processing workflow
- **DictionaryService** - Reference data (CPT codes, diagnosis codes, etc.)
- **RequisitionPrintingService** - Lab requisition form printing (PCL5)
- **RandomDrugScreenService** - Random drug screening module logic

### Data Access Pattern

- **ORM:** PetaPoco (lightweight micro-ORM with attribute-based mapping)
- **Repositories:** `LabBilling Library/Repositories/` - one per entity type
- **UnitOfWork:** two separate units of work bound to different databases — `UnitOfWorkMain` for billing/transactional data (accounts, charges, claims, HL7 inbound), and `UnitOfWorkSystem` for application config and users (system parameters, `Emp` users, auth/authorization). Repositories must be constructed against the correct UoW; mixing them silently writes to the wrong database.
- **Models:** 96+ entity classes in `LabBilling Library/Models/`

### Healthcare Integration

- **HL7:** ADT^A04/A08 (registration), DFT^P03 (charges), MFN^M02 (providers)
- **X12 837:** 5010 format claims via EdiTools library
- **Clearing house:** Electronic claims submission

## Technology Stack

- .NET 8.0 (modern projects) / .NET Framework 4.8 (legacy)
- SQL Server 2014+
- PetaPoco (ORM), Autofac (DI), FluentValidation
- NLog/log4net (logging), Quartz.NET (scheduling), Topshelf (service hosting)
- PDFsharp/MigraDoc (PDF), NPOI/ClosedXML (Excel)
- xUnit (testing)

## Database

- **Schema:** 500+ tables defined in `LabBillingDatabase` and `Lab PA Database` projects
- **Build script:** `LabPatientAccounting DB Build.sql` (4.3MB complete schema)
- **Connection:** Configure server/database in `Properties/Settings.settings` for each project

## Configuration

- **App settings:** `Properties/Settings.settings` in each executable project
- **Logging:** `NLog.config` (file + database logging)
- **Database:** Requires SQL Server with LabBilling database schema

## Blazor Web App (LabOutreachUI)

- ASP.NET Core Blazor Server on .NET 8.0
- Windows Authentication in production, development auth in debug
- Policy-based authorization (DatabaseUser, RandomDrugScreen)
- Configuration in `appsettings.json` and `launchSettings.json`
- **Module host pattern:** `LabOutreachUI` is not a single app but a host for multiple modules, each scoped to its own route namespace:
  - `/rds/*` — Random Drug Screen (active)
  - `/clients/*` — Client Viewer
  - New billing UI work (the `billing-ui-to-blazor` migration) lands here as additional modules
  - See `LabOutreachUI/MODULAR_ARCHITECTURE.md` for the navigation/module pattern before adding a new module.

## Repository-Level Documentation

`README.md` is partially stale (says .NET 6 / C# 7.3 — actual targets are .NET 8.0 / .NET Framework 4.8 as listed above). `REPOSITORY_INVENTORY.md` duplicates the project table in this file; treat this CLAUDE.md as the source of truth for project layout. Per-module/feature docs live under `LabOutreachUI/*.md` and `LabOutreachUI/Docs/`, and admin/user guides under `Docs/`.
