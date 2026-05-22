using System.Windows;
using System.Windows.Controls;
using PracticeFA.P33.Models;

namespace PracticeFA.P33;

public partial class MainWindow : Window
{
    private readonly EmployeeEditModel _model = new();
    private EmployeeEditModel _savedSnapshot;

    public MainWindow()
    {
        InitializeComponent();
        _savedSnapshot = _model.Clone();
        DataContext = _model;

        _model.ErrorsChanged += (_, _) => UpdateSaveState();
        _model.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EmployeeEditModel.HasErrors))
                UpdateSaveState();
        };

        Validation.AddErrorHandler(RootWindow, (_, _) => UpdateSaveState());

        UpdateSaveState();
        StatusText.Text = "Clear badge or enter bad quantity — Save stays disabled until valid.";
    }

    private void UpdateSaveState()
    {
        var bindingErrors = Validation.GetHasError(RootWindow);
        SaveSnapshotButton.IsEnabled = !_model.HasErrors && !bindingErrors;
    }

    private void SaveSnapshot_Click(object sender, RoutedEventArgs e)
    {
        if (_model.HasErrors || Validation.GetHasError(RootWindow))
        {
            StatusText.Text = "Cannot save — fix validation errors first.";
            return;
        }

        _savedSnapshot = _model.Clone();
        StatusText.Text = $"Snapshot saved at {DateTime.Now:HH:mm:ss}.";
    }

    private void Revert_Click(object sender, RoutedEventArgs e)
    {
        _model.CopyFrom(_savedSnapshot);
        StatusText.Text = "Reverted to last saved snapshot.";
        UpdateSaveState();
    }
}
