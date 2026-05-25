-- P35 — Optional-filter search stored procedure (NULL / empty = no filter on that field)
-- Run after 002: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\005_P35_SearchEmployees.sql

USE PracticeFA;
GO

CREATE OR ALTER PROCEDURE dbo.spSearchEmployees
    @BadgeFragment NVARCHAR(20) = NULL,
    @ProcessCenter NVARCHAR(50) = NULL,
    @ActiveOnly    BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    IF @BadgeFragment = N'' SET @BadgeFragment = NULL;
    IF @ProcessCenter = N'' SET @ProcessCenter = NULL;

    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive
    FROM dbo.Employees
    WHERE (@ActiveOnly = 0 OR IsActive = 1)
      AND (@BadgeFragment IS NULL OR BadgeId LIKE N'%' + @BadgeFragment + N'%')
      AND (@ProcessCenter IS NULL OR ProcessCenter LIKE N'%' + @ProcessCenter + N'%')
    ORDER BY BadgeId;
END;
GO

-- Tests (row counts should match WPF grid after Search):
-- EXEC dbo.spSearchEmployees;                                    -- active only, no text filters
-- EXEC dbo.spSearchEmployees @BadgeFragment = N'E10';            -- badges containing E10
-- EXEC dbo.spSearchEmployees @ProcessCenter = N'CAST';           -- centers containing CAST
-- EXEC dbo.spSearchEmployees @ActiveOnly = 0;                    -- include inactive (E104)
-- EXEC dbo.spSearchEmployees @BadgeFragment = N'zzz';            -- 0 rows
