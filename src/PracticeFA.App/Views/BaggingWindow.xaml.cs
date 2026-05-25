using System.Collections.ObjectModel;
using System.Windows;
using PracticeFA.App.Models;
using PracticeFA.App.Services;

namespace PracticeFA.App.Views;

public partial class BaggingWindow : Window
{
    private readonly ObservableCollection<BaggingLineRow> _lines = new();
    private bool _savedThisSession;

    public BaggingWindow(int moduleId = ModuleIds.BaggingEntry)
    {
        ModuleId = moduleId;
        DataContext = new SearchHostContext("E205", "Bagging floor");
        InitializeComponent();
        ModuleBadgeText.Text = ModuleId.ToString();
        HeaderText.Text = "Bagging Entry";
        Title = $"Bagging Entry ({ModuleId})";

        PlantText.Text = $"Plant: {AppState.CurrentUser?.PlantCode ?? "P01"} · Created by: {AppState.CurrentUser?.UserId ?? "(unknown)"}";

        _lines.Add(new BaggingLineRow(1, "STYLE-A", 10));
        _lines.Add(new BaggingLineRow(2, "STYLE-B", 5));
        LinesGrid.ItemsSource = _lines;
    }

    public int ModuleId { get; }

    private void EmployeeSearch_SearchRequested(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SearchHostContext ctx)
            return;

        if (string.IsNullOrWhiteSpace(ctx.SearchBadge))
        {
            SearchResultText.Text = $"[{ctx.HostName}] Enter operator badge.";
            return;
        }

        try
        {
            var employee = EmployeeService.GetByBadge(ctx.SearchBadge);
            SearchResultText.Text = employee is null
                ? $"[{ctx.HostName}] No employee for badge {ctx.SearchBadge}."
                : $"[{ctx.HostName}] Operator {employee.DisplayName} ({employee.BadgeId})";
        }
        catch (Exception ex)
        {
            SearchResultText.Text = $"Search failed: {ex.Message}";
        }
    }

    private string? GetOperatorBadge() =>
        DataContext is SearchHostContext ctx && !string.IsNullOrWhiteSpace(ctx.SearchBadge)
            ? ctx.SearchBadge.Trim()
            : null;

    private IReadOnlyList<OrderLineInput> CollectLines() =>
        _lines
            .Select(r => new OrderLineInput
            {
                LineNumber = r.LineNumber,
                SkuOrStyle = r.SkuOrStyle,
                Quantity = r.Quantity,
            })
            .ToList();

    private void AddLine_Click(object sender, RoutedEventArgs e)
    {
        var next = _lines.Count == 0 ? 1 : _lines.Max(l => l.LineNumber) + 1;
        _lines.Add(new BaggingLineRow(next, "", 1));
    }

    private void RemoveLine_Click(object sender, RoutedEventArgs e)
    {
        if (LinesGrid.SelectedItem is BaggingLineRow row)
            _lines.Remove(row);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Text = "";
        SuccessText.Visibility = Visibility.Collapsed;

        var operatorBadge = GetOperatorBadge();
        if (operatorBadge is null)
        {
            ErrorText.Text = "Search and confirm an operator badge before saving.";
            return;
        }

        var plant = AppState.CurrentUser?.PlantCode ?? "P01";
        var createdBy = AppState.CurrentUser?.UserId;

        var result = OrderService.Save(
            BagTagBox.Text,
            operatorBadge,
            plant,
            createdBy,
            CollectLines(),
            SimulateFailureCheck.IsChecked == true);

        if (!result.Success)
        {
            ErrorText.Text = result.Message;
            MessageBox.Show(
                result.Message,
                "Save failed — transaction rolled back",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        _savedThisSession = true;
        SuccessText.Text = result.Message;
        SuccessText.Visibility = Visibility.Visible;

        MessageBox.Show(
            $"{result.Message}\n\nVerify in SSMS:\n  SELECT * FROM dbo.OrderHeader WHERE OrderId = {result.OrderId};\n  SELECT * FROM dbo.OrderLine WHERE OrderId = {result.OrderId};",
            "Order saved",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = _savedThisSession;
        Close();
    }

    public sealed class BaggingLineRow
    {
        public BaggingLineRow(int lineNumber, string skuOrStyle, int quantity)
        {
            LineNumber = lineNumber;
            SkuOrStyle = skuOrStyle;
            Quantity = quantity;
        }

        public int LineNumber { get; set; }
        public string SkuOrStyle { get; set; }
        public int Quantity { get; set; }
    }
}
