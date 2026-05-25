-- P40 — Read order headers + lines for master-detail MVVM
-- Run after 006: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\010_P40_OrdersRead.sql

USE PracticeFA;
GO

CREATE OR ALTER PROCEDURE dbo.spGetOrderHeaders
AS
BEGIN
    SET NOCOUNT ON;

    SELECT h.OrderId,
           h.BagTag,
           h.OperatorBadge,
           h.PlantCode,
           h.CreatedBy,
           h.CreatedUtc,
           LineCount = COUNT(l.OrderLineId),
           TotalQuantity = ISNULL(SUM(l.Quantity), 0)
    FROM dbo.OrderHeader h
    LEFT JOIN dbo.OrderLine l ON l.OrderId = h.OrderId
    GROUP BY h.OrderId, h.BagTag, h.OperatorBadge, h.PlantCode, h.CreatedBy, h.CreatedUtc
    ORDER BY h.OrderId DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spGetOrderLines
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT OrderLineId, OrderId, LineNumber, SkuOrStyle, Quantity
    FROM dbo.OrderLine
    WHERE OrderId = @OrderId
    ORDER BY LineNumber;
END;
GO

-- Tests:
-- EXEC dbo.spGetOrderHeaders;
-- EXEC dbo.spGetOrderLines @OrderId = 1;
