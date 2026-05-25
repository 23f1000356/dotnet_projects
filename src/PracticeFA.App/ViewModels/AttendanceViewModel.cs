using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PracticeFA.App.Models;
using PracticeFA.App.Services;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.ViewModels;

/// <summary>P05 MVVM + P39 DI — IAttendanceService injected (no static service calls).</summary>
public partial class AttendanceViewModel : ObservableObject
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceViewModel(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
        Records = new ObservableCollection<AttendanceRecord>();
    }

    public ObservableCollection<AttendanceRecord> Records { get; }

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyMessage = "Please wait…";

    [ObservableProperty]
    private string _statusMessage = "Press Refresh to load attendance (P39: resolved from DI).";

    [ObservableProperty]
    private int _editAttendanceId;

    [ObservableProperty]
    private string _editBadgeId = "E101";

    [ObservableProperty]
    private DateTime _editWorkDate = DateTime.Today;

    [ObservableProperty]
    private string _editClockIn = "07:00";

    [ObservableProperty]
    private string _editClockOut = "";

    [ObservableProperty]
    private string _editNotes = "";

    [ObservableProperty]
    private AttendanceRecord? _selectedRecord;

    partial void OnSelectedRecordChanged(AttendanceRecord? value)
    {
        if (value is null)
            return;

        EditAttendanceId = value.AttendanceId;
        EditBadgeId = value.BadgeId;
        EditWorkDate = value.WorkDate;
        EditClockIn = value.ClockInAt.ToLocalTime().ToString("HH:mm");
        EditClockOut = value.ClockOutAt.HasValue
            ? value.ClockOutAt.Value.ToLocalTime().ToString("HH:mm")
            : "";
        EditNotes = value.Notes ?? "";
        StatusMessage = $"Selected attendance #{value.AttendanceId} — edit and Save, or clear selection for new row.";
    }

    partial void OnIsBusyChanged(bool value)
    {
        RefreshCommand.NotifyCanExecuteChanged();
        SaveCommand.NotifyCanExecuteChanged();
        NewRecordCommand.NotifyCanExecuteChanged();
    }

    private bool CanRunCommands() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanRunCommands))]
    private async Task RefreshAsync()
    {
        IsBusy = true;
        BusyMessage = "Loading attendance…";
        StatusMessage = "Loading…";

        try
        {
            var rows = await Task.Run(_attendanceService.GetAll);
            Records.Clear();
            foreach (var row in rows)
                Records.Add(row);

            StatusMessage = $"{Records.Count} attendance row(s) — via IAttendanceService / IDataAccess";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Refresh failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanRunCommands))]
    private async Task SaveAsync()
    {
        var validation = ValidateEdit();
        if (!validation.Success)
        {
            StatusMessage = validation.Message;
            return;
        }

        IsBusy = true;
        BusyMessage = "Saving attendance…";
        StatusMessage = "Saving…";

        try
        {
            var clockIn = validation.ClockIn;
            var clockOut = validation.ClockOut;
            var savedBy = AppState.CurrentUser?.UserId;

            var result = await Task.Run(() => _attendanceService.Save(
                EditAttendanceId,
                EditBadgeId,
                EditWorkDate,
                clockIn,
                clockOut,
                string.IsNullOrWhiteSpace(EditNotes) ? null : EditNotes,
                savedBy,
                weightGm: null));

            if (!result.Success)
            {
                StatusMessage = result.Message;
                return;
            }

            StatusMessage = result.Message;
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Save failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanRunCommands))]
    private void NewRecord()
    {
        SelectedRecord = null;
        EditAttendanceId = 0;
        EditBadgeId = "E101";
        EditWorkDate = DateTime.Today;
        EditClockIn = DateTime.Now.ToString("HH:mm");
        EditClockOut = "";
        EditNotes = "";
        StatusMessage = "New attendance row — fill fields and Save.";
    }

    private (bool Success, string Message, DateTime ClockIn, DateTime? ClockOut) ValidateEdit()
    {
        if (string.IsNullOrWhiteSpace(EditBadgeId))
            return (false, "Badge ID is required.", default, null);

        if (!TimeSpan.TryParse(EditClockIn, out var inTime))
            return (false, "Clock-in time use HH:mm (e.g. 07:30).", default, null);

        DateTime? clockOut = null;
        if (!string.IsNullOrWhiteSpace(EditClockOut))
        {
            if (!TimeSpan.TryParse(EditClockOut, out var outTime))
                return (false, "Clock-out time use HH:mm or leave empty.", default, null);
            clockOut = EditWorkDate.Date.Add(outTime);
        }

        var clockIn = EditWorkDate.Date.Add(inTime);

        if (clockOut.HasValue && clockOut.Value <= clockIn)
            return (false, "Clock-out must be after clock-in.", default, null);

        return (true, "", clockIn, clockOut);
    }
}
