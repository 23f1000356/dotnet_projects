-- P05 — Attendance table + get/save stored procedures
-- Run after 002: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\008_P05_Attendance.sql

USE PracticeFA;
GO

IF OBJECT_ID(N'dbo.Attendance', N'U') IS NOT NULL DROP TABLE dbo.Attendance;
GO

CREATE TABLE dbo.Attendance (
    AttendanceId INT IDENTITY(1,1) PRIMARY KEY,
    BadgeId      NVARCHAR(20)  NOT NULL,
    WorkDate     DATE          NOT NULL,
    ClockInAt    DATETIME2     NOT NULL,
    ClockOutAt   DATETIME2     NULL,
    Notes        NVARCHAR(200) NULL,
    SavedBy      NVARCHAR(20)  NULL,
    SavedAt      DATETIME2     NOT NULL CONSTRAINT DF_Attendance_SavedAt DEFAULT (SYSUTCDATETIME())
);
GO

INSERT INTO dbo.Attendance (BadgeId, WorkDate, ClockInAt, ClockOutAt, Notes, SavedBy) VALUES
    (N'E101', CAST(GETDATE() AS DATE), DATEADD(HOUR, -8, SYSUTCDATETIME()), DATEADD(HOUR, -1, SYSUTCDATETIME()), N'Casting shift', N'system'),
    (N'E102', CAST(GETDATE() AS DATE), DATEADD(HOUR, -7, SYSUTCDATETIME()), NULL, N'Still on floor', N'system');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Modules WHERE ModuleId = 7001)
    INSERT INTO dbo.Modules (ModuleId, ModuleName) VALUES (7001, N'Attendance List (P05)');

INSERT INTO dbo.UserAccess (UserId, ModuleId)
SELECT u.UserId, 7001
FROM (VALUES (N'operator1'), (N'manager1')) AS u(UserId)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.UserAccess ua
    WHERE ua.UserId = u.UserId AND ua.ModuleId = 7001);
GO

CREATE OR ALTER PROCEDURE dbo.spGetAttendance
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AttendanceId, BadgeId, WorkDate, ClockInAt, ClockOutAt, Notes, SavedBy, SavedAt
    FROM dbo.Attendance
    ORDER BY WorkDate DESC, ClockInAt DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spSaveAttendance
    @AttendanceId INT = 0,
    @BadgeId      NVARCHAR(20),
    @WorkDate     DATE,
    @ClockInAt    DATETIME2,
    @ClockOutAt   DATETIME2 = NULL,
    @Notes        NVARCHAR(200) = NULL,
    @SavedBy      NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @AttendanceId IS NULL OR @AttendanceId = 0
    BEGIN
        INSERT INTO dbo.Attendance (BadgeId, WorkDate, ClockInAt, ClockOutAt, Notes, SavedBy, SavedAt)
        VALUES (@BadgeId, @WorkDate, @ClockInAt, @ClockOutAt, @Notes, @SavedBy, SYSUTCDATETIME());
        SET @AttendanceId = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        UPDATE dbo.Attendance
        SET BadgeId = @BadgeId,
            WorkDate = @WorkDate,
            ClockInAt = @ClockInAt,
            ClockOutAt = @ClockOutAt,
            Notes = @Notes,
            SavedBy = @SavedBy,
            SavedAt = SYSUTCDATETIME()
        WHERE AttendanceId = @AttendanceId;
    END

    SELECT AttendanceId, BadgeId, WorkDate, ClockInAt, ClockOutAt, Notes, SavedBy, SavedAt
    FROM dbo.Attendance
    WHERE AttendanceId = @AttendanceId;
END;
GO

-- Tests:
-- EXEC dbo.spGetAttendance;
-- EXEC dbo.spSaveAttendance @BadgeId=N'E103', @WorkDate='2026-05-22', @ClockInAt='2026-05-22T07:00:00', @SavedBy=N'manager1';
