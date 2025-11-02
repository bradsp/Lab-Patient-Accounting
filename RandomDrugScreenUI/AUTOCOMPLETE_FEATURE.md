# Autocomplete Client Selector - Implementation Guide

## Overview
A reusable, searchable autocomplete component has been implemented to replace standard dropdown selects for client selection throughout the Random Drug Screen application. This provides a much better user experience when dealing with large numbers of clients.

## Component: `AutocompleteInput<TItem>`

### Location
`RandomDrugScreenUI\Shared\AutocompleteInput.razor`

### Features
- ? **Generic Component**: Works with any data type
- ? **Search Multiple Properties**: Searches both display name and value (e.g., "Client Name" and "CLI_MNEM")
- ? **Real-time Filtering**: Updates results as user types
- ? **Keyboard Support**: Tab, Enter, Escape navigation
- ? **Configurable**: Min search length, max results, placeholder text
- ? **Styled Dropdown**: Professional UI with hover effects
- ? **Accessible**: Proper focus management and blur handling

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Items` | `List<TItem>` | `new()` | Collection of items to search |
| `SearchProperty` | `Func<TItem, string>` | Required | Function to get searchable text from item |
| `ValueProperty` | `Func<TItem, string>` | Required | Function to get value from item |
| `ItemTemplate` | `RenderFragment<TItem>` | Required | Template for rendering each dropdown item |
| `OnItemSelected` | `EventCallback<TItem>` | Required | Callback when item is selected |
| `Placeholder` | `string` | `"Search..."` | Input placeholder text |
| `CssClass` | `string` | `""` | Additional CSS classes for input |
| `MaxResults` | `int` | `10` | Maximum results to display |
| `MinSearchLength` | `int` | `2` | Minimum characters before searching |

### Usage Example

```razor
<AutocompleteInput TItem="Client"
    Items="@clients"
    SearchProperty="@(c => $"{c.Name} ({c.ClientMnem})")"
    ValueProperty="@(c => c.ClientMnem)"
    ItemTemplate="@clientItemTemplate"
 OnItemSelected="@OnClientSelected"
    Placeholder="Search by name or mnemonic..."
    MinSearchLength="1"
    MaxResults="15" />

@code {
    private List<Client> clients = new();
    private string selectedClient = "";
    
    // Define template for dropdown items
    private RenderFragment<Client> clientItemTemplate = client => __builder =>
    {
        <div>
            <strong>@client.Name</strong>
      <br />
        <small class="text-muted">@client.ClientMnem</small>
        </div>
    };
    
    private async Task OnClientSelected(Client client)
    {
        selectedClient = client?.ClientMnem ?? "";
        // Additional logic here
        await Task.CompletedTask;
    }
}
```

## Implementation Details

### Search Algorithm
The component searches across both `SearchProperty` and `ValueProperty`:
- **SearchProperty**: Primary display text (e.g., "Hospital Name (HOSP)")
- **ValueProperty**: Value identifier (e.g., "HOSP")

This means users can search by:
1. Full or partial client name: "Hospital" ? finds "Hospital Name"
2. Client mnemonic: "HOSP" ? finds "Hospital Name (HOSP)"
3. Combination: "Hosp H" ? finds any match

### Dropdown Behavior
- Shows when input has focus and minimum characters are entered
- Hides when input loses focus (with 200ms delay to allow click)
- Displays "No results found" when search yields no matches
- Shows result count if more results exist than `MaxResults`

### Performance Considerations
- **Client-side filtering**: All filtering happens in browser (fast for reasonable data sets)
- **Configurable limits**: `MaxResults` prevents rendering too many items
- **Lazy rendering**: Only visible items are in the DOM

## Pages Updated

### 1. Candidate Management (`CandidateManagement.razor`)
- **Main filter**: Client selection with autocomplete
- **Modal**: Client selection when adding/editing candidates
- **Settings**: MinSearchLength=1, MaxResults=15

### 2. Random Selection (`RandomSelection.razor`)
- **Selection parameters**: Client selection with autocomplete
- **Settings**: MinSearchLength=1, MaxResults=15

### 3. Import Candidates (`ImportCandidates.razor`)
- **Import configuration**: Client selection with autocomplete
- **Settings**: MinSearchLength=1, MaxResults=15

### 4. Reports (`Reports.razor`)
- **Report parameters**: Client selection with autocomplete
- **Settings**: MinSearchLength=1, MaxResults=15

## Styling

### CSS File
`RandomDrugScreenUI\Shared\AutocompleteInput.razor.css`

### Key Styles
- **Dropdown positioning**: Absolute positioning below input
- **Scrollable results**: Max height 300px with overflow
- **Hover effects**: Visual feedback on item hover
- **Shadow**: Subtle box-shadow for depth
- **Border radius**: Matches Bootstrap form controls

### Customization
The component inherits Bootstrap styles and can be customized by:
1. Adding classes via `CssClass` parameter
2. Modifying `AutocompleteInput.razor.css`
3. Overriding styles in consuming page

## User Experience Improvements

### Before (Standard Dropdown)
- ? Scroll through hundreds of clients
- ? No search capability
- ? Must know exact client name
- ? Poor UX with large lists
- ? Mobile unfriendly

### After (Autocomplete)
- ? Type to search instantly
- ? Search by name OR mnemonic
- ? See formatted results (name + mnemonic)
- ? Only show relevant matches
- ? Mobile friendly
- ? Professional appearance
- ? Fast and responsive

## Testing Checklist

### Functional Testing
- [ ] Type client name - results appear
- [ ] Type client mnemonic - results appear
- [ ] Type partial match - filters correctly
- [ ] Select item - dropdown closes and value updates
- [ ] Click outside - dropdown closes
- [ ] No matches - shows "No results found"
- [ ] Min search length - requires minimum characters
- [ ] Max results - limits displayed items

### Cross-browser Testing
- [ ] Chrome
- [ ] Firefox
- [ ] Edge
- [ ] Safari (if applicable)

### Mobile Testing
- [ ] Touch selection works
- [ ] Keyboard appears appropriately
- [ ] Dropdown is scrollable
- [ ] Results are readable

## Future Enhancements (Optional)

### Potential Improvements
1. **Keyboard Navigation**: Arrow keys to navigate results
2. **Highlight Matching Text**: Bold the matching portion of results
3. **Loading Indicator**: Show spinner while filtering large datasets
4. **Recent Selections**: Remember and show recently selected items
5. **Debouncing**: Add debounce to reduce filtering frequency
6. **Server-side Search**: For very large datasets (1000+ clients)
7. **Multi-select**: Allow selecting multiple clients
8. **Clear Button**: X button to clear selection

### Server-side Search Example
For databases with thousands of clients, implement server-side filtering:

```csharp
// In service
public async Task<List<Client>> SearchClientsAsync(string searchTerm, int maxResults = 15)
{
    return await uow.ClientRepository
        .SearchAsync(searchTerm, maxResults);
}

// In repository
public async Task<List<Client>> SearchAsync(string searchTerm, int maxResults)
{
  var sql = Sql.Builder
        .Select("*")
        .From("dictionary.client")
        .Where("deleted = 0")
        .Where("(cli_nme LIKE @0 OR cli_mnem LIKE @0)", $"%{searchTerm}%")
        .OrderBy("cli_nme")
        .Limit(maxResults);
    
    return await Context.FetchAsync<Client>(sql);
}
```

## Troubleshooting

### Dropdown doesn't appear
- Check `MinSearchLength` - may need to type more characters
- Verify `Items` collection is populated
- Check browser console for errors

### Selection doesn't work
- Verify `OnItemSelected` callback is defined
- Check for JavaScript errors in console
- Ensure `ValueProperty` returns valid string

### Styling issues
- Check that `AutocompleteInput.razor.css` is loaded
- Verify Bootstrap 5 is available
- Check browser developer tools for CSS conflicts

## Performance Metrics

### Expected Performance
- **Load Time**: < 50ms for 1000 clients
- **Filter Time**: < 10ms per keystroke
- **Render Time**: < 100ms for 15 results
- **Memory**: Minimal impact (client-side filtering)

### Scaling Recommendations
- **< 500 clients**: Current implementation perfect
- **500-1000 clients**: Consider increasing `MinSearchLength` to 2-3
- **1000-5000 clients**: Implement server-side search
- **5000+ clients**: Use server-side search with pagination

## Summary

The autocomplete component provides a modern, user-friendly way to select clients from large lists. It's reusable, performant, and enhances the overall user experience across the application.

**Key Benefits:**
- ?? Fast client selection
- ?? Powerful search capabilities
- ?? Mobile-friendly
- ? Accessible
- ?? Professional appearance
- ?? Reusable across entire application
