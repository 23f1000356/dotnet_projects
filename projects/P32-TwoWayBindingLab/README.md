# P32 — TwoWay binding lab

**Stack:** WPF · Data binding · `INotifyPropertyChanged` · Code-behind `DataContext`  
**Prerequisites:** [P02](../../src/PracticeFA.App/) (WPF basics) · [P01](../P01-ClockInBoard/) helpful  
**Next:** [P33](../) (validation) · **P05** (MVVM toolkit)

---

## What is P32? (simple)

**P32 teaches data binding** — connect TextBoxes to a C# object so the UI and data stay in sync **without** manually reading `TextBox.Text` on every keystroke.

- **Model** raises `PropertyChanged` when a property changes
- **XAML** uses `{Binding PropertyName}`
- **Window** sets `DataContext = model`

No CommunityToolkit / Prism yet — manual pattern FA uses on older screens.

---

## The problem P32 solves

Without binding (P01 style):

```csharp
PreviewText.Text = NameTextBox.Text;  // on every TextChanged — tedious
```

With binding:

```xml
<TextBox Text="{Binding DisplayName, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>
<TextBlock Text="{Binding DisplayName}"/>
```

Type in the box → model updates → preview updates automatically.

---

## Web → WPF mapping

| React | P32 (WPF) |
|-------|-----------|
| `useState` + controlled `<input value={name} onChange=...>` | `INotifyPropertyChanged` + TwoWay `Binding` |
| Props / context | `DataContext` |
| Re-render on state change | `PropertyChanged` event |

---

## Folder structure

```text
projects/P32-TwoWayBindingLab/
  Models/EmployeeEditModel.cs   → INotifyPropertyChanged
  MainWindow.xaml               → Bindings + live preview
  MainWindow.xaml.cs            → DataContext, Save snapshot, Revert
  App.xaml
```

---

## `EmployeeEditModel`

| Property | Sample value |
|----------|----------------|
| `BadgeId` | E101 |
| `DisplayName` | Sara Chen |
| `Department` | CASTING |
| `Quantity` | 12 |

Each setter calls `SetProperty` → fires `PropertyChanged` → bound controls refresh.

| Method | Purpose |
|--------|---------|
| `Clone()` | Deep copy for snapshot |
| `CopyFrom(other)` | Revert — copy snapshot back into live model |

---

## Binding syntax (MainWindow.xaml)

```xml
Text="{Binding DisplayName, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
```

| Piece | Meaning |
|-------|---------|
| `Binding DisplayName` | Path on `DataContext` (`EmployeeEditModel`) |
| `Mode=TwoWay` | UI ↔ model (default for TextBox.Text is TwoWay) |
| `UpdateSourceTrigger=PropertyChanged` | Push to model **on each keystroke** (not only on LostFocus) |

Preview panel uses **one-way** binding (read model → UI):

```xml
<TextBlock Text="{Binding DisplayName}"/>
```

---

## Code-behind workflow

```text
Constructor
  → _model = new EmployeeEditModel()
  → _savedSnapshot = _model.Clone()
  → DataContext = _model

User types
  → TwoWay binding updates model property
  → PropertyChanged
  → Preview TextBlocks refresh

Save snapshot
  → _savedSnapshot = _model.Clone()

Revert
  → _model.CopyFrom(_savedSnapshot)
  → PropertyChanged on each property → form + preview reset
```

---

## Three concepts to memorize

| Concept | Role |
|---------|------|
| **DataContext** | The object bindings resolve against (`EmployeeEditModel`) |
| **Binding** | XAML link `{Binding PropertyName}` between UI and model |
| **INotifyPropertyChanged** | Model tells WPF “refresh anything bound to this property” |

---

## Floor Assistant mapping

| P32 | Floor Assistant |
|-----|-----------------|
| `INotifyPropertyChanged` | ViewModels, some code-behind models |
| `{Binding ...}` | Thousands of XAML lines in `Views/*` |
| `UpdateSourceTrigger=PropertyChanged` | Live search, live labels |
| Revert / snapshot | Cancel restores original row values |

**FA homework:** grep `{Binding` in one FA `.xaml` file.

---

## Run

```powershell
dotnet run --project projects/P32-TwoWayBindingLab/P32.App.csproj
```

---

## Acceptance checklist

- [ ] Typing in TextBox updates preview **without** clicking Save
- [ ] Explain `DataContext`, `Binding`, `INotifyPropertyChanged`
- [ ] Revert restores last **Save snapshot** values
- [ ] Save snapshot then edit then Revert — values go back

## Experiments

1. Remove `UpdateSourceTrigger=PropertyChanged` — preview updates only when leaving the field.  
2. Remove `PropertyChanged` from one setter — that field stops updating preview.  
3. Set `DataContext` in XAML instead of code-behind.

---

## What P32 does not cover

- MVVM commands (P05, P39)
- Validation / red borders (P33)
- `RelayCommand`, DI, ViewModel base classes

---

## Learning path

```text
P01 (manual TextBox) → P02 (shell) → P32 (binding) → P03/P04 → P33 (validation) → P05 (MVVM)
```
