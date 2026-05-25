# Debug P00 — step by step

## Best breakpoints (first time)

| File | Line (approx) | Why |
|------|----------------|-----|
| `Program.cs` | `TryBuildTicket(...)` call | Pause before validation |
| `Program.cs` | `RoutingTable.Find(wcCode)` | See if work center is found |
| `Program.cs` | `new ProductionTicket { ... }` | See final object |
| `RoutingTable.cs` | inside `Find` | Watch list search |

---

## Visual Studio

1. Open `PracticeFA.slnx` from repo root.
2. In **Solution Explorer**, right-click **P00.Console** → **Set as Startup Project**.
3. Open `Program.cs`.
4. Click left margin on line 14 (`TryBuildTicket`) — red dot = breakpoint.
5. Press **F5** (Start Debugging).
6. Terminal opens; enter PO, work center, qty.
7. When breakpoint hits:
   - **F10** — Step Over (next line, don’t go into methods)
   - **F11** — Step Into (go inside `TryBuildTicket` / `Find`)
   - **Shift+F11** — Step Out (finish current method)
8. **Locals** window — see `po`, `wcCode`, `qty`, `center`, `ticket`.
9. Hover variables in code to see values.
10. **Shift+F5** — Stop debugging.

### If the window closes too fast

- You stopped on last line without `ReadLine` — use breakpoint earlier, or **Ctrl+F5** only when not debugging end.
- Keep breakpoint before `return 0`.

---

## Cursor / VS Code

1. Install **C# Dev Kit** or **C#** extension (Microsoft).
2. Open folder `Sample_dotnet` as workspace.
3. **Run and Debug** (Ctrl+Shift+D) → choose **P00 — Console floor ticket**.
4. Set breakpoint in `Program.cs` (click left of line number).
5. **F5** — builds and runs in integrated terminal.
6. Same **F10 / F11** keys as Visual Studio.

`launch.json` is in `.vscode/` at repo root.

---

## Debug without IDE (console only)

Not true debugging, but useful:

```powershell
dotnet run --project projects/P00-ConsoleFloorTicket/P00.Console.csproj
```

Add temporary lines in code:

```csharp
Console.WriteLine($"DEBUG: wcCode=[{wcCode}] center={center?.Code}");
```

Remove before commit.

---

## Sample debug session

1. Breakpoint on line 14 `TryBuildTicket`.
2. Run **F5**, enter:
   - PO: `PO-TEST-1`
   - WC: `FSK`
   - Qty: `3`
3. **F11** into `TryBuildTicket` → **F11** into `Find` → watch `center` = FSK, `NextCode` = POL.
4. **F10** until `ticket` is created — inspect `ticket.Quantity` == 3.
5. **F5** Continue — see printed line: `Next: POL`.

Try again with WC `BAD` — `center` should stay **null**, error path.
