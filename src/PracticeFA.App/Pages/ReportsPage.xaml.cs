using System.Windows;
using System.Windows.Controls;

namespace PracticeFA.App.Pages;

public partial class ReportsPage : Page
{
    public ReportsPage() => InitializeComponent();

    private void Placeholder_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            MessageBox.Show(
                $"P13 MIS lite will load: {btn.Tag}",
                "Reports",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
