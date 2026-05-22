using System.Windows;
using PracticeFA.App.Models;

namespace PracticeFA.App.Views;

public partial class MisWindow : Window
{
    public MisWindow(int moduleId = ModuleIds.MisProductivity)
    {
        ModuleId = moduleId;
        InitializeComponent();
        HeaderText.Text = $"MIS Productivity (Module {ModuleId})";
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
