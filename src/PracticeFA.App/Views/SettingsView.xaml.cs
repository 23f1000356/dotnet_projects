using System.Windows.Controls;
using PracticeFA.App.ViewModels;

namespace PracticeFA.App.Views;

/// <summary>P05b + P39 — ViewModel injected from DI.</summary>
public partial class SettingsView : Page
{
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
