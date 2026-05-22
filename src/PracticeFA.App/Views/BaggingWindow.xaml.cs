using System.Windows;
using PracticeFA.App.Models;

namespace PracticeFA.App.Views;

public partial class BaggingWindow : Window
{
    public BaggingWindow(int moduleId = ModuleIds.BaggingEntry)
    {
        ModuleId = moduleId;
        DataContext = new SearchHostContext("E205", "Bagging floor");
        InitializeComponent();
        HeaderText.Text = $"Bagging Entry (Module {ModuleId})";
        Title = HeaderText.Text;
    }

    public int ModuleId { get; }

    private void EmployeeSearch_SearchRequested(object sender, RoutedEventArgs e)
    {
        if (DataContext is SearchHostContext ctx)
        {
            SearchResultText.Text = string.IsNullOrWhiteSpace(ctx.SearchBadge)
                ? $"[{ctx.HostName}] Search: enter a badge."
                : $"[{ctx.HostName}] Loaded operator {ctx.SearchBadge} for bagging station.";
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
