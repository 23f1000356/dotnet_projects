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
        HeaderText.Text = existing is null ? "Add employee (P08)" : $"Edit employee — {existing.BadgeId}";
        Title = HeaderText.Text;

        if (existing is not null)
        {
            BadgeIdBox.Text = existing.BadgeId;
            DisplayNameBox.Text = existing.DisplayName;
            ProcessCenterBox.Text = existing.ProcessCenter ?? "";
            IsActiveCheck.IsChecked = existing.IsActive;
        }
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
            };
            result = EmployeeService.Update(updated);
        }

        if (!result.Success)
        {
            ErrorText.Text = result.Message;
            return;
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
