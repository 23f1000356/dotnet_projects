using System.Windows;
using PracticeFA.App.Models;
using PracticeFA.App.Services;

namespace PracticeFA.App.Views;

public partial class EmployeeEditWindow : Window
{
    private readonly EmployeeRecord? _existing;

    public EmployeeEditWindow(EmployeeRecord? existing)
    {
        _existing = existing;
        InitializeComponent();
        HeaderText.Text = existing is null ? "Add employee (P08 + P41)" : $"Edit employee — {existing.BadgeId}";
        Title = HeaderText.Text;

        if (existing is not null)
        {
            BadgeIdBox.Text = existing.BadgeId;
            DisplayNameBox.Text = existing.DisplayName;
            ProcessCenterBox.Text = existing.ProcessCenter ?? "";
            IsActiveCheck.IsChecked = existing.IsActive;
            AuditText.Text = FormatAudit(existing);
            AuditText.Visibility = Visibility.Visible;
        }
        else
        {
            AuditText.Text = $"New row will be created by: {EmployeeService.CurrentUserId ?? "(sign in again)"}";
            AuditText.Visibility = Visibility.Visible;
        }
    }

    private static string FormatAudit(EmployeeRecord e)
    {
        var created = e.CreatedAt.HasValue
            ? e.CreatedAt.Value.ToLocalTime().ToString("g")
            : "—";
        var modified = e.ModifiedAt.HasValue
            ? $"{e.ModifiedBy ?? "—"} at {e.ModifiedAt.Value.ToLocalTime():g}"
            : "—";
        return $"Created by {e.CreatedBy ?? "—"} at {created}\nLast modified: {modified}";
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Text = "";
        OperationResult result;

        if (_existing is null)
        {
            result = EmployeeService.Insert(
                BadgeIdBox.Text,
                DisplayNameBox.Text,
                ProcessCenterBox.Text);
        }
        else
        {
            var updated = new EmployeeRecord
            {
                EmployeeId = _existing.EmployeeId,
                BadgeId = BadgeIdBox.Text,
                DisplayName = DisplayNameBox.Text,
                ProcessCenter = string.IsNullOrWhiteSpace(ProcessCenterBox.Text)
                    ? null
                    : ProcessCenterBox.Text.Trim(),
                IsActive = IsActiveCheck.IsChecked == true,
                CreatedBy = _existing.CreatedBy,
                CreatedAt = _existing.CreatedAt,
            };
            result = EmployeeService.Update(updated);
        }

        if (!result.Success)
        {
            ErrorText.Text = result.Message;
            return;
        }

        if (result.Employee is not null && _existing is not null)
        {
            AuditText.Text = FormatAudit(result.Employee);
        }

        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
