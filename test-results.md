# Test Results

- **Date:** 2026-07-24
- **Command:** `dotnet test tests/SupportTicket.Api.Tests/SupportTicket.Api.Tests.csproj`
- **Result:** Passed
- **Summary:** 20 tests run, 0 failed, 20 succeeded

## Unit tests (5)

| Test | Result |
|------|--------|
| `ValidTransitions_ShouldSucceed_And_InvalidTransitions_ShouldBeRejected` | Passed |
| `CancelFromOpen_ShouldSucceed` | Passed |
| `UpdateStatusAsync_ShouldFail_WhenTicketNotFound` | Passed |
| `AddCommentAsync_ShouldReject_EmptyMessage` | Passed |
| `CreateAsync_ShouldReject_WhitespaceTitleAndDescription` | Passed |

## Integration tests (15)

| Test | Result |
|------|--------|
| `PostTicket_ValidRequest_Returns201` | Passed |
| `PostTicket_EmptyTitle_Returns400` | Passed |
| `GetTicketById_Existing_Returns200` | Passed |
| `PutTicket_UpdatesFields_Returns200` | Passed |
| `GetTickets_SearchFilter_ReturnsMatching` | Passed |
| `PatchStatus_OpenToInProgress_Returns200` | Passed |
| `PatchStatus_InProgressToResolved_Returns200` | Passed |
| `PatchStatus_ResolvedToClosed_Returns200` | Passed |
| `PatchStatus_OpenToCancelled_Returns200` | Passed |
| `PatchStatus_InProgressToCancelled_Returns200` | Passed |
| `PatchStatus_InvalidTransition_Returns400` | Passed |
| `PatchStatus_ClosedToAny_Returns400` | Passed |
| `PostComment_ValidRequest_Returns200` | Passed |
| `GetTickets_ReturnsOk` | Passed |
| `GetUsers_ReturnsSeededUsers` | Passed |

## State machine coverage

All five valid transitions and invalid transitions tested at HTTP level.
