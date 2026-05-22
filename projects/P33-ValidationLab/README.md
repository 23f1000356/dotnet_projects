# P33 — Validation & ErrorTemplate lab

**Stack:** WPF · `INotifyDataErrorInfo` · `Validation.ErrorTemplate` · `Validation.GetHasError`  
**Prerequisites:** [P32](../P32-TwoWayBindingLab/)  
**Next:** [P34](../../src/PracticeFA.App/) (UserControl) · **P05** (MVVM)

---

## What is P33? (simple)

**P33 extends P32** with **form validation** — red borders, error messages, and **Save disabled** until fields are valid.

| Rule | Field |
|------|--------|
| Required, starts with `E`, min 4 chars | Badge ID |
| Required, whole number only | Quantity |
| Range 1–999 | Quantity |

---

## P32 vs P33

| | P32 | P33 |
|--|-----|-----|
| Binding | TwoWay + live preview | Same |
| Invalid data | Allowed | Errors shown |
| Save | Always enabled | Disabled when invalid |
| Model | `INotifyPropertyChanged` | + `INotifyDataErrorInfo` |
| UI | Plain TextBox | `Validation.ErrorTemplate` (red border) |

---

## Folder structure

```text
projects/P33-ValidationLab/
  Assets/ValidationStyles.xaml   → red ErrorTemplate for TextBoxes
  Models/EmployeeEditModel.cs    → validation rules in code
  MainWindow.xaml / .xaml.cs     → ValidatesOnNotifyDataErrors, Save gate
```

---

## `INotifyDataErrorInfo` (model)

`ValidateProperty` runs when Badge or Quantity changes:

- Empty badge → "Badge is required."
- `BAD` → must start with E, length ≥ 4
- `abc` in quantity → "Quantity must be a whole number."
- `0` or `1000` → "Quantity must be between 1 and 999."

`HasErrors` is true while any field has messages → Save button off.

---

## XAML binding flags

```xml
Text="{Binding BadgeId, UpdateSourceTrigger=PropertyChanged,
              ValidatesOnNotifyDataErrors=True, NotifyOnValidationError=True}"
```

| Flag | Role |
|------|------|
| `ValidatesOnNotifyDataErrors` | Ask model `GetErrors(property)` |
| `NotifyOnValidationError` | Fire events when error state changes |

---

## ErrorTemplate (`Assets/ValidationStyles.xaml`)

`ValidatedTextBoxStyle` — red **Border** around field + error **TextBlock** below.

Same idea as FA required-field highlighting before save.

---

## Save disabled

```csharp
SaveSnapshotButton.IsEnabled = !_model.HasErrors && !Validation.GetHasError(RootWindow);
```

- `_model.HasErrors` — data errors from `INotifyDataErrorInfo`
- `Validation.GetHasError` — any binding validation on the window

`SaveSnapshot_Click` double-checks before copying snapshot.

---

## Run

```powershell
dotnet run --project projects/P33-ValidationLab/P33.App.csproj
```

**Try:** clear badge → red error, Save grayed out → type `E101` → error clears, Save enabled.

---

## Acceptance checklist

- [ ] Empty badge cannot save
- [ ] Error clears when user fixes field
- [ ] Non-numeric quantity shows error
- [ ] Quantity outside 1–999 shows error

## FA homework

- [ ] Find `MessageBox` or validation on empty field in one FA View

---

## Learning path

```text
P32 (binding) → P33 (validation) → P05/P38 (MVVM) → real FA Views with save guards
```
