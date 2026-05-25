-- P10 — splash / status bar database ping (optional; connection open is enough for capstone)
USE PracticeFA;
GO

CREATE OR ALTER PROCEDURE dbo.spPing
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        Ok = 1,
        ServerUtc = SYSUTCDATETIME(),
        DbName = DB_NAME();
END
GO

PRINT 'P10: dbo.spPing ready.';
