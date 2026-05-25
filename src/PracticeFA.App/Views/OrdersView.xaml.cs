using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using PracticeFA.App.ViewModels;

namespace PracticeFA.App.Views;

/// <summary>P40 — master-detail; ViewModel from DI (P39).</summary>
public partial class OrdersView : Page
{
    public OrdersView(OrdersViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += async (_, _) =>
        {
            if (viewModel.RefreshHeadersCommand is IAsyncRelayCommand refresh)
                await refresh.ExecuteAsync(null);
        };
    }
}
