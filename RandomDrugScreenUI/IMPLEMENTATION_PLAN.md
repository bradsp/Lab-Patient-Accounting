# Random Drug Screen (RDS) Implementation Plan

## Overview
This document outlines the plan for implementing the Random Drug Screen functionality as a Blazor Server application within the existing Lab Patient Accounting solution. The implementation will follow the existing architecture patterns found in the LabBilling Core project.

## Existing Architecture Analysis

### Current Infrastructure
1. **LabBilling Core Project** - Contains:
   - Models (data entities with PetaPoco ORM)
   - Repositories (data access layer)
   - Services (business logic)
   - Unit of Work pattern for transaction management
   - IAppEnvironment for application configuration

2. **Database Model Already Exists**:
   - `RandomDrugScreenPerson` model defined in `LabBilling Library\Models\RandomDrugScreenPerson.cs`
   - `RandomDrugScreenPersonRepository` defined in `LabBilling Library\Repositories\RandomDrugScreenPersonRepository.cs`
   - Maps to `dbo.rds` table with fields: uri, deleted, name, cli_mnem, shift, test_date

3. **Service Pattern**: 
   - Services accept `IAppEnvironment` in constructor
   - Services use `IUnitOfWork` (defaulting to new instance if not provided)
   - Business logic in services, repositories handle only data access
   - Async methods provided alongside synchronous versions

4. **Blazor Server Project Structure**:
   - Already set up in `RandomDrugScreenUI` project
   - Basic scaffolding with Program.cs, App.razor, _Imports.razor

## Implementation Components

### 1. Service Layer (LabBilling Library)

#### 1.1 RandomDrugScreenService
**Location**: `LabBilling Library\Services\RandomDrugScreenService.cs`

**Purpose**: Encapsulate all business logic for Random Drug Screen operations

**Key Methods**:
```csharp
public interface IRandomDrugScreenService
{
    // Candidate Management
    Task<RandomDrugScreenPerson> AddCandidateAsync(RandomDrugScreenPerson person, IUnitOfWork uow = null);
    Task<RandomDrugScreenPerson> UpdateCandidateAsync(RandomDrugScreenPerson person, IUnitOfWork uow = null);
    Task<bool> DeleteCandidateAsync(int id, IUnitOfWork uow = null); // Soft delete
    Task<List<RandomDrugScreenPerson>> GetCandidatesByClientAsync(string clientMnem, bool includeDeleted = false, IUnitOfWork uow = null);
    Task<List<RandomDrugScreenPerson>> GetAllCandidatesAsync(bool includeDeleted = false, IUnitOfWork uow = null);
    Task<RandomDrugScreenPerson> GetCandidateByIdAsync(int id, IUnitOfWork uow = null);
    
    // Random Selection
    Task<List<RandomDrugScreenPerson>> SelectRandomCandidatesAsync(string clientMnem, int count, string shift = null, IUnitOfWork uow = null);
    
    // Import Operations
    Task<ImportResult> ImportCandidatesAsync(List<RandomDrugScreenPerson> candidates, string clientMnem, bool replaceAll = false, IUnitOfWork uow = null);
    
    // Client Management
    Task<List<string>> GetDistinctClientsAsync(IUnitOfWork uow = null);
    Task<List<string>> GetDistinctShiftsAsync(string clientMnem = null, IUnitOfWork uow = null);
    
    // Reporting
    Task<List<RandomDrugScreenPerson>> GetNonSelectedCandidatesAsync(string clientMnem, DateTime? fromDate = null, IUnitOfWork uow = null);
}
```

**Random Selection Algorithm**:
- Use `System.Security.Cryptography.RandomNumberGenerator` for cryptographically secure random selection (vs. legacy `srand()`/`rand()`)
- Filter candidates by client and optional shift
- Exclude soft-deleted records
- Validate count against available pool
- Update test_date on selected candidates
- Return selected list

**Import Logic**:
- RTS Mode: Delete all existing records for client, then bulk insert
- BTM/Merge Mode: Mark missing candidates as deleted, add new ones
- Transaction support for rollback on errors

#### 1.2 Repository Enhancements
**Location**: `LabBilling Library\Repositories\RandomDrugScreenPersonRepository.cs`

**Additional Methods Needed**:
```csharp
public class RandomDrugScreenPersonRepository : RepositoryBase<RandomDrugScreenPerson>
{
    // Query methods
    Task<List<RandomDrugScreenPerson>> GetByClientAsync(string clientMnem, bool includeDeleted = false);
    Task<List<RandomDrugScreenPerson>> GetByClientAndShiftAsync(string clientMnem, string shift, bool includeDeleted = false);
    Task<List<string>> GetDistinctClientsAsync();
    Task<List<string>> GetDistinctShiftsAsync(string clientMnem = null);
    Task<int> GetCandidateCountAsync(string clientMnem, string shift = null, bool includeDeleted = false);
    
    // Bulk operations
    Task<int> BulkInsertAsync(List<RandomDrugScreenPerson> persons);
    Task<int> SoftDeleteByClientAsync(string clientMnem);
    Task<int> MarkMissingAsDeletedAsync(string clientMnem, List<string> existingNames);
}
```

### 2. Blazor UI Components (RandomDrugScreenUI)

#### 2.1 Configuration Updates

**Program.cs**:
```csharp
using LabBilling.Core.DataAccess;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configure AppEnvironment
builder.Services.AddSingleton<IAppEnvironment>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
 var appEnv = new AppEnvironment
    {
        DatabaseName = config.GetValue<string>("AppSettings:DatabaseName"),
    ServerName = config.GetValue<string>("AppSettings:ServerName"),
    LogDatabaseName = config.GetValue<string>("AppSettings:LogDatabaseName"),
  User = Environment.UserName
    };
    
    // Load system parameters
 using var uow = new UnitOfWorkSystem(appEnv);
    var systemService = new SystemService(appEnv, uow);
    appEnv.ApplicationParameters = systemService.LoadSystemParameters();
    
    return appEnv;
});

// Register services
builder.Services.AddScoped<IRandomDrugScreenService, RandomDrugScreenService>();
builder.Services.AddScoped<DictionaryService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkMain>();

var app = builder.Build();
```

**appsettings.json**:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
}
  },
  "AllowedHosts": "*",
  "AppSettings": {
    "DatabaseName": "LabBillingProd",
    "ServerName": "localhost",
    "LogDatabaseName": "LabBillingLogs"
  }
}
```

#### 2.2 Page Components

##### 2.2.1 Index/Home Page
**Location**: `RandomDrugScreenUI\Pages\Index.razor`

**Features**:
- Dashboard overview
- Quick access to main functions
- Recent selections summary
- Client statistics

##### 2.2.2 Candidate Management Page
**Location**: `RandomDrugScreenUI\Pages\CandidateManagement.razor`

**Features**:
- Client dropdown selector (populated from DictionaryService)
- Shift filter (optional)
- Candidate list grid (Blazor data grid or custom table)
  - Columns: Name, Shift, Last Test Date, Actions
  - Inline editing capability
  - Add/Edit/Delete actions
  - Soft delete indicator
- Search/filter functionality
- Export to CSV/Excel

**Components Used**:
- Custom grid component or use Blazor built-in virtualization
- Edit form modal/inline
- Confirmation dialogs

##### 2.2.3 Random Selection Page
**Location**: `RandomDrugScreenUI\Pages\RandomSelection.razor`

**Features**:
- Client selection dropdown
- Number of candidates input (with validation)
- Optional shift filter
- "Generate Selection" button
- Results display:
  - Selected candidates list
  - Non-selected candidates count
  - Selection timestamp
- Print/Export options
- Email integration (if needed)

**Validation**:
- Number must be >= 1
- Number cannot exceed available candidate pool
- Client must be selected

##### 2.2.4 Import Candidates Page
**Location**: `RandomDrugScreenUI\Pages\ImportCandidates.razor`

**Features**:
- Client selection
- Import mode selection (RTS/Replace All vs. Merge/Update)
- File upload component (CSV/Excel)
- File format validation
- Preview imported data before committing
- Progress indicator during import
- Import results summary (added, updated, deleted counts)

**File Format Support**:
- CSV with headers: Name, Shift, ClientMnemonic
- Excel (.xlsx) with same structure
- Tab-delimited text files

##### 2.2.5 Reporting Page
**Location**: `RandomDrugScreenUI\Pages\Reports.razor`

**Features**:
- Report type selector:
  - Selected candidates (by date range)
  - Non-selected candidates (by date range)
  - Client statistics
  - Selection history
- Date range picker
- Client filter
- Generate report button
- Display results in grid
- Export options (PDF, Excel, CSV)

#### 2.3 Shared Components

##### Navigation Menu
**Location**: `RandomDrugScreenUI\Shared\NavMenu.razor`

Update to include:
- Dashboard
- Candidate Management
- Random Selection
- Import Candidates
- Reports
- Settings (if needed)

##### MainLayout
**Location**: `RandomDrugScreenUI\Shared\MainLayout.razor`

- Standard layout with navigation sidebar
- Header with application title
- Footer with copyright/version info

### 3. Data Models & DTOs

#### 3.1 View Models (if needed)
**Location**: `RandomDrugScreenUI\Models\`

```csharp
public class CandidateViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ClientMnemonic { get; set; }
    public string ClientName { get; set; } // From Client lookup
    public string Shift { get; set; }
    public DateTime? TestDate { get; set; }
    public bool IsDeleted { get; set; }
}

public class SelectionRequest
{
    public string ClientMnemonic { get; set; }
    public int Count { get; set; }
    public string Shift { get; set; }
}

public class ImportResult
{
    public int TotalRecords { get; set; }
    public int AddedCount { get; set; }
    public int UpdatedCount { get; set; }
    public int DeletedCount { get; set; }
    public List<string> Errors { get; set; }
    public bool Success { get; set; }
}
```

### 4. Database Considerations

#### 4.1 Existing Table Structure
```sql
dbo.rds table columns:
- uri (int, PK, identity) - Unique record identifier
- deleted (bit) - Soft delete flag
- name (varchar) - Candidate name
- cli_mnem (varchar) - Client mnemonic (FK to client table)
- shift (varchar) - Shift information
- test_date (datetime) - Last test date
- mod_date (datetime) - Last modified date
- mod_user (varchar) - Last modified user
- mod_prg (varchar) - Last modified program
- mod_host (varchar) - Last modified host
- rowguid (uniqueidentifier) - Row GUID
```

#### 4.2 Potential Enhancements
Consider adding:
- Index on cli_mnem for faster client filtering
- Index on test_date for reporting queries
- Index on (cli_mnem, deleted) for active candidate queries

### 5. Business Rules Implementation

#### 5.1 Selection Rules
- **Validation**: 
  - Must select at least 1 candidate
  - Cannot select more than available non-deleted candidates
  - Optional shift filter narrows the pool
  
- **Random Algorithm**:
  - Use `RandomNumberGenerator.GetBytes()` for seed
  - Implement Fisher-Yates shuffle or random sampling
  - Ensure no duplicates in selection
  
- **Post-Selection**:
- Update test_date to current timestamp on selected candidates
  - Log selection event (could add to account notes or separate log table)

#### 5.2 Data Integrity
- **Soft Delete**: Never physically delete candidates, set deleted = 1
- **Audit Trail**: Update mod_date, mod_user, mod_prg, mod_host on every change
- **Client Validation**: Validate cli_mnem against Client table
- **Transaction Management**: All multi-record operations in UnitOfWork transactions

#### 5.3 Import Rules

**RTS Mode (Replace All)**:
1. Begin transaction
2. Soft delete all existing records for client (SET deleted=1)
3. OR Delete all records for client (DELETE FROM rds WHERE cli_mnem = @client)
4. Bulk insert new records
5. Commit transaction

**BTM/Merge Mode**:
1. Begin transaction
2. Get all existing names for client
3. Mark candidates not in import file as deleted
4. Update existing candidates with new data
5. Insert new candidates
6. Commit transaction

### 6. UI/UX Considerations

#### 6.1 Design Principles
- Clean, modern interface using Bootstrap (already referenced in Blazor template)
- Responsive design for tablet/desktop use
- Clear validation messages
- Loading indicators for async operations
- Confirmation dialogs for destructive actions
- Toast notifications for success/error messages

#### 6.2 Accessibility
- Proper ARIA labels
- Keyboard navigation support
- Screen reader friendly
- High contrast support

### 7. Testing Strategy

#### 7.1 Unit Tests
**Location**: `LabBillingCore.UnitTests\Services\RandomDrugScreenServiceTests.cs`

Test coverage:
- Random selection algorithm (distribution, no duplicates)
- Candidate count validation
- Soft delete behavior
- Import logic (both modes)
- Client and shift filtering
- Transaction rollback on errors

#### 7.2 Integration Tests
- Database operations
- Service layer interactions
- File import parsing

#### 7.3 UI Tests (Optional)
- Blazor component testing using bUnit
- E2E testing with Selenium or Playwright

### 8. Security Considerations

#### 8.1 Authentication
- Integrate with existing user authentication (UserAccount/UserProfile system)
- Role-based access if needed

#### 8.2 Authorization
- Determine if certain operations require specific permissions
- Audit all data modifications

#### 8.3 Data Validation
- Server-side validation of all inputs
- SQL injection prevention (using parameterized queries - already handled by PetaPoco)
- File upload validation (size limits, file type verification)

### 9. Deployment & Configuration

#### 9.1 Configuration
- Use ConnectionStringExtensions in the Utilities project to build connection strings. App settings should hold database / server names. 
- Environment-specific settings (Development, Staging, Production)
- Logging configuration

#### 9.2 Deployment Steps
1. Build solution
2. Run database migration scripts (if any schema changes needed)
3. Deploy Blazor Server app to IIS or Azure App Service
4. Configure application settings
5. Test in production environment

### 10. Future Enhancements

#### 10.1 Phase 2 Features
- No future features for now. 

#### 10.2 Performance Optimization
- Implement caching for frequently accessed data (clients, shifts)
- Database query optimization
- Lazy loading for large datasets
- Consider pagination for candidate lists

### 11. Documentation Requirements

#### 11.1 Technical Documentation
- API documentation (XML comments on all public methods)
- Database schema documentation
- Architecture decision records

#### 11.2 User Documentation
- User guide for each major feature
- Quick start guide
- FAQ
- Video tutorials (optional)

## Implementation Progress

### Phase 1: Foundation ? COMPLETED
- [x] Create RandomDrugScreenService interface and implementation
- [x] Enhance RandomDrugScreenPersonRepository with additional methods
- [x] Set up Blazor app configuration (Program.cs, appsettings.json)
- [x] Create base layout and navigation
- [x] Update IUnitOfWork and UnitOfWorkMain to include RandomDrugScreenPersonRepository
- [x] Create Dashboard/Index page with statistics
- [x] Fix ApplicationParameters initialization issue

**Completed Tasks:**
1. Created `RandomDrugScreenService` with full CRUD operations, random selection algorithm, and import functionality
2. Enhanced `RandomDrugScreenPersonRepository` with query methods:
   - GetByClientAsync
   - GetByClientAndShiftAsync  
   - GetDistinctClientsAsync
   - GetDistinctShiftsAsync
   - GetCandidateCountAsync
   - SoftDeleteByClientAsync
   - MarkMissingAsDeletedAsync
3. Updated Blazor app configuration:
- Configured dependency injection in Program.cs
   - Added AppSettings to appsettings.json
   - Registered IRandomDrugScreenService, DictionaryService, and IUnitOfWork
4. Updated navigation menu with RDS-specific links
5. Created Dashboard page showing candidate count and active clients
6. Added IDisposable to IUnitOfWorkSystem interface (bug fix)
7. **Fixed circular dependency issue**: ApplicationParameters now initialized with defaults before database load,
   preventing the chicken-and-egg problem with ConnectionString property

**Random Selection Algorithm:**
- Uses `RandomNumberGenerator` for cryptographically secure random number generation
- Implements proper randomization without bias
- Updates test_date on selected candidates
- Validates selection count against available pool

**Key Architecture Fix:**
The `AppEnvironment.ConnectionString` property requires `ApplicationParameters` to be non-null. During startup,
we now initialize `ApplicationParameters` with defaults first, then attempt to load from database. This prevents
SQL connection errors during app initialization.

**Next Steps:** Begin Phase 2 - Core Features

### Phase 2: Core Features ? COMPLETED
- [x] Implement Candidate Management page
- [x] Implement Random Selection page
- [x] Create shared components (grids, forms, dialogs)
- [x] Test random selection algorithm

**Completed Tasks:**
1. **Candidate Management Page** (`CandidateManagement.razor`):
   - Client dropdown filter with all clients from database
   - Shift filter (dynamically loaded based on selected client)
   - Show/hide deleted candidates toggle
   - Full CRUD operations (Add, Edit, Delete, Restore)
   - Inline modals for add/edit operations
   - Delete confirmation dialog
   - Real-time candidate count display
   - Responsive table layout with action buttons

2. **Random Selection Page** (`RandomSelection.razor`):
   - Client selection with validation
   - Optional shift filtering
   - Selection count input with validation against available pool
   - Real-time available candidate count
   - Selection info panel showing current parameters
   - Results display in formatted table
   - Export to CSV functionality (placeholder)
   - Print results functionality (placeholder)
   - Clear results option
   - Comprehensive error handling and validation messages

**UI Features Implemented:**
- Bootstrap 5 styling throughout
- Responsive design (mobile-friendly)
- Loading spinners for async operations
- Modal dialogs for data entry
- Confirmation dialogs for destructive actions
- Real-time validation feedback
- Success/error message alerts
- Disabled states during processing

**Business Logic Validated:**
- Selection count cannot exceed available candidates
- Client selection is required
- Soft delete implementation (candidates can be restored)
- Test date updates on selection
- Filter combinations work correctly (client + shift)

**Next Steps:** Begin Phase 3 - Import & Reporting

### Phase 3: Import & Reporting ? READY TO START
- [ ] Implement Import Candidates page
- [ ] File parsing logic (CSV)
- [ ] Implement Reports page
- [ ] Export functionality

## Open Questions for User

1. **Client Selection**: Should the system pull clients from the existing `Client` table in the database, or maintain a separate list?
   - Use existing Client table via DictionaryService

2. **Authentication**: Should this app integrate with the existing user authentication system, or run as a standalone app?
   - Integrate with existing UserAccount system

3. **Email Integration**: Priority for email functionality? Should it use existing email service or new implementation?
   - Not implementing

4. **Reporting Requirements**: What specific reports are most critical?
   - Selection history
   - Non-selected candidates
   - Client statistics
   - Others?

5. **Access Control**: Should different users have different permissions (e.g., some can only view, others can select/import)?
   - Start with single permission level, add role-based access later

6. **Data Migration**: Is there existing RDS data to migrate? If so, from what source?
   - There is existing data in dbo.rds table that the system will use

7. **Legacy System**: Should the new system remain compatible with the legacy RDS database structure, or can we optimize?
   - Keep existing structure for now, as model and repository already exist

8. **Shift Management**: Should shifts be predefined in a lookup table, or free-form text entry?
   - Free-form

9. **Selection History**: Should we track every selection made (requires new table), or only the most recent test_date?
   - Phase 1 uses test_date only, Phase 2 adds history table

10. **File Import Formats**: Which file formats are priority? CSV, Excel, both?
    - Start with CSV (simpler), add Excel in Phase 2

## Success Criteria

1. Users can manage candidates (add, edit, soft delete)
2. Users can perform random selections with proper validation
3. Import functionality works for bulk data loading
4. System maintains data integrity and audit trail
5. UI is intuitive and responsive
6. All critical paths have error handling
7. System integrates with existing LabBilling infrastructure
8. Performance is acceptable (< 2 seconds for typical operations)

## Risk Mitigation

| Risk | Impact | Mitigation |
|------|--------|-----------|
| Random algorithm bias | High | Use cryptographically secure RNG, add unit tests to verify distribution |
| Data loss during import | High | Use transactions, add confirmation step before commit |
| Performance with large datasets | Medium | Implement pagination, optimize queries, add indexes |
| User adoption | Medium | Create intuitive UI, provide training materials |
| Integration issues | Medium | Follow existing patterns closely, thorough testing |

---

**Next Steps**: 
1. Review this plan with stakeholders
2. Answer open questions
3. Get approval to proceed
4. Begin Phase 1 implementation
