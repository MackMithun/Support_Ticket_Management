# Design Prompt

## Iteration 1

**Prompt:**
> Design the ticket data model and API endpoints for a support ticket system. Tickets have title, description, priority, status, assignee, and comments. Status must follow a strict state machine.

**AI response summary:**
Proposed `Ticket` and `Comment` entities, `TicketStatus` enum, and REST endpoints: GET/POST tickets, PATCH status, POST comments.

**Accepted:**
- Entity fields and one-to-many Ticket→Comment relationship
- PATCH for status (not PUT) to separate metadata updates from workflow

**Rejected:**
- Flat `statusHistory` JSON column — chose normalized `Ticket.Status` with service-layer rules instead

---

## Iteration 2

**Prompt:**
> Define valid status transitions. Open can go to InProgress or Cancelled. InProgress to Resolved or Cancelled. Resolved to Closed only. Everything else is invalid.

**AI response summary:**
Generated a C# switch expression for `IsValidTransition`.

**Accepted:**
- Transition table as documented in `data-model.md`

---

## Iteration 3

**Prompt:**
> Document the API contract with request/response shapes and error codes for each endpoint.

**AI response summary:**
Drafted endpoint list with 200/201/400/404 responses.

**Accepted and refined:**
- Expanded into `api-contract.md` with full payload examples
- Added `ServiceResult<T>` pattern for service-layer errors
