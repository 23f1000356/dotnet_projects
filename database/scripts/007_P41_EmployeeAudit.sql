-- P41 — Audit columns + soft delete (ModifiedBy on update/delete)
-- Run after 002 (and 005 if using search): sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\007_P41_EmployeeAudit.sql

USE PracticeFA;
GO

IF COL_LENGTH(N'dbo.Employees', N'CreatedBy') IS NULL
    ALTER TABLE dbo.Employees ADD CreatedBy NVARCHAR(20) NULL;
IF COL_LENGTH(N'dbo.Employees', N'CreatedAt') IS NULL
    ALTER TABLE dbo.Employees ADD CreatedAt DATETIME2 NULL;
IF COL_LENGTH(N'dbo.Employees', N'ModifiedBy') IS NULL
    ALTER TABLE dbo.Employees ADD ModifiedBy NVARCHAR(20) NULL;
IF COL_LENGTH(N'dbo.Employees', N'ModifiedAt') IS NULL
    ALTER TABLE dbo.Employees ADD ModifiedAt DATETIME2 NULL;
GO

UPDATE dbo.Employees
SET CreatedBy = COALESCE(CreatedBy, N'system'),
    CreatedAt = COALESCE(CreatedAt, SYSUTCDATETIME())
WHERE CreatedBy IS NULL OR CreatedAt IS NULL;
GO

CREATE OR ALTER PROCEDURE dbo.spGetEmployees
    @ActiveOnly BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive,
           CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
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
    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive,
           CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
    FROM dbo.Employees
    WHERE BadgeId = @BadgeId;
END;
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

    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive,
           CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
    FROM dbo.Employees
    WHERE (@ActiveOnly = 0 OR IsActive = 1)
      AND (@BadgeFragment IS NULL OR BadgeId LIKE N'%' + @BadgeFragment + N'%')
      AND (@ProcessCenter IS NULL OR ProcessCenter LIKE N'%' + @ProcessCenter + N'%')
    ORDER BY BadgeId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spInsEmployee
    @BadgeId       NVARCHAR(20),
    @DisplayName   NVARCHAR(100),
    @ProcessCenter NVARCHAR(50) = NULL,
    @CreatedBy     NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Employees (BadgeId, DisplayName, ProcessCenter, IsActive, CreatedBy, CreatedAt)
    VALUES (@BadgeId, @DisplayName, @ProcessCenter, 1, @CreatedBy, SYSUTCDATETIME());

    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive,
           CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
    FROM dbo.Employees
    WHERE EmployeeId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE dbo.spUpdEmployee
    @EmployeeId    INT,
    @BadgeId       NVARCHAR(20),
    @DisplayName   NVARCHAR(100),
    @ProcessCenter NVARCHAR(50) = NULL,
    @IsActive      BIT = 1,
    @ModifiedBy    NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Employees
    SET BadgeId = @BadgeId,
        DisplayName = @DisplayName,
        ProcessCenter = @ProcessCenter,
        IsActive = @IsActive,
        ModifiedBy = @ModifiedBy,
        ModifiedAt = SYSUTCDATETIME()
    WHERE EmployeeId = @EmployeeId;

    SELECT EmployeeId, BadgeId, DisplayName, ProcessCenter, IsActive,
           CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
    FROM dbo.Employees
    WHERE EmployeeId = @EmployeeId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spDelEmployee
    @EmployeeId  INT,
    @ModifiedBy  NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Employees
    SET IsActive = 0,
        ModifiedBy = @ModifiedBy,
        ModifiedAt = SYSUTCDATETIME()
    WHERE EmployeeId = @EmployeeId;
END;
GO

-- Tests:
-- EXEC dbo.spGetEmployees @ActiveOnly = 1;
-- EXEC dbo.spInsEmployee @BadgeId=N'E199', @DisplayName=N'Test User', @ProcessCenter=N'CASTING', @CreatedBy=N'manager1';
-- EXEC dbo.spDelEmployee @EmployeeId = 5, @ModifiedBy = N'manager1';
-- SELECT * FROM dbo.Employees WHERE BadgeId = N'E199';  -- IsActive=0, ModifiedBy set
