-- P36 — Order header + lines saved atomically (BEGIN TRAN / COMMIT / ROLLBACK)
-- Run after 002: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\006_P36_Orders.sql

USE PracticeFA;
GO

IF OBJECT_ID(N'dbo.spSaveOrder', N'P') IS NOT NULL DROP PROCEDURE dbo.spSaveOrder;
GO
IF TYPE_ID(N'dbo.OrderLineInput') IS NOT NULL DROP TYPE dbo.OrderLineInput;
GO
IF OBJECT_ID(N'dbo.OrderLine', N'U') IS NOT NULL DROP TABLE dbo.OrderLine;
IF OBJECT_ID(N'dbo.OrderHeader', N'U') IS NOT NULL DROP TABLE dbo.OrderHeader;
GO

CREATE TABLE dbo.OrderHeader (
    OrderId        INT IDENTITY(1,1) PRIMARY KEY,
    BagTag         NVARCHAR(50)  NOT NULL,
    OperatorBadge  NVARCHAR(20)  NOT NULL,
    PlantCode      NVARCHAR(10)  NOT NULL,
    CreatedBy      NVARCHAR(20)  NULL,
    CreatedUtc     DATETIME2     NOT NULL CONSTRAINT DF_OrderHeader_CreatedUtc DEFAULT (SYSUTCDATETIME())
);
GO

CREATE TABLE dbo.OrderLine (
    OrderLineId   INT IDENTITY(1,1) PRIMARY KEY,
    OrderId       INT NOT NULL REFERENCES dbo.OrderHeader(OrderId) ON DELETE CASCADE,
    LineNumber    INT NOT NULL,
    SkuOrStyle    NVARCHAR(50) NOT NULL,
    Quantity      INT NOT NULL,
    CONSTRAINT CK_OrderLine_Quantity CHECK (Quantity > 0),
    CONSTRAINT UQ_OrderLine_Order_LineNumber UNIQUE (OrderId, LineNumber)
);
GO

CREATE TYPE dbo.OrderLineInput AS TABLE (
    LineNumber INT NOT NULL,
    SkuOrStyle NVARCHAR(50) NOT NULL,
    Quantity   INT NOT NULL
);
GO

CREATE OR ALTER PROCEDURE dbo.spSaveOrder
    @BagTag                NVARCHAR(50),
    @OperatorBadge         NVARCHAR(20),
    @PlantCode             NVARCHAR(10),
    @CreatedBy             NVARCHAR(20) = NULL,
    @Lines                 dbo.OrderLineInput READONLY,
    @SimulateLine2Failure  BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        DECLARE @OrderId INT;

        INSERT INTO dbo.OrderHeader (BagTag, OperatorBadge, PlantCode, CreatedBy)
        VALUES (@BagTag, @OperatorBadge, @PlantCode, @CreatedBy);

        SET @OrderId = SCOPE_IDENTITY();

        IF NOT EXISTS (SELECT 1 FROM @Lines)
            RAISERROR(N'At least one order line is required.', 16, 1);

        DECLARE @LineNumber INT, @SkuOrStyle NVARCHAR(50), @Quantity INT;

        DECLARE line_cursor CURSOR LOCAL FAST_FORWARD FOR
            SELECT LineNumber, SkuOrStyle, Quantity
            FROM @Lines
            ORDER BY LineNumber;

        OPEN line_cursor;
        FETCH NEXT FROM line_cursor INTO @LineNumber, @SkuOrStyle, @Quantity;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            IF @SimulateLine2Failure = 1 AND @LineNumber = 2
                RAISERROR(N'P36 test: simulated failure on line 2 (transaction rolled back).', 16, 1);

            IF @Quantity <= 0
                RAISERROR(N'Line quantity must be greater than zero.', 16, 1);

            INSERT INTO dbo.OrderLine (OrderId, LineNumber, SkuOrStyle, Quantity)
            VALUES (@OrderId, @LineNumber, @SkuOrStyle, @Quantity);

            FETCH NEXT FROM line_cursor INTO @LineNumber, @SkuOrStyle, @Quantity;
        END

        CLOSE line_cursor;
        DEALLOCATE line_cursor;

        COMMIT TRAN;

        SELECT OrderId, BagTag, OperatorBadge, PlantCode, CreatedBy, CreatedUtc
        FROM dbo.OrderHeader
        WHERE OrderId = @OrderId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        IF CURSOR_STATUS('local', 'line_cursor') >= 0
        BEGIN
            CLOSE line_cursor;
            DEALLOCATE line_cursor;
        END;

        DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @Severity INT = ERROR_SEVERITY();
        DECLARE @State INT = ERROR_STATE();
        RAISERROR(@Msg, @Severity, @State);
    END CATCH
END;
GO

-- Tests:
-- DECLARE @L dbo.OrderLineInput;
-- INSERT @L (LineNumber, SkuOrStyle, Quantity) VALUES (1, N'STYLE-A', 10), (2, N'STYLE-B', 5);
-- EXEC dbo.spSaveOrder @BagTag=N'BAG-001', @OperatorBadge=N'E101', @PlantCode=N'P01', @CreatedBy=N'manager1', @Lines=@L;
