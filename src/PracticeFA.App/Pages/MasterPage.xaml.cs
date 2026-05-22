using System.Windows;
using System.Windows.Controls;
using PracticeFA.App.Models;
using PracticeFA.App.Services;
using PracticeFA.App.Views;

namespace PracticeFA.App.Pages;

public partial class MasterPage : Page
{
    public MasterPage()
    {
        InitializeComponent();
        DataContext = new SearchHostContext("E101", "Master hub");
        ApplyModuleAccess();
        ModulesListText.Text = string.IsNullOrWhiteSpace(AppState.ModuleListDisplay)
            ? "Your modules: (none — sign in again)"
            : $"Your modules: {AppState.ModuleListDisplay}";
    }

    private void ApplyModuleAccess()
    {
        SetModuleButton(StyleCreationButton, ModuleIds.StyleCreation);
        SetModuleButton(BaggingEntryButton, ModuleIds.BaggingEntry);
        SetModuleButton(MisProductivityButton, ModuleIds.MisProductivity);
        SetModuleButton(EmployeeMaintenanceButton, ModuleIds.EmployeeMaintenance);
    }

    private static void SetModuleButton(Button button, int moduleId) =>
        button.Visibility = ModuleAuth.CanAccess(moduleId)
            ? Visibility.Visible
            : Visibility.Collapsed;

    private void EmployeeSearch_SearchRequested(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SearchHostContext ctx)
            return;

        if (string.IsNullOrWhiteSpace(ctx.SearchBadge))
        {
            SearchResultText.Text = $"[{ctx.HostName}] Search: enter a badge.";
            return;
        }

        var employee = EmployeeService.GetByBadge(ctx.SearchBadge);
        SearchResultText.Text = employee is null
            ? $"[{ctx.HostName}] No employee for badge {ctx.SearchBadge}."
            : $"[{ctx.HostName}] Found {employee.DisplayName} · {employee.ProcessCenter ?? "—"} · Active={employee.IsActive}";
    }

    private void StyleCreation_Click(object sender, RoutedEventArgs e)
    {
        if (!TryOpenModule(ModuleIds.StyleCreation))
            return;
        OpenFeature(new StyleWindow(ModuleIds.StyleCreation));
    }

    private void BaggingEntry_Click(object sender, RoutedEventArgs e)
    {
        if (!TryOpenModule(ModuleIds.BaggingEntry))
            return;
        OpenFeature(new BaggingWindow(ModuleIds.BaggingEntry));
    }

    private void MisProductivity_Click(object sender, RoutedEventArgs e)
    {
        if (!TryOpenModule(ModuleIds.MisProductivity))
            return;
        OpenFeature(new MisWindow(ModuleIds.MisProductivity));
    }

    private void EmployeeMaintenance_Click(object sender, RoutedEventArgs e)
    {
        if (!TryOpenModule(ModuleIds.EmployeeMaintenance))
            return;

        var owner = Window.GetWindow(this);
        var list = new EmployeeListWindow();
        if (owner is not null)
            list.Owner = owner;

        list.ShowDialog();
        LastDialogText.Text = "Employee maintenance closed — grid saved via stored procedures.";
    }

    private bool TryOpenModule(int moduleId)
    {
        if (ModuleAuth.CanAccess(moduleId))
            return true;

        LastDialogText.Text = $"Module {moduleId} is not in your access list.";
        return false;
    }

    private void OpenFeature(Window featureWindow)
    {
        var owner = Window.GetWindow(this);
        if (owner is not null)
            featureWindow.Owner = owner;

        var saved = featureWindow.ShowDialog() == true;
        LastDialogText.Text = saved
            ? $"Last dialog: {featureWindow.Title} — Saved (DialogResult=true)"
            : $"Last dialog: {featureWindow.Title} — Cancelled (DialogResult=false)";
    }
}
