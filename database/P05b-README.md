# P05b — Settings screen (JSON)

**Prerequisites:** P05 (MVVM pattern)  
**No SQL** — file: `%AppData%\PracticeFA\settings.json`

## Open

**File → Settings (P05b)** or sidebar **Settings (P05b)**

## Test persistence

1. Change **Preferred plant code** to `P02` → **Save to JSON**
2. Exit app completely
3. Restart → sign in → open Settings → plant should still be **P02**
4. Status bar shows `Prefs plant P02`

## Test invalid JSON

1. Close app
2. Edit `settings.json` in Notepad — break JSON (delete a brace)
3. Start app → should start with defaults and a warning (no crash)
4. Open Settings → **Reload from disk** shows warning message

## Fields

| Field | Example |
|-------|---------|
| PlantCode | P01, P02 |
| DefaultPrinter | Zebra-Floor-1 |
| Theme | Light / Dark |
