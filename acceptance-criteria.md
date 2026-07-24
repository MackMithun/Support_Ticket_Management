# Acceptance Criteria

Each criterion maps to an automated test and/or a manual verification scenario.

## Core assignment criteria

| # | Criterion | Automated test | Manual scenario |
|---|-----------|----------------|-----------------|
| AC-1 | Create a ticket via the UI | `PostTicket_ValidRequest_Returns201` | Submit create form; ticket appears in list |
| AC-2 | View all tickets from the database | `GetTickets_ReturnsOk` | Dashboard loads seeded tickets |
| AC-3 | Open a ticket detail view | `GetTicketById_Existing_Returns200` | Click "View details"; full panel loads via GET by id |
| AC-4 | Update ticket fields and reassign | `PutTicket_UpdatesFields_Returns200` | Edit title/priority/assignee in detail view |
| AC-5 | Add comments to a ticket | `PostComment_ValidRequest_Returns200` | Add comment in detail view |
| AC-6 | Status changes only through valid transitions | `PatchStatus_OpenToInProgress_Returns200`, etc. (all 5 valid paths) | Status buttons in detail view |
| AC-7 | Invalid transitions rejected | `PatchStatus_InvalidTransition_Returns400`, `PatchStatus_ClosedToAny_Returns400` | API 400; UI hides invalid buttons |
| AC-8 | Keyword search and status filter | `GetTickets_SearchFilter_ReturnsMatching` | Search box + status dropdown |
| AC-9 | Data survives restart | — (manual) | SQL Server persistence after API restart |
| AC-10 | Backend validation prevents invalid records | `PostTicket_EmptyTitle_Returns400`, `CreateAsync_ShouldReject_WhitespaceTitleAndDescription` | Empty/whitespace rejected |
| AC-11 | No secrets in repo | — | Only `.env.example`; Integrated Security connection string |
| AC-12 | State-machine integration tests pass | 15 integration tests in `TicketsControllerIntegrationTests.cs` | `dotnet test` — 20/20 |

## Seeded users

| # | Criterion | Automated test | Manual scenario |
|---|-----------|----------------|-----------------|
| AC-13 | Seeded users available for assignee picker | `GetUsers_ReturnsSeededUsers` | Assignee dropdown in create/edit forms |

## Checklist summary

- [x] AC-1 Create ticket via UI
- [x] AC-2 View all tickets
- [x] AC-3 Ticket detail view
- [x] AC-4 Update fields / reassign
- [x] AC-5 Comments
- [x] AC-6 Valid status transitions (all 5 paths)
- [x] AC-7 Invalid transitions rejected
- [x] AC-8 Search and filter
- [x] AC-9 Data persists (SQL Server)
- [x] AC-10 Backend validation
- [x] AC-11 No secrets committed
- [x] AC-12 Integration tests pass (20/20)
- [x] AC-13 Seeded users
