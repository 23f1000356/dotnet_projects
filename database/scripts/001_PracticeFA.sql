-- Practice FA — P06 database (LocalDB / SQL Server Express)
-- Run in SSMS or: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\001_PracticeFA.sql

IF DB_ID(N'PracticeFA') IS NULL
    CREATE DATABASE PracticeFA;
GO

USE PracticeFA;
GO

IF OBJECT_ID(N'dbo.UserAccess', N'U') IS NOT NULL DROP TABLE dbo.UserAccess;
IF OBJECT_ID(N'dbo.Modules', N'U') IS NOT NULL DROP TABLE dbo.Modules;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users (
    UserId       NVARCHAR(20)  NOT NULL PRIMARY KEY,
    PasswordHash NVARCHAR(100) NOT NULL,  -- plain text for practice only
    DisplayName  NVARCHAR(100) NOT NULL,
    PlantCode    NVARCHAR(10)  NOT NULL
);

CREATE TABLE dbo.Modules (
    ModuleId   INT          NOT NULL PRIMARY KEY,
    ModuleName NVARCHAR(50) NOT NULL
);

CREATE TABLE dbo.UserAccess (
    UserId   NVARCHAR(20) NOT NULL REFERENCES dbo.Users(UserId),
    ModuleId INT          NOT NULL REFERENCES dbo.Modules(ModuleId),
    PRIMARY KEY (UserId, ModuleId)
);
GO

INSERT INTO dbo.Users (UserId, PasswordHash, DisplayName, PlantCode) VALUES
    (N'operator1', N'pass1', N'Floor Operator One', N'P01'),
    (N'manager1',  N'pass1', N'Plant Manager',      N'P01'),
    (N'operator2', N'pass2', N'Floor Operator Two', N'P02');

INSERT INTO dbo.Modules (ModuleId, ModuleName) VALUES
    (1001, N'Style Creation'),
    (2001, N'Bagging Entry'),
    (3001, N'MIS Productivity'),
    (4001, N'Reports Hub'),
    (5001, N'Admin Tools');

INSERT INTO dbo.UserAccess (UserId, ModuleId) VALUES
    (N'operator1', 1001),
    (N'operator1', 2001),
    (N'operator1', 4001),
    (N'manager1',  1001),
    (N'manager1',  2001),
    (N'manager1',  3001),
    (N'manager1',  4001),
    (N'manager1',  5001),
    (N'operator2', 2001),
    (N'operator2', 4001);
GO

CREATE OR ALTER PROCEDURE dbo.spLogin
    @UserId   NVARCHAR(20),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UserId, DisplayName, PlantCode
    FROM dbo.Users
    WHERE UserId = @UserId
      AND PasswordHash = @Password;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spGetUserModules
    @UserId NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT m.ModuleId, m.ModuleName
    FROM dbo.UserAccess ua
    INNER JOIN dbo.Modules m ON m.ModuleId = ua.ModuleId
    WHERE ua.UserId = @UserId
    ORDER BY m.ModuleId;
END;
GO

-- SSMS test:
-- EXEC dbo.spLogin @UserId = N'operator1', @Password = N'pass1';
-- EXEC dbo.spGetUserModules @UserId = N'operator1';
