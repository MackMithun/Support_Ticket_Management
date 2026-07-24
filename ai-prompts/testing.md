# Testing Prompt

## Iteration 1 — Initial unit tests (failed gap)

**Prompt:**
> Write xUnit tests for TicketService status transitions. Cover Open→InProgress→Resolved and reject Resolved→Open.

**AI response:**
Generated two tests in `TicketServiceTests.cs` using EF Core InMemory.

**Result:** Passed locally (2/2).

**Gap found:** No tests for whitespace input, cancel path, or HTTP-level behavior.

---

## Iteration 2 — Edge cases (after self-review)

**Prompt:**
> My tests only cover the happy path. Add tests for: cancel from Open, not-found ticket ID, whitespace-only title, empty comment message.

**AI response:**
Added four additional unit test methods.

**Failure encountered:** First draft used a shared InMemory database name, causing test pollution.

**Correction (manual):**
```csharp
.UseInMemoryDatabase(Guid.NewGuid().ToString())
```
Unique DB per test — all 5 unit tests green.

---

## Iteration 3 — Integration tests (failed first attempt)

**Prompt:**
> Add WebApplicationFactory integration tests for POST /api/tickets and PATCH status. Use InMemory, not SQL Server.

**AI response:**
Generated `TicketsControllerIntegrationTests` but `Program.cs` always registered SQL Server — tests failed at startup with connection errors.

**Error:**
```
Microsoft.Data.SqlClient.SqlException: A network-related error occurred...
```

**Fix applied:**
Added `Testing` environment branch in `Program.cs`:
```csharp
if (builder.Environment.IsEnvironment("Testing"))
    options.UseInMemoryDatabase("SupportTicketTests");
```

**Result:** 5 integration tests added; full suite 10/10 passing.

---

## Iteration 4 — Tie tests to acceptance criteria

**Prompt:**
> Map each acceptance criterion in acceptance-criteria.md to a specific test method name.

**AI response:**
Drafted traceability table linking UI scenarios to unit and integration tests.

**Accepted:** Updated `acceptance-criteria.md` with test references.
