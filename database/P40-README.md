# P40 — Master-detail MVVM (orders)

**Prerequisites:** P05, P36 (`006`), P39  
**View:** `OrdersView` + `OrdersViewModel`

## Run SQL

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\010_P40_OrdersRead.sql
```

Requires `006_P36_Orders.sql` for tables + `spSaveOrder`.

## Open

**View → Orders (P40)** or sidebar **Orders (P40)**

## Master-detail flow

1. **Master** `ListView` — `spGetOrderHeaders` (bag tag, line count, total qty)
2. **Select** a row — `spGetOrderLines @OrderId` loads detail grid
3. **Add line** / **Remove last** — updates `DetailTotalQuantity` (computed)
4. **New order** — draft header + lines
5. **Save order** — `IOrderService.Save` → `dbo.spSaveOrder` (P36 transaction)

## Acceptance checks

- Change selection → lines reload
- Add line → total updates in `DetailTotalDisplay`
- No SQL in View code-behind — `IOrderService` only
