# Planning Prompt

## Iteration 1

**Prompt:**
> Build a backend-heavy support ticket management system with a .NET API and simple React UI. Users should create tickets, search/filter them, and move them through a status lifecycle. Include tests for the state machine.

**AI response summary:**
Proposed a layered .NET solution: models, `TicketService`, `TicketsController`, EF Core persistence, and a React dashboard with cards and a create form.

**Accepted:**
- Project structure (`src/`, `tests/`, `frontend/`)
- Ticket domain model with `Open → InProgress → Resolved → Closed` flow
- xUnit tests targeting `TicketService`

**Rejected:**
- Suggestion to use a single-file API without a service layer

---

## Iteration 2

**Prompt:**
> Break this into ordered tasks with risks. Map each task to a file I should create.

**AI response summary:**
Suggested: (1) models + DbContext, (2) service with state machine, (3) controller, (4) React UI, (5) tests, (6) docs.

**Accepted:**
- Task order and risk list (port mismatch, transition rule ambiguity)
- Mitigation: centralize transitions in service + unit tests

**Mapped to implementation plan:**
See `implementation-plan.md` and traceability table in `requirements-analysis.md`.
