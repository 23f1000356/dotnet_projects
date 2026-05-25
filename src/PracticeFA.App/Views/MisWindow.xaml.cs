using System.Windows;
using PracticeFA.App.Models;

namespace PracticeFA.App.Views;

public partial class MisWindow : Window
{
    public MisWindow(int moduleId = ModuleIds.MisProductivity)
    {
        ModuleId = moduleId;
        InitializeComponent();
        ModuleBadgeText.Text = ModuleId.ToString();
        HeaderText.Text = "MIS Productivity";
        Title = $"MIS Productivity ({ModuleId})";
        ShiftDatePicker.SelectedDate = DateTime.Today;
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
