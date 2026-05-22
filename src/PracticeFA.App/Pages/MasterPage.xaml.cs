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
    }

    private static void SetModuleButton(Button button, int moduleId) =>
        button.Visibility = ModuleAuth.CanAccess(moduleId)
            ? Visibility.Visible
            : Visibility.Collapsed;

    private void EmployeeSearch_SearchRequested(object sender, RoutedEventArgs e)
    {
        if (DataContext is SearchHostContext ctx)
        {
            SearchResultText.Text = string.IsNullOrWhiteSpace(ctx.SearchBadge)
                ? $"[{ctx.HostName}] Search: enter a badge."
                : $"[{ctx.HostName}] Search badge {ctx.SearchBadge} — P08 will call spGetEmployeeByBadge.";
        }
    }

    private void StyleCreation_Click(object sender, RoutedEventArgs e)
    {
        if (!TryOpenModule(ModuleIds.StyleCreation, out var denied))
            return;
        OpenFeature(new StyleWindow(ModuleIds.StyleCreation));
    }

    private void BaggingEntry_Click(object sender, RoutedEventArgs e)
    {
        if (!TryOpenModule(ModuleIds.BaggingEntry, out var denied))
            return;
        OpenFeature(new BaggingWindow(ModuleIds.BaggingEntry));
    }

    private void MisProductivity_Click(object sender, RoutedEventArgs e)
    {
        if (!TryOpenModule(ModuleIds.MisProductivity, out var denied))
            return;
        OpenFeature(new MisWindow(ModuleIds.MisProductivity));
    }

    private bool TryOpenModule(int moduleId, out string? deniedMessage)
    {
        if (ModuleAuth.CanAccess(moduleId))
        {
            deniedMessage = null;
            return true;
        }

        deniedMessage = $"Module {moduleId} is not in your access list.";
        LastDialogText.Text = deniedMessage;
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
