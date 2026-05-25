# P42 — TabControl wizard

**Prerequisites:** P36 (`006`), P39  
**View:** `OrderWizardView` + `OrderWizardViewModel`

## Open

**View → Order wizard (P42)** or sidebar **Order wizard (P42)**

## Three tabs

| Step | Tab | Validation on Next |
|------|-----|-------------------|
| 1 | Order info | Bag tag, operator badge, plant |
| 2 | Lines | At least one line, SKU + qty |
| 3 | Confirm | Review summary |

- **Back** — previous tab (no validation)
- **Next** — validates current tab before advancing
- **Save order** — only on tab 3; calls `IOrderService.Save` → `dbo.spSaveOrder`
- **Start over** — resets wizard to step 1

## Test

1. Step 1: leave bag tag empty → **Next** shows validation error
2. Fill header → **Next** → edit lines → **Next** → review → **Save**
3. Open **Orders (P40)** → new order appears in list
