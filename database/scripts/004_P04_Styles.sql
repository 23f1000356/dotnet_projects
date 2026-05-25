-- P04 — Styles table + save/list stored procedures (Style Creation module 1001)
-- Run after 001: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\004_P04_Styles.sql

USE PracticeFA;
GO

IF OBJECT_ID(N'dbo.Styles', N'U') IS NOT NULL DROP TABLE dbo.Styles;
GO

CREATE TABLE dbo.Styles (
    StyleId      INT IDENTITY(1,1) PRIMARY KEY,
    StyleCode    NVARCHAR(20)  NOT NULL UNIQUE,
    Description  NVARCHAR(500) NOT NULL,
    CreatedBy    NVARCHAR(20)  NULL,
    CreatedUtc   DATETIME2     NOT NULL CONSTRAINT DF_Styles_CreatedUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedUtc   DATETIME2     NULL
);
GO

INSERT INTO dbo.Styles (StyleCode, Description, CreatedBy) VALUES
    (N'DEMO-001', N'Sample casting style for practice', N'manager1'),
    (N'DEMO-002', N'Sample FSK style for practice', N'operator1');
GO

CREATE OR ALTER PROCEDURE dbo.spGetStyles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT StyleId, StyleCode, Description, CreatedBy, CreatedUtc, UpdatedUtc
    FROM dbo.Styles
    ORDER BY StyleCode;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spSaveStyle
    @StyleCode   NVARCHAR(20),
    @Description NVARCHAR(500),
    @CreatedBy   NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Styles WHERE StyleCode = @StyleCode)
    BEGIN
        UPDATE dbo.Styles
        SET Description = @Description,
            UpdatedUtc = SYSUTCDATETIME()
        WHERE StyleCode = @StyleCode;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.Styles (StyleCode, Description, CreatedBy)
        VALUES (@StyleCode, @Description, @CreatedBy);
    END

    SELECT StyleId, StyleCode, Description, CreatedBy, CreatedUtc, UpdatedUtc
    FROM dbo.Styles
    WHERE StyleCode = @StyleCode;
END;
GO

-- Tests:
-- EXEC dbo.spGetStyles;
-- EXEC dbo.spSaveStyle @StyleCode = N'TEST-01', @Description = N'Test description', @CreatedBy = N'operator1';
