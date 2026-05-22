using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PracticeFA.P32.Models;

/// <summary>
/// Edit model for P32 — manual INotifyPropertyChanged (no MVVM toolkit).
/// </summary>
public sealed class EmployeeEditModel : INotifyPropertyChanged
{
    private string _badgeId = "E101";
    private string _displayName = "Sara Chen";
    private string _department = "CASTING";
    private int _quantity = 12;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string BadgeId
    {
        get => _badgeId;
        set => SetProperty(ref _badgeId, value);
    }

    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    public string Department
    {
        get => _department;
        set => SetProperty(ref _department, value);
    }

    public int Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }

    public EmployeeEditModel Clone() =>
        new()
        {
            BadgeId = BadgeId,
            DisplayName = DisplayName,
            Department = Department,
            Quantity = Quantity,
        };

    public void CopyFrom(EmployeeEditModel other)
    {
        BadgeId = other.BadgeId;
        DisplayName = other.DisplayName;
        Department = other.Department;
        Quantity = other.Quantity;
    }

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
