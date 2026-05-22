using System.Windows;
using PracticeFA.P01.Models;

namespace PracticeFA.P01;

public partial class MainWindow : Window
{
    private readonly Dictionary<string, UserSession> _demoUsers = new(StringComparer.OrdinalIgnoreCase)
    {
        ["E001"] = new() { UserId = "E001", DisplayName = "Priya Sharma", PlantCode = "1001" },
        ["E002"] = new() { UserId = "E002", DisplayName = "Rahul Mehta", PlantCode = "1003" },
    };

    private readonly Dictionary<string, string> _employeeDirectory = new(StringComparer.OrdinalIgnoreCase)
    {
        ["E101"] = "Wax operator — Anil",
        ["E102"] = "Casting — Meena",
        ["E103"] = "FSK setting — Joel",
        ["E104"] = "QC — Sara",
    };

    private UserSession? _currentUser;
    private readonly List<FloorEmployee> _onFloor = new();

    public MainWindow()
    {
        InitializeComponent();
        LoginBadgeBox.Text = "E001";
        UpdateFloorCount();
    }

    private void SignIn_Click(object sender, RoutedEventArgs e)
    {
        var badge = LoginBadgeBox.Text.Trim();
        if (string.IsNullOrEmpty(badge))
        {
            MessageBox.Show(this, "Enter a badge ID.", "Sign in", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (!_demoUsers.TryGetValue(badge, out var session))
        {
            session = new UserSession
            {
                UserId = badge,
                DisplayName = $"Guest ({badge})",
                PlantCode = "1001",
            };
        }

        _currentUser = session;
        SessionText.Text = $"Signed in: {session.DisplayName}  |  Plant {session.PlantCode}";
        ClockInBadgeBox.IsEnabled = true;
        ClockInButton.IsEnabled = true;
        ClockInBadgeBox.Focus();
    }

    private void ClockIn_Click(object sender, RoutedEventArgs e)
    {
        if (_currentUser is null)
        {
            MessageBox.Show(this, "Sign in first.", "Clock in", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var badge = ClockInBadgeBox.Text.Trim();
        if (string.IsNullOrEmpty(badge))
        {
            MessageBox.Show(this, "Enter employee badge to clock in.", "Clock in",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (_onFloor.Any(x => x.BadgeId.Equals(badge, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show(this, $"{badge} is already on the floor list.", "Clock in",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var name = _employeeDirectory.TryGetValue(badge, out var display)
            ? display
            : $"Unknown badge {badge}";

        _onFloor.Add(new FloorEmployee
        {
            BadgeId = badge,
            DisplayName = name,
            ClockedInAt = DateTime.Now,
            ClockedInBy = _currentUser.UserId,
        });

        RefreshList();
        ClockInBadgeBox.Clear();
        ClockInBadgeBox.Focus();
    }

    private void ClearList_Click(object sender, RoutedEventArgs e)
    {
        if (_onFloor.Count == 0)
            return;

        var confirm = MessageBox.Show(this, "Clear everyone from the floor list?", "Clear list",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes)
            return;

        _onFloor.Clear();
        RefreshList();
    }

    private void RefreshList()
    {
        OnFloorList.ItemsSource = null;
        OnFloorList.ItemsSource = _onFloor.OrderBy(x => x.ClockedInAt).ToList();
        UpdateFloorCount();
    }

    private void UpdateFloorCount() =>
        FloorCountText.Text = $"{_onFloor.Count} on floor";
}
