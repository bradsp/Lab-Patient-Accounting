# Lab Patient Accounting Repository Inventory

## Repository Overview
This repository contains a comprehensive Laboratory Patient Accounting system built with .NET technologies and SQL Server. The system manages patient billing, insurance processing, laboratory charge management, and reporting for laboratory services.

## Component Inventory

| Path | Type | Primary Purpose | Key Dependencies | Notes |
|------|------|-----------------|------------------|-------|
| **Applications & Executables** |
| `Lab PA WinForms UI/` | WinExe | Main Windows Forms user interface for lab billing management | LabBilling Core, WinFormsLibrary, Microsoft.Data.SqlClient, NLog | Primary desktop application with comprehensive billing functionality |
| `LabBillingConsole/` | Console Exe | Command-line interface for batch operations and administrative tasks | LabBilling Core, Utilities, Spectre.Console, NLog | Used for automated processing and system maintenance |
| `LabBillingService/` | Windows Service | Background service for automated billing processes | LabBilling Core, Topshelf, log4net, NLog | Runs scheduled billing operations and data processing |
| `Lab Patient Accounting Job Scheduler/` | Console Exe | Job scheduling system for automated tasks | LabBilling Core, Quartz.NET, Autofac, log4net | Manages scheduled jobs for billing processes |
| **Core Libraries** |
| `LabBilling Library/` | Class Library | Core business logic and data access layer | PetaPoco, Microsoft.Data.SqlClient, FluentValidation, NPOI, NLog | Central business logic with ORM, validation, and Excel processing |
| `WinFormsLibrary/` | Class Library | Shared Windows Forms components and utilities | LabBilling Core, ClosedXML, DocumentFormat.OpenXml | Reusable UI components and Excel functionality |
| `LabBilling Winforms Library/` | Class Library | Specialized WinForms components for lab billing | LabBilling Core, PDFsharp, MigraDoc | PDF generation and specialized lab billing UI components |
| `j4jayant.HL7.Parser/` | Class Library | HL7 message parsing and processing | Microsoft.CSharp, System.Data.DataSetExtensions | Healthcare data interchange format handling |
| `Utilities/` | Class Library | Common utility functions and helpers | Microsoft.Data.SqlClient, SSH.NET, Newtonsoft.Json | Database utilities, SSH operations, JSON processing |
| `MCL/` | Class Library | Model classes and data access layer | Utilities | Data models and repository pattern implementation |
| **Database & Infrastructure** |
| `LabBillingDatabase/` | SQL Server Database Project | Complete database schema and stored procedures | SQL Server | Comprehensive database with 500+ tables, views, functions, and procedures |
| `Lab PA Database/` | SQL Server Database Project | Additional database components | SQL Server | Secondary database project with additional schemas |
| **Test Assemblies** |
| `LabBillingCore.UnitTests/` | Test Library | Unit tests for core library functionality | xunit, LabBilling Core | Modern xUnit test framework for .NET 8 |
| `LabBillingUnitTesting/` | Test Library | Legacy unit tests | xunit, Moq, Castle.Core | Legacy .NET Framework test suite with mocking |
| **Setup & Deployment** |
| `LabBillingServiceSetup/` | MSI Setup Project | Windows Installer for billing service | Visual Studio Installer | Deployment package for Windows Service |
| `LabBillingJobsSetup/` | MSI Setup Project | Windows Installer for job scheduler | Visual Studio Installer | Deployment package for Job Scheduler |
| **Configuration & Scripts** |
| `Lab Patient Accounting SQL Agent Jobs.sql` | SQL Script | SQL Server Agent job definitions | SQL Server Agent | Automated database maintenance and processing jobs |
| `LabPatientAccounting DB Build.sql` | SQL Script | Database build and deployment script | SQL Server | Complete database schema deployment |
| `SqlSchemaCompare1 07-13-2023.scmp` | Schema Compare | Database schema comparison file | SQL Server Data Tools | Database version control and change management |
| **Documentation & Configuration** |
| `.gitignore` | Git Configuration | Source control ignore patterns | Git | Standard Visual Studio gitignore with 340+ entries |
| `README.md` | Documentation | Project documentation and setup instructions | N/A | Basic project information and getting started guide |
| `Lab Billing.sln` | Solution File | Visual Studio solution definition | Visual Studio | Master solution file containing all projects |

## Technology Stack Summary

### Primary Technologies
- **.NET 8.0** - Modern .NET framework for most projects
- **.NET Framework 4.8** - Legacy framework for some components  
- **SQL Server** - Primary database engine
- **Windows Forms** - Desktop UI framework
- **C#** - Primary programming language

### Key Dependencies & Libraries
- **PetaPoco** - Lightweight ORM for data access
- **NLog/log4net** - Logging frameworks
- **FluentValidation** - Input validation
- **Quartz.NET** - Job scheduling
- **Topshelf** - Windows service hosting
- **PDFsharp/MigraDoc** - PDF generation
- **NPOI** - Excel file processing
- **xUnit** - Unit testing framework
- **SSH.NET** - Secure file transfer

### Development Tools
- **Visual Studio 2022** - Primary IDE
- **SQL Server Data Tools** - Database development
- **Git** - Source control
- **MSI Installers** - Deployment packaging

## Architecture Notes

1. **Multi-tiered Architecture**: Clear separation of UI, business logic, data access, and database layers
2. **Service-Oriented**: Background services handle automated processing
3. **Database-Centric**: Extensive use of stored procedures and database functions
4. **Windows-Focused**: Primarily designed for Windows environments
5. **Healthcare Integration**: HL7 message processing for healthcare data interchange
6. **Billing Focused**: Specialized for laboratory billing and insurance processing

## Project Dependencies Visualization

```
Lab PA WinForms UI
├── LabBilling Core
├── WinFormsLibrary
├── LabBilling Winforms Library
└── Utilities

LabBillingService
├── LabBilling Core
└── Utilities

LabBillingConsole
├── LabBilling Core
└── Utilities

LabBilling Core (Central Hub)
├── j4jayant.HL7.Parser
└── Utilities

Job Scheduler
├── LabBilling Core
└── Utilities
```

## Infrastructure Summary

- **15 Projects** total in solution
- **3 Executable applications** (Desktop, Console, Service)
- **6 Library projects** with specialized functionality
- **2 Database projects** with comprehensive schemas
- **2 Test assemblies** for quality assurance
- **2 Setup projects** for deployment
- **Multiple configuration files** for logging and application settings