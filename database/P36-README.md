# P36 — Transaction: header + lines save

**Prerequisites:** P08 (`002`) · **App:** Bagging Entry (2001) → `OrderService` → `dbo.spSaveOrder`

## Run script

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\006_P36_Orders.sql
```

## SSMS — prove rollback

```sql
USE PracticeFA;
DECLARE @L dbo.OrderLineInput;
INSERT @L (LineNumber, SkuOrStyle, Quantity) VALUES (1, N'STYLE-A', 10), (2, N'STYLE-B', 5);

-- Success
EXEC dbo.spSaveOrder
    @BagTag = N'BAG-OK', @OperatorBadge = N'E101', @PlantCode = N'P01',
    @CreatedBy = N'manager1', @Lines = @L;

-- Failure on line 2 (nothing saved)
EXEC dbo.spSaveOrder
    @BagTag = N'BAG-FAIL', @OperatorBadge = N'E101', @PlantCode = N'P01',
    @Lines = @L, @SimulateLine2Failure = 1;

SELECT * FROM dbo.OrderHeader WHERE BagTag = N'BAG-FAIL';  -- 0 rows
```

## WPF

1. **Master → Bagging Entry (2001)**
2. Search operator badge (e.g. `E101`)
3. Enter bag tag, edit lines, **Save order**
4. Check **Simulate line 2 failure** → Save → error; confirm `BAG-FAIL` header does not exist in SSMS
