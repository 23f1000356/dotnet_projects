using System.Windows.Controls;
using PracticeFA.App.ViewModels;

namespace PracticeFA.App.Views;

/// <summary>P45 — SAP-style async stock check; ViewModel from DI.</summary>
public partial class ErpStockView : Page
{
    public ErpStockView(ErpStockViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
