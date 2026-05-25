-- P09 — Add Email to spGetEmployees (Version A: grid shows new column without C# model change on legacy binding)
-- Run after 002: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\003_P09_EmployeeEmail.sql

USE PracticeFA;
GO

IF COL_LENGTH('dbo.Employees', 'Email') IS NULL
    ALTER TABLE dbo.Employees ADD Email NVARCHAR(120) NULL;
GO

UPDATE dbo.Employees SET Email = LOWER(REPLACE(DisplayName, ' ', '.')) + N'@practice.local'
WHERE Email IS NULL;
GO

CREATE OR ALTER PROCEDURE dbo.spGetEmployees
    @ActiveOnly BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, Email, IsActive
    FROM dbo.Employees
    WHERE @ActiveOnly = 0 OR IsActive = 1
    ORDER BY BadgeId;
END;
GO

-- EXEC dbo.spGetEmployees @ActiveOnly = 1;
