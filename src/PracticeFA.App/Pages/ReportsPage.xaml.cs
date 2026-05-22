using System.Windows;
using System.Windows.Controls;
using PracticeFA.App.Models;

namespace PracticeFA.App.Pages;

public partial class ReportsPage : Page
{
    public ReportsPage()
    {
        InitializeComponent();
        WipGrid.ItemsSource = new[]
        {
            new WipSummaryRow { WorkCenter = "CASTING", OpenOrders = 12, Status = "On track" },
            new WipSummaryRow { WorkCenter = "FSK", OpenOrders = 7, Status = "Delayed" },
            new WipSummaryRow { WorkCenter = "GRINDING", OpenOrders = 4, Status = "On track" },
        };
    }

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
