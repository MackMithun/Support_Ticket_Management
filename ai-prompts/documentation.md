# Documentation Prompt

## Iteration 1 — README setup (incomplete first draft)

**Prompt:**
> Write README setup instructions for the .NET API and React frontend.

**AI response:**
Draft mentioned EF Core InMemory, but `Program.cs` actually uses SQL Server.

**Failure:** Reviewer would not be able to reproduce with README alone.

**Correction:** Rewrote database section pointing to `database/README.md` with SQL Server connection string steps.

---

## Iteration 2 — API contract

**Prompt:**
> Document all REST endpoints with request/response examples and error codes.

**AI response:**
Short bullet list with endpoint names only.

**Rejected:** Too thin for assessment.

**Second prompt:**
> Expand each endpoint with JSON examples, query params, and status codes.

**Result:** Full `api-contract.md` with payloads and transition table.

---

## Iteration 3 — Design artifacts (post 32/100 review)

**Prompt:**
> Assessment requires data-model.md, ui-flow.md, and database setup docs. Generate them from the existing code.

**AI response:**
Drafted ER diagram, user flows, and migration steps.

**Manual verification:** Cross-checked enum values and transition table against `TicketService.IsValidTransition`.

---

## Iteration 4 — Acceptance criteria traceability (post 62/100 review)

**Prompt:**
> acceptance-criteria.md is a thin checklist. Tie each criterion to a test method and manual verification step.

**AI response:**
Proposed table with criterion → test → scenario.

**Accepted:** Updated `acceptance-criteria.md`.

---

## Iteration 5 — Design trade-offs

**Prompt:**
> Why PATCH for status instead of PUT? Why three validation layers? Document the trade-offs in design-notes.md.

**AI response:**
Explained REST semantics and defense-in-depth rationale.

**Accepted:** Added "Design trade-offs" section to `design-notes.md`.
