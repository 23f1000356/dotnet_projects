# P45 — Async SAP-style mock

**Prerequisites:** P20 (busy overlay), P39 DI  
**No SQL** — mock SAP RFC only

## Open

**View → SAP stock check (P45)** or sidebar **SAP stock (P45)**

## What it teaches

| Pattern | Practice FA | Floor Assistant |
|---------|-------------|-----------------|
| ERP facade | `IErpService` | `SAPRFCHandler` / `PULLFromSAP` |
| Long call off UI thread | `Task.Run` + `async` | Same — never block WPF |
| Status ticks | `IProgress<string>` | FA status bar / progress text |
| Operator cancel | `CancellationTokenSource.Cancel()` | Stop hung RFC wait |

## Flow

1. Enter **SKU** (e.g. `STYLE-A`)
2. **Verify stock (~5s)** — mock reports 5 progress steps (~1s each)
3. While running: UI responsive; **Cancel** calls `CTS.Cancel()`
4. On success: grid shows mock stock by plant
5. On cancel: `OperationCanceledException` → status “cancelled”

## Code

| File | Role |
|------|------|
| `IErpService` | Contract — ViewModel never references SAP DLL |
| `MockErpService` | 5s delay + progress + mock lines |
| `ErpStockViewModel` | `VerifyStockCommand`, `CancelCommand`, `CTS` |
| `ErpStockView` | P20 `BusyOverlay` |

## Compare P20 vs P45

| | P20 | P45 |
|--|-----|-----|
| Work | SQL `spGetOrderHeaders` | Mock SAP RFC |
| Progress | Busy message only | `IProgress<string>` step text |
| Cancel | No | **Cancel** button |

## FA homework

Grep FA QA for `PULLFromSAP` or `CancellationToken` — compare to this screen.

## P14 note

Same `IErpService` interface — P14 can add SQL/JSON implementations later; P45 uses `MockErpService`.
