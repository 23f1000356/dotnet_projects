using System.Windows;
using System.Windows.Controls;

namespace PracticeFA.App.Controls;

/// <summary>P20 — semi-transparent overlay + indeterminate progress; blocks input while IsBusy is true.</summary>
public partial class BusyOverlay : UserControl
{
    public static readonly DependencyProperty IsBusyProperty =
        DependencyProperty.Register(
            nameof(IsBusy),
            typeof(bool),
            typeof(BusyOverlay),
            new PropertyMetadata(false, OnBusyChanged));

    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register(
            nameof(Message),
            typeof(string),
            typeof(BusyOverlay),
            new PropertyMetadata("Please wait…", OnMessageChanged));

    public BusyOverlay()
    {
        InitializeComponent();
        UpdateMessage();
    }

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    private static void OnBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BusyOverlay overlay)
            overlay.UpdateVisibility();
    }

    private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BusyOverlay overlay)
            overlay.UpdateMessage();
    }

    private void UpdateVisibility()
    {
        OverlayPanel.Visibility = IsBusy ? Visibility.Visible : Visibility.Collapsed;
        IsHitTestVisible = IsBusy;
    }

    private void UpdateMessage() => MessageText.Text = Message;
}
