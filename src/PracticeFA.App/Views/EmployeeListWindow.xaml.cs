using System.Data;
using System.Windows;
using PracticeFA.App.Models;
using PracticeFA.App.Services;

namespace PracticeFA.App.Views;

public partial class EmployeeListWindow : Window
{
    private DataView? _gridView;

    public EmployeeListWindow(int moduleId = ModuleIds.EmployeeMaintenance)
    {
        ModuleId = moduleId;
        InitializeComponent();
        ModuleBadgeText.Text = ModuleId.ToString();
        HeaderText.Text = "Employee maintenance";
        Title = $"Employee maintenance ({ModuleId})";
        RunSearch();
    }

    public int ModuleId { get; }

    private void RunSearch()
    {
        try
        {
            var activeOnly = ActiveOnlyCheck.IsChecked == true;
            var table = EmployeeService.Search(
                BadgeFragmentBox.Text,
                ProcessCenterBox.Text,
                activeOnly);

            _gridView = table.DefaultView;
            EmployeeGrid.ItemsSource = _gridView;

            var count = table.Rows.Count;
            NoRecordsText.Visibility = count == 0 ? Visibility.Visible : Visibility.Collapsed;

            var badge = string.IsNullOrWhiteSpace(BadgeFragmentBox.Text) ? "(any)" : BadgeFragmentBox.Text.Trim();
            var center = string.IsNullOrWhiteSpace(ProcessCenterBox.Text) ? "(any)" : ProcessCenterBox.Text.Trim();
            var activeLabel = activeOnly ? "active only" : "active + inactive";

            StatusText.Text = count == 0
                ? $"No records — dbo.spSearchEmployees · badge={badge}, center={center}, {activeLabel}"
                : $"{count} employee(s) — P41: Deleted rows hidden when Active only · CreatedBy in grid · badge={badge}, {activeLabel}";
        }
        catch (Exception ex)
        {
            EmployeeGrid.ItemsSource = null;
            NoRecordsText.Visibility = Visibility.Collapsed;
            StatusText.Text = "Search failed.";
            MessageBox.Show(
                $"Could not search employees.\n\n{ex.Message}\n\nRun database/scripts/005_P35_SearchEmployees.sql (after 002).",
                "P35 Search",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private EmployeeRecord? GetSelectedEmployee()
    {
        if (EmployeeGrid.SelectedItem is not DataRowView rowView)
            return null;
        return EmployeeMapper.FromRow(rowView.Row);
    }

    private void Search_Click(object sender, RoutedEventArgs e) => RunSearch();

    private void Refresh_Click(object sender, RoutedEventArgs e) => RunSearch();

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (!OpenEditDialog(null))
            return;
        RunSearch();
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        var selected = GetSelectedEmployee();
        if (selected is null)
        {
            MessageBox.Show("Select a row to edit.", "Edit", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (!OpenEditDialog(selected))
            return;
        RunSearch();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        var selected = GetSelectedEmployee();
        if (selected is null)
        {
            MessageBox.Show("Select a row to delete.", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var confirm = MessageBox.Show(
            $"Deactivate employee {selected.BadgeId} — {selected.DisplayName}?",
            "Confirm delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes)
            return;

        var result = EmployeeService.SoftDelete(selected.EmployeeId);
        if (!result.Success)
        {
            MessageBox.Show(result.Message, "Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        StatusText.Text = result.Message;
        RunSearch();
    }

    private bool OpenEditDialog(EmployeeRecord? existing)
    {
        var dialog = new EmployeeEditWindow(existing) { Owner = this };
        return dialog.ShowDialog() == true;
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}
