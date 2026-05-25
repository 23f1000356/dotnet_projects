using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PracticeFA.P09.Models;
using PracticeFA.P09.Services;

namespace PracticeFA.P09;

public partial class MainWindow : Window
{
    private DataTable _table = new();
    private DataView? _legacyView;
    private ObservableCollection<EmployeeRowViewModel> _modernRows = new();
    private ICollectionView? _modernView;

    public MainWindow()
    {
        InitializeComponent();
        ReloadData();
    }

    private void Reload_Click(object sender, RoutedEventArgs e) => ReloadData();

    private void ReloadData()
    {
        var result = EmployeeDataLoader.Load(activeOnly: true);
        _table = result.Table;

        SourceText.Text = $"{result.SourceDescription} · {_table.Rows.Count} row(s) · Columns: {string.Join(", ", _table.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}";

        BindVersionA();
        BindVersionB();
    }

    private void BindVersionA()
    {
        _legacyView = _table.DefaultView;
        if (_table.Columns.Contains("IsActive"))
            _legacyView.RowFilter = "IsActive = true";

        LegacyGrid.ItemsSource = _legacyView;
    }

    private void BindVersionB()
    {
        var filtered = _table.Clone();
        foreach (DataRow row in _table.Rows)
        {
            if (_table.Columns.Contains("IsActive") && !Convert.ToBoolean(row["IsActive"]))
                continue;

            filtered.ImportRow(row);
        }

        _modernRows = EmployeeRowMapper.ToObservableCollection(filtered);
        _modernView = CollectionViewSource.GetDefaultView(_modernRows);
        ModernGrid.ItemsSource = _modernView;
        ApplyNameFilter();
    }

    private void SortByName_Click(object sender, RoutedEventArgs e)
    {
        if (_modernView is null)
            return;

        _modernView.SortDescriptions.Clear();
        _modernView.SortDescriptions.Add(new SortDescription(
            nameof(EmployeeRowViewModel.DisplayName),
            ListSortDirection.Ascending));
    }

    private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyNameFilter();

    private void ApplyNameFilter()
    {
        if (_modernView is null)
            return;

        var text = FilterBox.Text.Trim();
        if (string.IsNullOrEmpty(text))
        {
            _modernView.Filter = null;
            return;
        }

        _modernView.Filter = obj =>
            obj is EmployeeRowViewModel row &&
            row.DisplayName.Contains(text, StringComparison.OrdinalIgnoreCase);
    }
}
