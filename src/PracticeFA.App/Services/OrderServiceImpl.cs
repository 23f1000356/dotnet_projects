using System.Data;
using Microsoft.Data.SqlClient;
using PracticeFA.App.Models;
using PracticeFA.App.Services.Abstractions;

namespace PracticeFA.App.Services;

/// <summary>P36/P39/P40 — orders via IDataAccess.</summary>
public sealed class OrderServiceImpl : IOrderService
{
    private readonly IDataAccess _dataAccess;

    public OrderServiceImpl(IDataAccess dataAccess) => _dataAccess = dataAccess;

    public IReadOnlyList<OrderHeaderSummary> GetHeaders()
    {
        try
        {
            var table = _dataAccess.ExecSp("dbo.spGetOrderHeaders");
            return OrderMapper.HeadersFromTable(table);
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            throw new InvalidOperationException(
                "dbo.spGetOrderHeaders not found. Run database/scripts/010_P40_OrdersRead.sql",
                ex);
        }
    }

    public IReadOnlyList<OrderLineRecord> GetLines(int orderId)
    {
        try
        {
            var table = _dataAccess.ExecSp(
                "dbo.spGetOrderLines",
                new SqlParameter("@OrderId", orderId));
            return OrderMapper.LinesFromTable(table);
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            throw new InvalidOperationException(
                "dbo.spGetOrderLines not found. Run database/scripts/010_P40_OrdersRead.sql",
                ex);
        }
    }

    public OrderSaveResult Save(
        string bagTag,
        string operatorBadge,
        string plantCode,
        string? createdBy,
        IReadOnlyList<OrderLineInput> lines,
        bool simulateLine2Failure = false)
    {
        var validation = Validate(bagTag, operatorBadge, plantCode, lines);
        if (!validation.Success)
            return validation;

        try
        {
            var table = _dataAccess.ExecSp(
                "dbo.spSaveOrder",
                new SqlParameter("@BagTag", bagTag.Trim()),
                new SqlParameter("@OperatorBadge", operatorBadge.Trim()),
                new SqlParameter("@PlantCode", plantCode.Trim()),
                new SqlParameter("@CreatedBy", (object?)createdBy?.Trim() ?? DBNull.Value),
                CreateLinesParameter(lines),
                new SqlParameter("@SimulateLine2Failure", simulateLine2Failure));

            if (table.Rows.Count == 0)
                return OrderSaveResult.Fail("Order was not saved.");

            var orderId = Convert.ToInt32(table.Rows[0]["OrderId"]);
            return OrderSaveResult.Ok(
                $"Order {orderId} saved — header + {lines.Count} line(s) committed.",
                orderId);
        }
        catch (SqlException ex) when (ex.Number == 2812)
        {
            return OrderSaveResult.Fail(
                "dbo.spSaveOrder not found. Run database/scripts/006_P36_Orders.sql");
        }
        catch (SqlException ex)
        {
            return OrderSaveResult.Fail(ex.Message);
        }
    }

    private static SqlParameter CreateLinesParameter(IReadOnlyList<OrderLineInput> lines)
    {
        var table = new DataTable();
        table.Columns.Add("LineNumber", typeof(int));
        table.Columns.Add("SkuOrStyle", typeof(string));
        table.Columns.Add("Quantity", typeof(int));

        foreach (var line in lines.OrderBy(l => l.LineNumber))
            table.Rows.Add(line.LineNumber, line.SkuOrStyle.Trim(), line.Quantity);

        return new SqlParameter("@Lines", SqlDbType.Structured)
        {
            TypeName = "dbo.OrderLineInput",
            Value = table,
        };
    }

    private static OrderSaveResult Validate(
        string bagTag,
        string operatorBadge,
        string plantCode,
        IReadOnlyList<OrderLineInput> lines)
    {
        if (string.IsNullOrWhiteSpace(bagTag))
            return OrderSaveResult.Fail("PO / bag tag is required.");
        if (string.IsNullOrWhiteSpace(operatorBadge))
            return OrderSaveResult.Fail("Operator badge is required.");
        if (string.IsNullOrWhiteSpace(plantCode))
            return OrderSaveResult.Fail("Plant code is required.");
        if (lines.Count == 0)
            return OrderSaveResult.Fail("Add at least one line.");

        foreach (var line in lines)
        {
            if (line.LineNumber <= 0)
                return OrderSaveResult.Fail("Line numbers must be 1, 2, 3, …");
            if (string.IsNullOrWhiteSpace(line.SkuOrStyle))
                return OrderSaveResult.Fail($"Line {line.LineNumber}: SKU / style is required.");
            if (line.Quantity <= 0)
                return OrderSaveResult.Fail($"Line {line.LineNumber}: quantity must be greater than zero.");
        }

        if (lines.Select(l => l.LineNumber).Distinct().Count() != lines.Count)
            return OrderSaveResult.Fail("Duplicate line numbers are not allowed.");

        return OrderSaveResult.Ok();
    }
}
