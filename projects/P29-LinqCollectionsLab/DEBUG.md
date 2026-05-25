# Debug P29 — step by step

See main walkthrough in repo docs; breakpoints below match `Program.cs` line numbers.

| Step | Line | What to inspect |
|------|------|-----------------|
| A | 4 | `tickets.Count` == 12 |
| B | 17-18 | Step into `.Where` — watch `t` each row |
| C | 66 | `PrintRows` — `list.Count` after `.ToList()` |
| D | 24-27 | Query syntax `highQty` |
| E | 44-47 | `group.Key`, `group.Count()`, `totalQty` |

Keys: **F5** start · **F10** step over · **F11** step into · **Shift+F5** stop.
