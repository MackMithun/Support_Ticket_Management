# Design Notes

## Architecture Overview
- Frontend: React + Vite (`frontend/`)
- Backend: ASP.NET Core Web API (`src/SupportTicket.Api/`)
- Data: EF Core with SQL Server (InMemory for automated tests)

## Backend Design
Ticket management is handled through a service layer (`TicketService`) that:
- Validates input (required fields, whitespace trimming)
- Enforces the status state machine
- Persists through `AppDbContext`

Controllers are thin: they parse HTTP input, call the service, and map `ServiceResult<T>` to HTTP status codes.

## Frontend Design
The UI uses a sidebar + main panel layout:
- Sidebar: create form, search box, status filter dropdown
- Main: ticket cards with status pills, next-action buttons, and comment thread

Only valid next-status buttons are rendered per card (see `ui-flow.md`).

## Validation Strategy

### Layer 1 — Data annotations (controller input)
`CreateTicketRequest`, `UpdateTicketRequest`, and `CreateCommentRequest` use `[Required]` and `[StringLength]` attributes. The controller checks `ModelState.IsValid` and returns `ValidationProblem` (400) for annotation failures.

### Layer 2 — Service business rules
`TicketService` performs:
- Whitespace trimming on all string fields
- Explicit null/whitespace checks beyond annotations (e.g. `"   "` title)
- State machine enforcement via `IsValidTransition`
- Not-found checks before update, status change, or comment

Errors return `ServiceResult<T>.Fail(message)` → controller maps to `BadRequest(message)`.

### Layer 3 — Client-side (React)
- Block form submit when required fields are empty
- Display API error text in a banner
- Hide invalid status action buttons

### Error response summary

| Condition | HTTP status | Body |
|-----------|-------------|------|
| Annotation validation failure | 400 | RFC 7807 ValidationProblem |
| Business rule failure | 400 | Plain text error message |
| Ticket not found (GET by id) | 404 | Empty |
| Success create | 201 | Ticket JSON |
| Success read/update | 200 | Ticket or Comment JSON |

See `api-contract.md` for endpoint-level detail.

## Design Trade-offs

### PATCH vs PUT for status changes

| Approach | Pros | Cons | Decision |
|----------|------|------|----------|
| `PUT /api/tickets/{id}` with full body | Single endpoint | Mixes metadata edits with workflow; easy to bypass state machine | Rejected |
| `PATCH /api/tickets/{id}/status` | Separates workflow from CRUD; intent is explicit | Extra endpoint | **Chosen** |
| `POST /api/tickets/{id}/transitions` | Event-sourcing style | Overkill for exercise scope | Rejected |

`PUT` updates title, description, priority, and assignee only. Status can only change through `PATCH .../status`, which routes to `TicketService.IsValidTransition`. This prevents a client from setting `status: "Closed"` directly on a metadata update.

### Three-layer validation

| Layer | Responsibility | Why keep it |
|-------|----------------|-------------|
| Data annotations | Structural input (required, max length) | Standard ASP.NET pattern; produces RFC 7807 `ValidationProblem` |
| Service rules | Business logic (whitespace, transitions, not-found) | Single place for rules; unit-testable without HTTP |
| Client-side | UX guardrails (empty fields, hide invalid buttons) | Faster feedback; reduces unnecessary API calls |

**Trade-off:** Some rules are duplicated (e.g. required title in both annotation and service). The duplication is intentional — annotations catch malformed HTTP input; the service enforces domain rules even if called from another entry point later.

### CORS policy

`AllowAnyOrigin()` is enabled for local development so the Vite dev server (5173) can call the API (5195). This is not production-safe; a real deployment would restrict origins via configuration.

### SQL Server vs InMemory

| Environment | Provider | Reason |
|-------------|----------|--------|
| Development / demo | SQL Server | Real persistence across restarts |
| `dotnet test` (`Testing` env) | InMemory | No external DB dependency in CI or fresh clones |
