using System.Windows.Controls;
using PracticeFA.App.ViewModels;

namespace PracticeFA.App.Views;

/// <summary>P42 — TabControl wizard; ViewModel from DI.</summary>
public partial class OrderWizardView : Page
{
    public OrderWizardView(OrderWizardViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
