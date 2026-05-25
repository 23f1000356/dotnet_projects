using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using PracticeFA.App.ViewModels;

namespace PracticeFA.App.Views;

/// <summary>P21 — LiveCharts dashboard page; ViewModel from DI.</summary>
public partial class OutputChartView : Page
{
    public OutputChartView(OutputChartViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += (_, _) =>
        {
            if (viewModel.RefreshCommand is IRelayCommand refresh)
                refresh.Execute(null);
        };
    }
}
