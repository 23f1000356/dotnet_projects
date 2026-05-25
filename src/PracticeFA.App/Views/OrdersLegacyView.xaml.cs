using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Services;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Views;

/// <summary>
/// P44 — master-detail like P40, but legacy FA style: code-behind + DataTable/DataView only (no ViewModel).
/// </summary>
public partial class OrdersLegacyView : Page
{
    private readonly IDataAccess _dataAccess;
    private DataTable? _headersTable;
    private DataTable? _linesTable;
    private int? _selectedOrderId;
    private int? _dbTotalQuantity;
    private bool _isBusy;

    public OrdersLegacyView(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
        InitializeComponent();
        StatusText.Text = "P44 legacy master-detail — select header or New order.";
        Loaded += async (_, _) => await RefreshHeadersAsync();
    }

    private void SetBusy(bool busy)
    {
        _isBusy = busy;
        RefreshButton.IsEnabled = !busy;
        NewOrderButton.IsEnabled = !busy;
        AddLineButton.IsEnabled = !busy;
        RemoveLineButton.IsEnabled = !busy;
        SaveOrderButton.IsEnabled = !busy;
        MasterList.IsEnabled = !busy;
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e) =>
        await RefreshHeadersAsync();

    private async Task RefreshHeadersAsync()
    {
        SetBusy(true);
        StatusText.Text = "Loading headers…";
        try
        {
            var table = await Task.Run(() => _dataAccess.ExecSp("dbo.spGetOrderHeaders"));
            AddDisplayLabelColumn(table);
            _headersTable = table;
            MasterList.ItemsSource = table.DefaultView;
            StatusText.Text = $"{table.Rows.Count} order(s) — dbo.spGetOrderHeaders (DataTable)";
        }
        catch (Exception ex)
        {
            MasterList.ItemsSource = null;
            StatusText.Text = "Failed to load headers.";
            MessageBox.Show(
                $"Could not load orders.\n\n{ex.Message}\n\nRun database/scripts/010_P40_OrdersRead.sql",
                "P44 Orders",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private static void AddDisplayLabelColumn(DataTable table)
    {
        if (!table.Columns.Contains("DisplayLabel"))
            table.Columns.Add("DisplayLabel", typeof(string));

        foreach (DataRow row in table.Rows)
        {
            var orderId = Convert.ToInt32(row["OrderId"]);
            var bagTag = Convert.ToString(row["BagTag"]) ?? "";
            var totalQty = table.Columns.Contains("TotalQuantity")
                ? Convert.ToInt32(row["TotalQuantity"])
                : 0;
            row["DisplayLabel"] = $"#{orderId} {bagTag} · {totalQty} pcs";
        }
    }

    private async void MasterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isBusy || MasterList.SelectedItem is not DataRowView rowView)
            return;

        _selectedOrderId = Convert.ToInt32(rowView["OrderId"]);
        _dbTotalQuantity = rowView.Row.Table.Columns.Contains("TotalQuantity")
            ? Convert.ToInt32(rowView["TotalQuantity"])
            : null;

        BagTagBox.Text = Convert.ToString(rowView["BagTag"]) ?? "";
        OperatorBadgeBox.Text = Convert.ToString(rowView["OperatorBadge"]) ?? "";
        PlantCodeBox.Text = Convert.ToString(rowView["PlantCode"]) ?? "";

        await LoadLinesAsync(_selectedOrderId.Value);
    }

    private async Task LoadLinesAsync(int orderId)
    {
        SetBusy(true);
        try
        {
            var table = await Task.Run(() => _dataAccess.ExecSp(
                "dbo.spGetOrderLines",
                new SqlParameter("@OrderId", orderId)));

            _linesTable = table;
            LinesGrid.ItemsSource = table.DefaultView;
            UpdateDetailTotal();
            StatusText.Text = $"Loaded {table.Rows.Count} line(s) for order #{orderId} (DataView)";
        }
        catch (Exception ex)
        {
            LinesGrid.ItemsSource = null;
            StatusText.Text = "Failed to load lines.";
            MessageBox.Show(
                $"Could not load order lines.\n\n{ex.Message}",
                "P44 Orders",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void NewOrder_Click(object sender, RoutedEventArgs e)
    {
        MasterList.SelectedItem = null;
        _selectedOrderId = null;
        _dbTotalQuantity = null;

        BagTagBox.Text = $"BAG-{DateTime.Now:HHmmss}";
        OperatorBadgeBox.Text = "E101";
        PlantCodeBox.Text = AppState.CurrentUser?.PlantCode ?? "P01";

        _linesTable = CreateDraftLinesTable();
        LinesGrid.ItemsSource = _linesTable.DefaultView;
        UpdateDetailTotal();
        StatusText.Text = "New order draft — edit header + lines, then Save (dbo.spSaveOrder).";
    }

    private static DataTable CreateDraftLinesTable()
    {
        var table = new DataTable();
        table.Columns.Add("LineNumber", typeof(int));
        table.Columns.Add("SkuOrStyle", typeof(string));
        table.Columns.Add("Quantity", typeof(int));
        table.Rows.Add(1, "STYLE-A", 10);
        return table;
    }

    private void AddLine_Click(object sender, RoutedEventArgs e)
    {
        _linesTable ??= CreateDraftLinesTable();
        var next = 1;
        foreach (DataRow row in _linesTable.Rows)
        {
            if (row.RowState == DataRowState.Deleted)
                continue;
            next = Math.Max(next, Convert.ToInt32(row["LineNumber"]) + 1);
        }
        _linesTable.Rows.Add(next, "", 1);
        LinesGrid.ItemsSource = _linesTable.DefaultView;
        UpdateDetailTotal();
    }

    private void RemoveLine_Click(object sender, RoutedEventArgs e)
    {
        if (_linesTable is null || _linesTable.Rows.Count == 0)
            return;

        _linesTable.Rows.RemoveAt(_linesTable.Rows.Count - 1);
        UpdateDetailTotal();
    }

    private async void SaveOrder_Click(object sender, RoutedEventArgs e)
    {
        if (_linesTable is null)
        {
            StatusText.Text = "No lines to save.";
            return;
        }

        var bagTag = BagTagBox.Text;
        var operatorBadge = OperatorBadgeBox.Text;
        var plantCode = PlantCodeBox.Text;
        var validation = ValidateForSave(bagTag, operatorBadge, plantCode, _linesTable);
        if (!validation.Success)
        {
            StatusText.Text = validation.Message;
            return;
        }

        SetBusy(true);
        StatusText.Text = "Saving order…";
        try
        {
            var createdBy = AppState.CurrentUser?.UserId;
            var linesParam = CreateLinesParameter(_linesTable);
            var resultTable = await Task.Run(() => _dataAccess.ExecSp(
                "dbo.spSaveOrder",
                new SqlParameter("@BagTag", bagTag.Trim()),
                new SqlParameter("@OperatorBadge", operatorBadge.Trim()),
                new SqlParameter("@PlantCode", plantCode.Trim()),
                new SqlParameter("@CreatedBy", (object?)createdBy?.Trim() ?? DBNull.Value),
                linesParam,
                new SqlParameter("@SimulateLine2Failure", false)));

            if (resultTable.Rows.Count == 0)
            {
                StatusText.Text = "Order was not saved.";
                return;
            }

            var orderId = Convert.ToInt32(resultTable.Rows[0]["OrderId"]);
            StatusText.Text = $"Order {orderId} saved — header + {_linesTable.Rows.Count} line(s) committed.";
            await RefreshHeadersAsync();
            SelectHeaderByOrderId(orderId);
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            StatusText.Text = "dbo.spSaveOrder not found. Run 006_P36_Orders.sql";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Save failed: {ex.Message}";
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SelectHeaderByOrderId(int orderId)
    {
        if (_headersTable is null)
            return;

        foreach (DataRowView rowView in _headersTable.DefaultView)
        {
            if (Convert.ToInt32(rowView["OrderId"]) != orderId)
                continue;

            MasterList.SelectedItem = rowView;
            break;
        }
    }

    private void UpdateDetailTotal()
    {
        if (_linesTable is null)
        {
            DetailTotalText.Text = "Draft total: 0 pcs";
            return;
        }

        var total = 0;
        foreach (DataRow row in _linesTable.Rows)
        {
            if (row.RowState == DataRowState.Deleted)
                continue;
            total += row["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(row["Quantity"]);
        }

        DetailTotalText.Text = _selectedOrderId is null
            ? $"Draft total: {total} pcs"
            : $"Order #{_selectedOrderId} detail total: {total} pcs (DB had {_dbTotalQuantity ?? 0})";
    }

    private static SqlParameter CreateLinesParameter(DataTable linesTable)
    {
        var tvp = new DataTable();
        tvp.Columns.Add("LineNumber", typeof(int));
        tvp.Columns.Add("SkuOrStyle", typeof(string));
        tvp.Columns.Add("Quantity", typeof(int));

        foreach (DataRow row in linesTable.Rows)
        {
            if (row.RowState == DataRowState.Deleted)
                continue;

            tvp.Rows.Add(
                Convert.ToInt32(row["LineNumber"]),
                Convert.ToString(row["SkuOrStyle"])?.Trim() ?? "",
                Convert.ToInt32(row["Quantity"]));
        }

        return new SqlParameter("@Lines", SqlDbType.Structured)
        {
            TypeName = "dbo.OrderLineInput",
            Value = tvp,
        };
    }

    private static (bool Success, string Message) ValidateForSave(
        string bagTag,
        string operatorBadge,
        string plantCode,
        DataTable lines)
    {
        if (string.IsNullOrWhiteSpace(bagTag))
            return (false, "PO / bag tag is required.");
        if (string.IsNullOrWhiteSpace(operatorBadge))
            return (false, "Operator badge is required.");
        if (string.IsNullOrWhiteSpace(plantCode))
            return (false, "Plant code is required.");

        var activeRows = new List<DataRow>();
        foreach (DataRow row in lines.Rows)
        {
            if (row.RowState != DataRowState.Deleted)
                activeRows.Add(row);
        }
        if (activeRows.Count == 0)
            return (false, "Add at least one line.");

        foreach (var row in activeRows)
        {
            var lineNo = Convert.ToInt32(row["LineNumber"]);
            var sku = Convert.ToString(row["SkuOrStyle"]) ?? "";
            var qty = Convert.ToInt32(row["Quantity"]);

            if (lineNo <= 0)
                return (false, "Line numbers must be 1, 2, 3, …");
            if (string.IsNullOrWhiteSpace(sku))
                return (false, $"Line {lineNo}: SKU / style is required.");
            if (qty <= 0)
                return (false, $"Line {lineNo}: quantity must be greater than zero.");
        }

        var numbers = activeRows.Select(r => Convert.ToInt32(r["LineNumber"])).ToList();
        if (numbers.Distinct().Count() != numbers.Count)
            return (false, "Duplicate line numbers are not allowed.");

        return (true, "OK");
    }
}
