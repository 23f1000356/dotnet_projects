using System.Windows;
using PracticeFA.App.Models;

namespace PracticeFA.App.Views;

public partial class StyleWindow : Window
{
    public StyleWindow(int moduleId = ModuleIds.StyleCreation)
    {
        ModuleId = moduleId;
        InitializeComponent();
        HeaderText.Text = $"Style Creation (Module {ModuleId})";
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
