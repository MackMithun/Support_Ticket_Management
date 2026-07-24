# Code Review Notes

## AI-assisted review summary

The implementation was reviewed for:
- Separation of concerns (controller vs service)
- State machine correctness and test coverage
- Validation at multiple layers
- UI clarity for the ticket workflow

## Findings and changes

| Finding | Severity | Action |
|---------|----------|--------|
| Business validation in controller | Medium | Moved to `TicketService`; controller only checks `ModelState` |
| Missing whitespace validation | Medium | Added explicit checks in `ValidateCreateRequest` |
| Only 2 unit tests | Medium | Added cancel path, not-found, and comment validation tests |
| No HTTP-level integration tests | High | Added `TicketsControllerIntegrationTests` |
| Frontend source not in repo | High | Restored `frontend/` directory |
| Docs missing data model and UI flow | High | Added `data-model.md`, `ui-flow.md`, `database/README.md` |
| Thin AI prompt history | Medium | Added full lifecycle prompts with iterations |

## Changes made after review

- Moved business rules into the service layer
- Added structured React dashboard with conditional status buttons
- Documented setup, test steps, and database configuration
- Expanded test strategy with integration and edge-case coverage
- Created `review-fixes.md` mapping assessment gaps to fixes

## Remaining recommendations

- Add `.gitignore` for `bin/`, `obj/`, `node_modules/`
- Consider `WebApplicationFactory` with Testcontainers for SQL Server integration tests
- Add loading and empty-state UI polish
