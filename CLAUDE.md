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
```

**Platform target:** x64 is the default/enforced platform for the solution.

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
- **UnitOfWork:** `UnitOfWorkMain` (main DB), `UnitOfWorkSystem` (system DB)
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
- Modules: Random Drug Screen, Client Viewer
- Configuration in `appsettings.json` and `launchSettings.json`
