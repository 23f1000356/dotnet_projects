using System.Windows;
using PracticeFA.P32.Models;

namespace PracticeFA.P32;

public partial class MainWindow : Window
{
    private readonly EmployeeEditModel _model = new();
    private EmployeeEditModel _savedSnapshot;

    public MainWindow()
    {
        InitializeComponent();
        _savedSnapshot = _model.Clone();
        DataContext = _model;
        StatusText.Text = "Ready — edit fields or Revert to last saved snapshot.";
    }

    private void SaveSnapshot_Click(object sender, RoutedEventArgs e)
    {
        _savedSnapshot = _model.Clone();
        StatusText.Text = $"Snapshot saved at {DateTime.Now:HH:mm:ss} — Revert will restore these values.";
    }

    private void Revert_Click(object sender, RoutedEventArgs e)
    {
        _model.CopyFrom(_savedSnapshot);
        StatusText.Text = "Reverted to last saved snapshot.";
    }
}
