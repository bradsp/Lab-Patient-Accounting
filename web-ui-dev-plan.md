# WinForms screen inventory and proposed Blazor mapping

Note on completeness
- This inventory was built from repository code search and inferred references. It may be incomplete due to code search result limitations.
- View and validate the full list of form files in GitHub UI using these searches:
  - Forms (.cs) in Lab PA WinForms UI: [code search](https://github.com/search?q=repo%3Abradsp%2FLab-Patient-Accounting+path%3A%22Lab+PA+WinForms+UI%2FForms%2F%22+language%3AC%23&type=code)
  - Form resources (.resx) in Lab PA WinForms UI: [code search](https://github.com/search?q=repo%3Abradsp%2FLab-Patient-Accounting+path%3A%22Lab+PA+WinForms+UI%2FForms%2F%22+filename%3Aresx&type=code)
  - UserControls in Lab PA WinForms UI: [code search](https://github.com/search?q=repo%3Abradsp%2FLab-Patient-Accounting+path%3A%22Lab+PA+WinForms+UI%2FUserControls%2F%22&type=code)

Conventions
- Pages are top-level, routable Blazor components under `Pages/`.
- Shared components (non-routable, reusable) under `Shared/` or feature subfolders.
- “Inferred” indicates presence implied by resources or references but code-behind not located in the search snapshot.

## Shell, auth, and navigation

| WinForms screen | Purpose | Proposed Blazor mapping | Route | Notes |
|---|---|---|---|---|
| [MainForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/MainForm.cs#L1-L109) | Shell + menu, MDI Tabs, logging config | `Shared/MainLayout.razor` + `Shared/NavMenu.razor` + tabbed navigation pattern | N/A (layout) | Replace MDI with router + tabs (optional). Menu -> NavMenu links. |
| [DashboardForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/DashboardForm.cs#L1-L122) | Updates/announcements panel | `Pages/Dashboard/Dashboard.razor` | `/` | Render announcements HTML; replace WinForms WebBrowser with sanitized HTML component. |
| [LoginForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/LoginForm.cs#L1-L54) | Manual credential dialog (fallback when Windows auth fails) | `Pages/Auth/Login.razor` (only if not using Windows Auth) | `/login` | Prefer Windows Authentication (IIS) for SSO parity; keep login page as fallback if needed. |
| [AboutBox.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/AboutBox.cs#L1-L108) | About dialog | `Shared/AboutDialog.razor` | N/A | Show app/env details; open as modal. |
| [AskCloseTabForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/AskCloseTabForm.cs#L1-L47) | Select a tab to close (MDI helper) | Not required; use built-in tab component UX | N/A | If you want explicit selection, create `Shared/SelectTabDialog.razor`. |

## Accounts, charges, insurance, worklists

| WinForms screen | Purpose | Proposed Blazor mapping | Route | Notes |
|---|---|---|---|---|
| [AccountForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/AccountForm.cs#L1-L116) | Account view/edit with tabs (demo, charges, insurance, dx…) | `Pages/Accounts/Account.razor` | `/accounts/{accountNo}` | Tabbed sub-components; grid virtualization; replace MessageBox with toasts/modals. |
| [NewAccountForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/NewAccountForm.cs#L1-L125) | Add account wizard/form | `Pages/Accounts/NewAccount.razor` | `/accounts/new` | Server-side validation + toasts; use InputDate, selects. |
| [AccountChargeEntry.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/AccountChargeEntry.cs#L1-L87) | Charges entry for current account (grid) | `Pages/Accounts/AccountCharges.razor` or embed in `Account.razor` | `/accounts/{accountNo}/charges` | Use a grid component with inline editing; CDM lookup modal. |
| [BatchChargeEntryForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/BatchChargeEntryForm.cs#L1-L102) | Batch charge entry grid | `Pages/Charges/BatchEntry.razor` | `/charges/batch` | Debounced CDM search; account picker modal. |
| [ChargeEntryForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/ChargeEntryForm.cs#L1-L117) | Charge entry dialog with CDM lookup | `Shared/Charges/ChargeEntryDialog.razor` | N/A | Debounce text input; modal with confirm/cancel. |
| [WorkListForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/WorkListForm.cs#L1-L116) | Worklist selection and accounts queue | `Pages/Worklists/Worklists.razor` | `/worklists` and `/worklists/{name}` | Tree view for queues; virtualized accounts grid; async loading. |
| [ErrorsForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/ErrorsForm.cs#L1-L77) | Error display and print | `Shared/ErrorViewer.razor` | N/A | Replace GDI printing with server-generated PDF or browser print. |

### User controls (Accounts domain)

| UserControl | Purpose | Proposed Blazor mapping | Notes |
|---|---|---|---|
| [ChargeMaintenanceUC.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/ChargeMaintenanceUC.cs#L1-L116) | Charges grid with grouping and admin actions | `Shared/Charges/ChargesGrid.razor` | Use a Blazor grid with grouping (e.g., MudBlazor, Radzen, DevExpress). |
| [InsMaintenanceUC.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/InsMaintenanceUC.cs#L1-L109) | Insurance maintenance (primary/secondary/tertiary) | `Shared/Insurance/InsuranceEditor.razor` | Parametrize coverage type; lookup plan/company. |
| [InsuranceLookup.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/InsuranceLookup.cs#L1-L113) | Autocomplete lookup for insurance | `Shared/Lookups/InsuranceLookup.razor` | Use debounced search, popover list. |
| [ProviderLookup.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/ProviderLookup.cs#L1-L123) | Autocomplete provider lookup | `Shared/Lookups/ProviderLookup.razor` | Debounce and highlight. |
| [LookupBox.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/LookupBox.cs#L1-L21) | Generic lookup box | `Shared/Lookups/LookupBox.razor` | Generic type param and display template. |
| [LabeledTextBox.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/LabeledTextBox.cs#L1-L18) | Label + input | Use `InputText` with label in form framework | Prefer framework form controls. |
| [CurrencyTextBox.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/CurrencyTextBox.cs#L1-L126) | Currency formatting textbox | `Shared/Inputs/CurrencyTextBox.razor` | Format via @bind with converter; culture-aware. |
| [DateTextBox.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/DateTextBox.cs#L1-L126) | Expression-capable date input | `Shared/Inputs/DateTextBox.razor` | Support T, T+N parsing server-side. |
| [LabDataGridView.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/LabDataGridView.cs#L1-L41) | Grid Enter→Tab override | N/A | Not needed; use grid component keyboard UX. |
| [MasterDetailDataGridView.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/MasterDetailDataGridView.cs#L1-L115) | Master-detail expansion | `Shared/Data/MasterDetailGrid.razor` | Use nested templates or grid with detail rows. |
| [MultiColumnComboBox.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/MultiColumnCombo.cs#L1-L139) | Multi-column dropdown | Replace with autocomplete popup list | Use a dropdown list with templated rows. |
| [WizardPages.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/UserControls/WizardPages.cs#L1-L25) | Hidden-tab wizard container | `Shared/Wizard/Wizard.razor` | Implement stepper with validation. |

## Interfaces, monitoring, mappings

| WinForms screen | Purpose | Proposed Blazor mapping | Route | Notes |
|---|---|---|---|---|
| [InterfaceMonitor.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/InterfaceMonitor.cs#L1-L106) | HL7 messages grid, queue counts | `Pages/Interfaces/Monitor.razor` | `/interfaces/monitor` | Use SignalR for live updates; virtualize grid; details pane shows message + errors. |
| [InterfaceMapping.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/InterfaceMapping.cs#L1-L44) | Mapping maintenance (code set, sending system) | `Pages/Interfaces/Mappings.razor` | `/interfaces/mappings` | Filter + grid CRUD; protect via admin policy. |

## System administration and security

| WinForms screen | Purpose | Proposed Blazor mapping | Route | Notes |
|---|---|---|---|---|
| [SystemParametersForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/SystemParametersForm.cs#L1-L51) | Edit system parameters (PropertyGrid) | `Pages/Admin/SystemParameters.razor` | `/admin/system-parameters` | Replace PropertyGrid with grouped forms; persist via SystemService. |
| [UserSecurity.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/UserSecurity.cs#L1-L124) | User admin, permissions | `Pages/Admin/Users.razor` | `/admin/users` | List + edit + password reset; RBAC enforcement. |
| [AccountLocksForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/AccountLocksForm.cs#L1-L51) | View/clear account locks | `Pages/Admin/AccountLocks.razor` | `/admin/account-locks` | Grid with clear action. |

## Billing, claims, remittances, collections

| WinForms screen | Purpose | Proposed Blazor mapping | Route | Notes |
|---|---|---|---|---|
| [ClaimsManagementForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/ClaimsManagementForm.cs#L1-L111) | Manage billing batches, compile claims | `Pages/Billing/ClaimsManagement.razor` | `/billing/claims` | Async job with progress; cancel token; results grid. |
| [PostRemittanceForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/PostRemittanceForm.cs#L1-L96) | View/post/unpost remittance 835 | `Pages/Billing/Remittances/RemittanceDetail.razor` | `/billing/remittances/{remittanceId}` | Render HTML details; claim lines grid and actions. |
| ProcessRemittanceForm (inferred from [resx](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/ProcessRemittanceForm.resx#L1-L69)) | Process inbound remittances | `Pages/Billing/Remittances/Process.razor` | `/billing/remittances/process` | Upload 835, parse, preview, process with progress. |
| [PatientCollectionsRunWizard.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/PatientCollectionsRunWizard.cs#L1-L104) | Send to collections, create statements, SFTP | `Pages/Billing/Collections/Run.razor` | `/billing/collections/run` | Stepper (wizard) with progress; SFTP service call. |
| [PatientCollectionsEditForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/PatientCollectionsEditForm.cs#L1-L99) | Edit single bad debt record | `Pages/Billing/Collections/CollectionDetail.razor` | `/billing/collections/{id}` | Form with save/cancel; date parsing. |
| PatientCollectionsForm (inferred from [resx](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/PatientCollectionsForm.resx#L1-L69)) | Collections list | `Pages/Billing/Collections/Collections.razor` | `/billing/collections` | Virtualized list, filter/search. |
| PaymentAdjustmentEntryForm (inferred from [resx](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/PaymentAdjustmentEntryForm.resx#L1-L69)) | Payment/adjustment entry | `Pages/Payments/AdjustmentEntry.razor` | `/payments/adjustments/new` or account-scoped | Integrate with account context as appropriate. |

## Dictionaries and reference data

| WinForms screen | Purpose | Proposed Blazor mapping | Route | Notes |
|---|---|---|---|---|
| HealthPlanMaintenanceEditForm ([HealtPlanMaintenanceEditForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/HealtPlanMaintenanceEditForm.cs#L1-L105)) | Insurance plan maintenance | `Pages/Admin/HealthPlans/Edit.razor` | `/admin/health-plans/{code}` | Claim filing indicator, fin code, plan metadata. |
| ClientMaintenanceForm (inferred from [resx](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/ClientMaintenanceForm.resx#L1-L69)) | Client dictionary | `Pages/Admin/Clients.razor` | `/admin/clients` | CRUD with search. |
| PhysicianMaintenanceForm (inferred from [resx](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/PhysicianMaintenanceForm.resx#L1-L69)) | Provider dictionary | `Pages/Admin/Physicians.razor` | `/admin/physicians` | CRUD with NPI. |
| AuditReportMaintenanceForm (inferred from [resx](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Forms/AuditReportMaintenanceForm.resx#L1-L69)) | Audit report config | `Pages/Admin/AuditReports.razor` | `/admin/audit-reports` | Configure stored reports. |
| InterfaceMapping form is listed in Interfaces section above. |  |  |  |  |

## Generic dialogs and helpers (WinForms library)

| WinForms screen | Purpose | Proposed Blazor mapping | Notes |
|---|---|---|---|
| [WaitForm.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/Lab%20PA%20WinForms%20UI/Library/WaitForm.cs#L1-L50) | Progress/Busy dialog | `Shared/BusyOverlay.razor` | Wrap long ops; expose `IProgress<T>`. |
| [FormResponse.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/WinFormsLibrary/FormResponse.cs#L1-L49) | Yes/No with filter list | `Shared/ConfirmDialog.razor` | Add optional checkbox list. |
| [DataGridToExcel.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/WinFormsLibrary/DataGridToExcel.cs#L1-L54) | Export grid to Excel | Backend export endpoint + client download | Use ClosedXML/OpenXML server-side. |
| [DataGridViewPrinter.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/WinFormsLibrary/DataGridViewPrinter.cs#L1-L102) | Print grid via GDI | Replace with PDF generation | Prefer QuestPDF/Syncfusion server-side. |
| [ToolStripDateTimePicker.cs](https://github.com/bradsp/Lab-Patient-Accounting/blob/67e39063198270bf3d543ff82aedcb5bf7e653d2/WinFormsLibrary/ToolStripDateTimePicker.cs#L1-L15) | Toolbar date picker host | Use page-level date pickers | Not needed in Blazor. |

## Additional inferred lookup/forms (from references)

| Name (inferred) | Purpose | Proposed Blazor mapping | Notes |
|---|---|---|---|
| CdmLookupForm | Search/select CDM | `Shared/Lookups/CdmLookup.razor` | Referenced by charge entry and batch charge screens. |
| PersonSearchForm | Find account/person | `Shared/Lookups/AccountLookup.razor` | Referenced by batch charge entry. |
| InsCompanyLookupForm | Lookup insurance company | `Shared/Lookups/InsuranceCompanyLookup.razor` | Created inside InsMaintenanceUC. |

## Notes on migration mechanics

- Grids: Use a Blazor grid with virtualization and templates (MudBlazor, Radzen, Syncfusion, DevExpress).
- Debounce: Replace System.Windows.Forms.Timer use with Blazor debounced input or `CancellationTokenSource`-based throttling.
- Modals: Replace MessageBox with modal components (Blazored.Modal/Bootstrap).
- Printing/Export: Replace GDI printing with server-generated PDF/Excel and browser downloads.
- WebBrowser/HTML: Render sanitized HTML in components; for external links, open target in new tab.
- Auth: Prefer Windows Authentication in IIS; map roles to existing UserStatus/permissions.
- Long-running tasks: Offload to background services (IHostedService) and update UI via SignalR.
