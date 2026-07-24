# Requirement Analysis

## Selected project option
Backend-heavy support ticket management system

## My understanding
The application should let a support team create tickets, review them in a dashboard, search and filter tickets, and move them through a controlled status lifecycle. The backend owns business rules; the frontend presents the workflow clearly.

## Functional requirements
- Create tickets with title, description, priority, and optional assignee
- List, search, and filter tickets by status
- Update ticket status through valid transitions only
- Reject invalid transitions with a clear error message
- Add comments to tickets
- Persist data across API restarts (SQL Server)

## Non-functional requirements
- Simple, organized UI with status pills and action buttons
- Clear validation and error feedback (client and server)
- Automated unit and integration tests for the state machine
- README and database setup documentation for reproducible local runs

## Assumptions
- Single-user / no authentication required for this exercise
- Priority is a free-text field constrained to Low / Medium / High in the UI
- Timestamps are stored in UTC
- Search is a simple substring match on title and description (no full-text index)
- One SQL Server instance per developer machine

## Edge cases

| Scenario | Expected behavior |
|----------|-------------------|
| Whitespace-only title or description | Rejected by service with 400 |
| Transition from Closed to any state | Rejected — terminal state |
| Transition from Cancelled to any state | Rejected — terminal state |
| Resolved → Open (reopen) | Rejected — not in state machine |
| Update non-existent ticket | 400 with "Ticket not found" |
| Comment with empty message or author | Rejected by service |
| Invalid status query parameter | Ignored; returns unfiltered results |
| Duplicate ticket titles | Allowed — no uniqueness constraint |
| API down while UI loads | UI shows connection error banner |

## Plan-to-task traceability

| Requirement | Implementation task | File(s) |
|-------------|---------------------|---------|
| Create tickets | Backend create endpoint + UI form | `TicketService.CreateAsync`, `TicketForm.jsx` |
| List tickets | GET endpoint + dashboard load | `TicketsController.Get`, `App.jsx` |
| Search / filter | Query params on GET | `TicketService.GetAllAsync`, `TicketFilters.jsx` |
| Status transitions | State machine in service | `TicketService.IsValidTransition` |
| Reject invalid transitions | ServiceResult + 400 | `TicketsController.UpdateStatus` |
| Comments | POST comments endpoint | `TicketService.AddCommentAsync` |
| Persistence | EF Core + SQL Server | `AppDbContext`, migrations |
| Tests | Unit + integration | `TicketServiceTests`, `TicketsControllerIntegrationTests` |
| Documentation | Planning and design artifacts | `data-model.md`, `ui-flow.md`, `database/README.md` |
