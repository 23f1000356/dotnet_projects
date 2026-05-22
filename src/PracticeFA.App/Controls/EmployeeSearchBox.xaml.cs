using System.Windows;
using System.Windows.Controls;

namespace PracticeFA.App.Controls;

public partial class EmployeeSearchBox : UserControl
{
    public static readonly DependencyProperty BadgeTextProperty =
        DependencyProperty.Register(
            nameof(BadgeText),
            typeof(string),
            typeof(EmployeeSearchBox),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly RoutedEvent SearchRequestedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(SearchRequested),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(EmployeeSearchBox));

    public EmployeeSearchBox() => InitializeComponent();

    public string BadgeText
    {
        get => (string)GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }

    public event RoutedEventHandler SearchRequested
    {
        add => AddHandler(SearchRequestedEvent, value);
        remove => RemoveHandler(SearchRequestedEvent, value);
    }

    private void Search_Click(object sender, RoutedEventArgs e) =>
        RaiseEvent(new RoutedEventArgs(SearchRequestedEvent, this));
}
