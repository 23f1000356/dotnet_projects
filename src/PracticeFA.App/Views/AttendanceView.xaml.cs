using System.Windows.Controls;
using PracticeFA.App.ViewModels;

namespace PracticeFA.App.Views;

/// <summary>P05 + P39 — ViewModel injected; no new AttendanceViewModel() here.</summary>
public partial class AttendanceView : Page
{
    public AttendanceView(AttendanceViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
