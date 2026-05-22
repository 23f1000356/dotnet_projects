-- P08 — Employees table + CRUD stored procedures
-- Run after 001: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\002_P08_Employees.sql

USE PracticeFA;
GO

IF OBJECT_ID(N'dbo.Employees', N'U') IS NOT NULL DROP TABLE dbo.Employees;
GO

CREATE TABLE dbo.Employees (
    EmployeeId    INT IDENTITY(1,1) PRIMARY KEY,
    BadgeId       NVARCHAR(20)  NOT NULL UNIQUE,
    DisplayName   NVARCHAR(100) NOT NULL,
    ProcessCenter NVARCHAR(50)  NULL,
    IsActive      BIT NOT NULL CONSTRAINT DF_Employees_IsActive DEFAULT (1)
);
GO

INSERT INTO dbo.Employees (BadgeId, DisplayName, ProcessCenter, IsActive) VALUES
    (N'E101', N'Sara Chen', N'CASTING', 1),
    (N'E102', N'Raj Patel', N'FSK', 1),
    (N'E103', N'Mia Lopez', N'GRINDING', 1),
    (N'E104', N'Inactive User', N'CASTING', 0);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Modules WHERE ModuleId = 6001)
    INSERT INTO dbo.Modules (ModuleId, ModuleName) VALUES (6001, N'Employee Maintenance');

INSERT INTO dbo.UserAccess (UserId, ModuleId)
SELECT u.UserId, 6001
FROM (VALUES (N'operator1'), (N'manager1')) AS u(UserId)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.UserAccess ua
    WHERE ua.UserId = u.UserId AND ua.ModuleId = 6001);
GO

CREATE OR ALTER PROCEDURE dbo.spGetEmployees
    @ActiveOnly BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive
    FROM dbo.Employees
    WHERE @ActiveOnly = 0 OR IsActive = 1
    ORDER BY BadgeId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spGetEmployeeByBadge
    @BadgeId NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive
    FROM dbo.Employees
    WHERE BadgeId = @BadgeId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spInsEmployee
    @BadgeId       NVARCHAR(20),
    @DisplayName   NVARCHAR(100),
    @ProcessCenter NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Employees (BadgeId, DisplayName, ProcessCenter, IsActive)
    VALUES (@BadgeId, @DisplayName, @ProcessCenter, 1);

    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive
    FROM dbo.Employees
    WHERE EmployeeId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE dbo.spUpdEmployee
    @EmployeeId    INT,
    @BadgeId       NVARCHAR(20),
    @DisplayName   NVARCHAR(100),
    @ProcessCenter NVARCHAR(50) = NULL,
    @IsActive      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Employees
    SET BadgeId = @BadgeId,
        DisplayName = @DisplayName,
        ProcessCenter = @ProcessCenter,
        IsActive = @IsActive
    WHERE EmployeeId = @EmployeeId;

    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive
    FROM dbo.Employees
    WHERE EmployeeId = @EmployeeId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spDelEmployee
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Employees SET IsActive = 0 WHERE EmployeeId = @EmployeeId;
END;
GO

-- Tests:
-- EXEC dbo.spGetEmployees @ActiveOnly = 1;
-- EXEC dbo.spGetEmployeeByBadge @BadgeId = N'E101';
