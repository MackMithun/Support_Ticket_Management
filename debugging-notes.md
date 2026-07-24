# Debugging Notes

## Issue 1 — WeatherForecast scaffold mismatch

### Problem
The initial `dotnet new webapi` template still exposed `/weatherforecast` and had no ticket domain wiring.

### How I investigated
Ran `dotnet run` and hit the default endpoint. Checked `Program.cs` for service registrations.

### Final fix
Removed template controller, registered `AppDbContext` and `TicketService`, added `db.Database.Migrate()` on startup.

---

## Issue 2 — Frontend API connection failure

### Problem
React dev server on port 5173 could not fetch tickets; browser console showed CORS errors.

### How I investigated
Compared network tab request URL with `launchSettings.json` API port (5195). Confirmed missing CORS policy.

### Final fix
Added `AllowAnyOrigin` CORS in `Program.cs` for development. Set fetch base URL to `http://localhost:5195` in `frontend/src/api/tickets.js`.

---

## Issue 3 — README said InMemory but code used SQL Server

### Problem
Documentation claimed EF Core InMemory while `Program.cs` and `appsettings.json` configured SQL Server.

### How I investigated
Read `Program.cs` connection string registration and compared with README.

### Final fix
Updated README and `database/README.md` to document SQL Server setup. Added `Testing` environment that swaps to InMemory for automated tests.

---

## Issue 4 — Assessment could not find backend source

### Problem
Reviewer reported only a compiled frontend bundle; backend and tests appeared missing.

### How I investigated
Checked git tracked files — source exists under `src/` and `tests/` but `bin/`, `obj/`, and `node_modules/` were also committed, obscuring the real code. Frontend source was missing (only `dist/`).

### Final fix
Restored `frontend/` React source. Added `review-fixes.md` documenting the repository layout. Recommended adding `.gitignore` for build artifacts.

---

## Issue 5 — UI showed all status buttons

### Problem
Users could attempt invalid transitions from the UI even though the API rejected them.

### How I investigated
Traced button rendering in `TicketCard.jsx`; all statuses had action buttons.

### Final fix
Conditional rendering: only show buttons for valid next states per the state machine table in `data-model.md`.
