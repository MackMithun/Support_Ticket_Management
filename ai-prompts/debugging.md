# Debugging Prompt

## Iteration 1 — Scaffold mismatch

**Problem:**
`dotnet new webapi` scaffold still had WeatherForecast controller and no ticket wiring.

**Prompt:**
> The API still serves weather data. Replace it with ticket endpoints. Register AppDbContext and run migrations on startup.

**AI response:**
Removed template code, added `Program.cs` migration call and CORS for React dev server.

**Fix verified:**
`GET /api/tickets` returns seeded tickets after `dotnet run`.

---

## Iteration 2 — Frontend cannot reach API

**Problem:**
React app showed network errors; CORS and port mismatch.

**Prompt:**
> React on port 5173 cannot call API on 5195. Add CORS policy and confirm the fetch base URL.

**AI response:**
Added `AllowAnyOrigin` CORS in development and hardcoded `http://localhost:5195` in the API client.

**Fix verified:**
Ticket list loads in browser with both servers running.

---

## Iteration 3 — Invalid transition not surfaced in UI

**Problem:**
Clicking a disabled transition still possible if UI showed all status buttons.

**Prompt:**
> Only render status action buttons for valid next states based on the state machine table.

**AI response:**
Conditional button rendering per current status.

**Fix verified:**
Closed and Cancelled tickets show no action buttons.
