using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PracticeFA.P33.Models;

/// <summary>
/// P33 — P32 model plus INotifyDataErrorInfo for field validation.
/// </summary>
public sealed class EmployeeEditModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errors = new(StringComparer.Ordinal);

    private string _badgeId = "E101";
    private string _displayName = "Sara Chen";
    private string _department = "CASTING";
    private string _quantityText = "12";

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _errors.Values.Any(list => list.Count > 0);

    public string BadgeId
    {
        get => _badgeId;
        set
        {
            if (SetProperty(ref _badgeId, value))
                ValidateProperty(nameof(BadgeId));
        }
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

    /// <summary>String binding so non-numeric input shows validation (numeric only, 1–999).</summary>
    public string QuantityText
    {
        get => _quantityText;
        set
        {
            if (SetProperty(ref _quantityText, value))
                ValidateProperty(nameof(QuantityText));
        }
    }

    public EmployeeEditModel()
    {
        ValidateAll();
    }

    public EmployeeEditModel Clone() =>
        new()
        {
            BadgeId = BadgeId,
            DisplayName = DisplayName,
            Department = Department,
            QuantityText = QuantityText,
        };

    public void CopyFrom(EmployeeEditModel other)
    {
        _errors.Clear();
        BadgeId = other.BadgeId;
        DisplayName = other.DisplayName;
        Department = other.Department;
        QuantityText = other.QuantityText;
        ValidateAll();
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
            return _errors.SelectMany(pair => pair.Value);

        return _errors.TryGetValue(propertyName, out var messages)
            ? messages
            : Array.Empty<string>();
    }

    public void ValidateAll()
    {
        ValidateProperty(nameof(BadgeId));
        ValidateProperty(nameof(QuantityText));
    }

    private void ValidateProperty(string propertyName)
    {
        var messages = new List<string>();

        switch (propertyName)
        {
            case nameof(BadgeId):
                if (string.IsNullOrWhiteSpace(BadgeId))
                    messages.Add("Badge is required.");
                else
                {
                    if (!BadgeId.StartsWith('E') && !BadgeId.StartsWith('e'))
                        messages.Add("Badge must start with E.");
                    if (BadgeId.Trim().Length < 4)
                        messages.Add("Badge must be at least 4 characters.");
                }
                break;

            case nameof(QuantityText):
                if (string.IsNullOrWhiteSpace(QuantityText))
                    messages.Add("Quantity is required.");
                else if (!int.TryParse(QuantityText.Trim(), out var qty))
                    messages.Add("Quantity must be a whole number.");
                else if (qty < 1 || qty > 999)
                    messages.Add("Quantity must be between 1 and 999.");
                break;
        }

        if (messages.Count > 0)
            _errors[propertyName] = messages;
        else
            _errors.Remove(propertyName);

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
    }

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
