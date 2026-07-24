# Test Results

- **Date:** 2026-07-24
- **Command:** `dotnet test tests/SupportTicket.Api.Tests/SupportTicket.Api.Tests.csproj`
- **Result:** Passed
- **Summary:** 10 tests run, 0 failed, 10 succeeded

## Unit tests (5)

| Test | Result |
|------|--------|
| `ValidTransitions_ShouldSucceed_And_InvalidTransitions_ShouldBeRejected` | Passed |
| `CancelFromOpen_ShouldSucceed` | Passed |
| `UpdateStatusAsync_ShouldFail_WhenTicketNotFound` | Passed |
| `AddCommentAsync_ShouldReject_EmptyMessage` | Passed |
| `CreateAsync_ShouldReject_WhitespaceTitleAndDescription` | Passed |

## Integration tests (5)

| Test | Result |
|------|--------|
| `PostTicket_ValidRequest_Returns201` | Passed |
| `PostTicket_EmptyTitle_Returns400` | Passed |
| `PatchStatus_ValidTransition_Returns200` | Passed |
| `PatchStatus_InvalidTransition_Returns400` | Passed |
| `GetTickets_ReturnsOk` | Passed |

## Notes

- Integration tests use `WebApplicationFactory` with `ASPNETCORE_ENVIRONMENT=Testing` (InMemory database).
- No SQL Server instance required for the test run.
