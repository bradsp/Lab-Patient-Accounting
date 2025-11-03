# Lab Outreach UI - Module Structure

## Overview
The application has been transformed from a single-purpose "Random Drug Screen" application into a modular "Lab Outreach" platform that can host multiple modules.

## Architecture Changes

### 1. Branding Update
- **Application Name**: Changed from "Random Drug Screen" to "Lab Outreach"
- **Page Title**: Updated throughout the application
- **Navigation**: Restructured to show module organization

### 2. Module Structure

#### Random Drug Screen Module (Active)
Located in `/rds/` route namespace:
- **Dashboard**: `/rds/dashboard` - Overview of RDS system with stats and quick actions
- **Manage Candidates**: `/candidates` - Full candidate management interface
- **Import Candidates**: `/import` - Bulk import functionality
- **Reports**: `/reports` - Reporting tools

#### Client Viewer Module (Placeholder)
Located in `/clients/` route namespace:
- **Client List**: `/clients` - Browse all clients (Coming Soon)
- **Client Details**: `/client-details` - View detailed client information (Coming Soon)

### 3. Navigation Menu Structure

The navigation menu (`NavMenu.razor`) is now organized into sections:
```
Home
??? HOME

RANDOM DRUG SCREEN MODULE
??? RDS Dashboard
??? Manage Candidates
??? Import Candidates
??? Reports

CLIENT VIEWER MODULE
??? Client List
??? Client Details (placeholder)
```

### 4. Home Page (`/`)
The home page now serves as a module selector showing:
- Module cards with descriptions
- Quick links to each module
- System overview statistics
- Module status (Active/Coming Soon)

### 5. File Structure

```
RandomDrugScreenUI/
??? Pages/
?   ??? Index.razor (Home/Module Selector)
?   ??? RDS/
?   ?   ??? RDSDashboard.razor (RDS-specific dashboard)
?   ??? Clients/
?   ?   ??? ClientList.razor (placeholder)
?   ?   ??? ClientDetails.razor (placeholder)
? ??? CandidateManagement.razor (existing)
?   ??? ImportCandidates.razor (existing)
?   ??? Reports.razor (existing)
??? Shared/
?   ??? MainLayout.razor (updated branding)
?   ??? NavMenu.razor (modular structure)
??? Authentication/
    ??? (existing auth files)
```

## Key Features

### Modular Design
- Each module is self-contained in its own route namespace
- Easy to add new modules without affecting existing functionality
- Clear separation of concerns

### Breadcrumb Navigation
- Added breadcrumb navigation to help users understand their location
- Consistent navigation pattern across all pages

### Placeholder Pages
- Client Viewer module pages include "Coming Soon" messaging
- Professional placeholder design maintains UI consistency
- Clear indication of planned features

### Existing Functionality Preserved
- All Random Drug Screen features remain fully functional
- No breaking changes to existing pages
- Authentication and authorization remain unchanged

## Benefits

1. **Scalability**: Easy to add new modules as requirements grow
2. **Organization**: Clear structure makes navigation intuitive
3. **Professional**: Polished UI with consistent design
4. **Future-Ready**: Framework in place for additional modules

## Next Steps

To add a new module:
1. Create a new folder under `Pages/` for the module
2. Add route pages with appropriate `@page` directives
3. Update `NavMenu.razor` to include the new module section
4. Add a module card to the home page (`Index.razor`)
5. Implement module-specific functionality

## Migration Notes

- Existing Random Drug Screen URLs remain valid
- The old dashboard content moved to `/rds/dashboard`
- Home page (`/`) now shows module overview instead of RDS dashboard
- No database changes required
- No configuration changes required
