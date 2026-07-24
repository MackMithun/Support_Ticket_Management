# Implementation Prompt

## Iteration 1 — Backend models and DbContext

**Prompt:**
> Create Ticket and Comment models with data annotations. Add AppDbContext with EF Core SQL Server, seed data for two sample tickets, and an initial migration.

**AI response:**
Generated `TicketModels.cs`, `AppDbContext.cs`, and migration files.

**Correction:**
Verified connection string matches local SQL Server instance name.

---

## Iteration 2 — TicketService

**Prompt:**
> Implement TicketService with CreateAsync, GetAllAsync (search + status filter), UpdateStatusAsync with state machine validation, and AddCommentAsync. Return ServiceResult for errors.

**AI response:**
Full service implementation with validation helpers.

**Correction:**
Added explicit whitespace checks for title/description beyond data annotations.

---

## Iteration 3 — Controller

**Prompt:**
> Create TicketsController with REST endpoints matching api-contract.md. Use ModelState validation and map ServiceResult errors to 400.

**AI response:**
Controller with all CRUD + status + comment endpoints.

**Correction:**
Injected `AppDbContext` and constructed `TicketService` inline (kept simple for exercise scope).

---

## Iteration 4 — React UI

**Prompt:**
> Build a React dashboard: sidebar form to create tickets, search/filter bar, ticket cards with status pills and next-action buttons. Fetch from localhost:5195.

**AI response:**
Component scaffold with `fetch` calls and CSS layout.

**Correction:**
Only showed action buttons for valid next transitions. Restored source to `frontend/` after initial build output was committed as `dist/` only.
