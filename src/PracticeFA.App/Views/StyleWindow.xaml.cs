using System.Windows;
using System.Windows.Controls;
using PracticeFA.App.Models;
using PracticeFA.App.Services;

namespace PracticeFA.App.Views;

public partial class StyleWindow : Window
{
    private bool _savedThisSession;

    public StyleWindow(int moduleId = ModuleIds.StyleCreation)
    {
        ModuleId = moduleId;
        InitializeComponent();
        ModuleBadgeText.Text = ModuleId.ToString();
        HeaderText.Text = "Style Creation";
        Title = $"Style Creation ({ModuleId})";
        Loaded += (_, _) => LoadStylesGrid();
    }

    public int ModuleId { get; }

    private void LoadStylesGrid()
    {
        try
        {
            var table = StyleService.GetStyles();
            var rows = new List<StyleGridRow>();
            foreach (System.Data.DataRow row in table.Rows)
            {
                var style = StyleMapper.FromRow(row);
                rows.Add(new StyleGridRow(style.StyleCode, style.Description));
            }

            StylesGrid.ItemsSource = rows;
            GridStatusText.Text = $"{rows.Count} style(s) — dbo.spGetStyles";
        }
        catch (Exception ex)
        {
            StylesGrid.ItemsSource = null;
            GridStatusText.Text = "Could not load styles. Run database/scripts/004_P04_Styles.sql";
            MessageBox.Show(
                $"Could not load styles.\n\n{ex.Message}\n\nRun database/scripts/004_P04_Styles.sql",
                "Style Creation",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void StylesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (StylesGrid.SelectedItem is not StyleGridRow row)
            return;

        StyleCodeBox.Text = row.StyleCode;
        DescriptionBox.Text = row.Description;
        ErrorText.Text = "";
        SavedPanel.Visibility = Visibility.Collapsed;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Text = "";

        var createdBy = AppState.CurrentUser?.UserId;
        var result = StyleService.Save(StyleCodeBox.Text, DescriptionBox.Text, createdBy);

        if (!result.Success)
        {
            ErrorText.Text = result.Message;
            return;
        }

        var style = result.Style!;
        SavedCodeText.Text = $"Code: {style.StyleCode}";
        SavedDescriptionText.Text = style.Description;
        SavedPanel.Visibility = Visibility.Visible;

        _savedThisSession = true;
        LoadStylesGrid();

        MessageBox.Show(
            $"{result.Message}\n\nCode: {style.StyleCode}\nDescription: {style.Description}",
            "Style saved",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = _savedThisSession;
        Close();
    }

    private sealed record StyleGridRow(string StyleCode, string Description);
}
