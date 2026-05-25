-- P38 — WeightGm column on Attendance for DecimalWeightConverter demo
-- Run after 008: sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i database\scripts\009_P38_AttendanceWeight.sql

USE PracticeFA;
GO

IF COL_LENGTH(N'dbo.Attendance', N'WeightGm') IS NULL
    ALTER TABLE dbo.Attendance ADD WeightGm DECIMAL(10, 3) NULL;
GO

UPDATE dbo.Attendance SET WeightGm = 1250.500 WHERE BadgeId = N'E101' AND WeightGm IS NULL;
UPDATE dbo.Attendance SET WeightGm = 980.125 WHERE BadgeId = N'E102' AND WeightGm IS NULL;
GO

CREATE OR ALTER PROCEDURE dbo.spGetAttendance
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AttendanceId, BadgeId, WorkDate, ClockInAt, ClockOutAt, Notes, SavedBy, SavedAt, WeightGm
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
    @SavedBy      NVARCHAR(20) = NULL,
    @WeightGm     DECIMAL(10, 3) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @AttendanceId IS NULL OR @AttendanceId = 0
    BEGIN
        INSERT INTO dbo.Attendance (BadgeId, WorkDate, ClockInAt, ClockOutAt, Notes, SavedBy, SavedAt, WeightGm)
        VALUES (@BadgeId, @WorkDate, @ClockInAt, @ClockOutAt, @Notes, @SavedBy, SYSUTCDATETIME(), @WeightGm);
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
            SavedAt = SYSUTCDATETIME(),
            WeightGm = @WeightGm
        WHERE AttendanceId = @AttendanceId;
    END

    SELECT AttendanceId, BadgeId, WorkDate, ClockInAt, ClockOutAt, Notes, SavedBy, SavedAt, WeightGm
    FROM dbo.Attendance
    WHERE AttendanceId = @AttendanceId;
END;
GO
