# Acceptance Criteria

Each criterion maps to an automated test and/or a manual verification scenario.

## Functional criteria

| # | Criterion | Automated test | Manual scenario |
|---|-----------|----------------|-----------------|
| AC-1 | A user can create a ticket via the UI | `PostTicket_ValidRequest_Returns201` | Submit create form in React UI; card appears in list |
| AC-2 | A user can view all tickets from the database | `GetTickets_ReturnsOk` | Open dashboard; seeded tickets visible on load |
| AC-3 | A user can update ticket status through valid transitions | `ValidTransitions_ShouldSucceed_And_InvalidTransitions_ShouldBeRejected`, `PatchStatus_ValidTransition_Returns200` | Click "Start" on Open ticket; status pill changes to InProgress |
| AC-4 | Invalid transitions are rejected by the backend | `PatchStatus_InvalidTransition_Returns400` | API returns 400 for Resolved→Open; UI shows no invalid buttons |
| AC-5 | Search and status filter work | — (manual / future test) | Type search term; select status filter; list updates |
| AC-6 | Data persists across API restarts | — (manual) | Create ticket, restart API, confirm ticket still in SQL Server |
| AC-7 | Comments can be added to tickets | `AddCommentAsync_ShouldReject_EmptyMessage` (negative); manual for happy path | Add comment on card; appears in thread |

## Validation criteria

| # | Criterion | Automated test | Scenario |
|---|-----------|----------------|----------|
| AC-8 | Whitespace-only title rejected | `CreateAsync_ShouldReject_WhitespaceTitleAndDescription`, `PostTicket_EmptyTitle_Returns400` | POST with `"   "` title returns 400 |
| AC-9 | Cancel from Open allowed | `CancelFromOpen_ShouldSucceed` | PATCH Open→Cancelled returns 200 |
| AC-10 | Not-found ticket returns error | `UpdateStatusAsync_ShouldFail_WhenTicketNotFound` | PATCH status on ID 999 returns 400 |

## Project criteria

| # | Criterion | Verification |
|---|-----------|--------------|
| AC-11 | README setup instructions included | `README.md` + `database/README.md` |
| AC-12 | xUnit tests pass | `dotnet test` — 10/10 (see `test-results.md`) |
| AC-13 | Fresh clone reproducible | Clone repo → configure SQL Server → `dotnet run` + `npm run dev` in `frontend/` |

## Checklist summary

- [x] AC-1 Create ticket via UI
- [x] AC-2 View all tickets
- [x] AC-3 Valid status transitions
- [x] AC-4 Invalid transitions rejected
- [x] AC-5 Search and filter
- [x] AC-6 Data persists (SQL Server)
- [x] AC-7 Comments
- [x] AC-8 Whitespace validation
- [x] AC-9 Cancel workflow
- [x] AC-10 Not-found handling
- [x] AC-11 README setup
- [x] AC-12 Tests pass (10/10)
- [x] AC-13 Reproducible clone-and-run
