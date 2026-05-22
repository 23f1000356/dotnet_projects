using System.Windows;
using System.Windows.Controls;

namespace PracticeFA.App.Pages;

public partial class MasterPage : Page
{
    public MasterPage() => InitializeComponent();

    private void Placeholder_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            MessageBox.Show(
                $"P04 will open a View window for: {btn.Tag}",
                "Master module",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
