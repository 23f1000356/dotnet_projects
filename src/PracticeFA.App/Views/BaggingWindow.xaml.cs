using System.Windows;
using PracticeFA.App.Models;

namespace PracticeFA.App.Views;

public partial class BaggingWindow : Window
{
    public BaggingWindow(int moduleId = ModuleIds.BaggingEntry)
    {
        ModuleId = moduleId;
        InitializeComponent();
        HeaderText.Text = $"Bagging Entry (Module {ModuleId})";
        Title = HeaderText.Text;
    }

    public int ModuleId { get; }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
