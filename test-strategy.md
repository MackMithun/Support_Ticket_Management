# Test Strategy

## Test scope

| Layer | What is tested | Tool |
|-------|----------------|------|
| Unit | `TicketService` business rules | xUnit + EF InMemory |
| Integration | HTTP endpoints and status codes | xUnit + `WebApplicationFactory` |
| Manual | End-to-end UI workflow | Browser + running API |

## Unit tests (`TicketServiceTests.cs`)

- Full happy path: Open → InProgress → Resolved
- Invalid transition: Resolved → Open rejected
- Whitespace-only title/description rejected on create
- Cancel from Open succeeds
- Update status on non-existent ticket fails
- Comment with empty message fails

## Integration tests (`TicketsControllerIntegrationTests.cs`)

- POST /api/tickets returns 201 with created ticket
- POST with empty title returns 400
- PATCH /api/tickets/{id}/status valid transition returns 200
- PATCH invalid transition returns 400
- GET /api/tickets returns seeded + created tickets

## Test database

- Unit tests: per-test InMemory database (unique name per test)
- Integration tests: `ASPNETCORE_ENVIRONMENT=Testing` swaps to InMemory in `Program.cs`
- No SQL Server required for `dotnet test`

## Edge cases covered

| Case | Test |
|------|------|
| Invalid state transition | `ValidTransitions_ShouldSucceed_And_InvalidTransitions_ShouldBeRejected` |
| Whitespace input | `CreateAsync_ShouldReject_WhitespaceTitleAndDescription` |
| Cancel workflow | `CancelFromOpen_ShouldSucceed` |
| Not found | `UpdateStatusAsync_ShouldFail_WhenTicketNotFound` |
| HTTP-level rejection | `PatchStatus_InvalidTransition_Returns400` |

## Not yet covered

- Concurrent status updates (optimistic concurrency)
- SQL Server-specific migration integration
- Frontend component tests (React Testing Library)
