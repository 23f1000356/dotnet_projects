using System.Windows;
using PracticeFA.App.Pages;

namespace PracticeFA.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NavigateToMaster();
    }

    private void Master_Click(object sender, RoutedEventArgs e) => NavigateToMaster();

    private void Reports_Click(object sender, RoutedEventArgs e) => NavigateToReports();

    private void Exit_Click(object sender, RoutedEventArgs e) => Close();

    private void NavigateToMaster()
    {
        MainFrame.Navigate(new MasterPage());
        NavHintText.Text = "Current: Master (product data hub)";
    }

    private void NavigateToReports()
    {
        MainFrame.Navigate(new ReportsPage());
        NavHintText.Text = "Current: Reports (MIS hub)";
    }
}
