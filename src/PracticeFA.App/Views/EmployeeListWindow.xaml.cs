using System.Data;
using System.Windows;
using PracticeFA.App.Models;
using PracticeFA.App.Services;

namespace PracticeFA.App.Views;

public partial class EmployeeListWindow : Window
{
    private DataView? _gridView;

    public EmployeeListWindow()
    {
        InitializeComponent();
        LoadGrid();
    }

    private void LoadGrid()
    {
        try
        {
            var table = EmployeeService.GetEmployees(activeOnly: true);
            _gridView = table.DefaultView;
            EmployeeGrid.ItemsSource = _gridView;
            StatusText.Text = $"{table.Rows.Count} active employee(s) — loaded via dbo.spGetEmployees";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Failed to load employees.";
            MessageBox.Show(
                $"Could not load employees.\n\n{ex.Message}\n\nRun database/scripts/002_P08_Employees.sql",
                "P08",
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

    private void Refresh_Click(object sender, RoutedEventArgs e) => LoadGrid();

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (!OpenEditDialog(null))
            return;
        LoadGrid();
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
        LoadGrid();
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
        LoadGrid();
    }

    private bool OpenEditDialog(EmployeeRecord? existing)
    {
        var dialog = new EmployeeEditWindow(existing) { Owner = this };
        return dialog.ShowDialog() == true;
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}
