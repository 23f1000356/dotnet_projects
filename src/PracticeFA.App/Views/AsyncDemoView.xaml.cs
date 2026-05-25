using System.Windows.Controls;
using PracticeFA.App.ViewModels;

namespace PracticeFA.App.Views;

/// <summary>P20 — async slow load demo; ViewModel from DI.</summary>
public partial class AsyncDemoView : Page
{
    public AsyncDemoView(AsyncDemoViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
