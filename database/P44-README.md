# P44 — ListView master-detail (legacy / code-behind)

**Prerequisites:** P36 (`006`), P40 (`010`) — same procs as P40  
**View:** `OrdersLegacyView` — **no ViewModel**

## Open

**View → Orders legacy (P44)** or sidebar **Orders legacy (P44)**

Compare side-by-side with **Orders (P40)**.

## Same behavior as P40

| Action | Stored procedure |
|--------|------------------|
| Refresh master | `dbo.spGetOrderHeaders` |
| Select header | `dbo.spGetOrderLines @OrderId` |
| Save | `dbo.spSaveOrder` + TVP `@Lines` |

## P44 vs P40

| | P40 (`OrdersView`) | P44 (`OrdersLegacyView`) |
|--|-------------------|--------------------------|
| UI logic | `OrdersViewModel` (MVVM) | `.xaml.cs` event handlers |
| Master/detail bind | `ObservableCollection<T>` | `DataTable` → `DefaultView` |
| Grid columns | Explicit `DataGrid` columns | `AutoGenerateColumns` from `DataTable` |
| Testability | ViewModel unit-test friendly | UI + SQL wired in code-behind |
| FA reality | Newer screens | Many legacy bagging/MIS screens |

### Pros (legacy)

- Fast to wire grids directly from `ExecSP` result
- Familiar to teams maintaining old FA Views

### Cons (legacy)

- Validation and save logic duplicated in code-behind
- Harder to unit test; `DataRow` / `DataRowView` typing is weak
- Total quantity / busy state updated manually (`UpdateDetailTotal`, `SetBusy`)

## Acceptance checks

1. Refresh → master list fills from `DataTable`
2. Select order → detail grid reloads
3. New order → draft lines; Save → appears in master list
4. Open P40 — same orders visible (same database)
