using System.Windows;
using System.Windows.Controls;
using PracticeFA.App.Models;
using PracticeFA.App.Views;

namespace PracticeFA.App.Pages;

public partial class MasterPage : Page
{
    public MasterPage() => InitializeComponent();

    private void StyleCreation_Click(object sender, RoutedEventArgs e) =>
        OpenFeature(new StyleWindow(ModuleIds.StyleCreation));

    private void BaggingEntry_Click(object sender, RoutedEventArgs e) =>
        OpenFeature(new BaggingWindow(ModuleIds.BaggingEntry));

    private void MisProductivity_Click(object sender, RoutedEventArgs e) =>
        OpenFeature(new MisWindow(ModuleIds.MisProductivity));

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
