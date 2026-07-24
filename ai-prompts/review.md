# Review Prompt

## Iteration 1 — Architecture review

**Prompt:**
> Review my ticket API for separation of concerns. Should validation live in the controller or service?

**AI response:**
Recommended service-layer validation for business rules (transitions, whitespace) and controller-level `ModelState` for annotation-based input validation.

**Action taken:**
Kept `ModelState.IsValid` in controller; moved transition logic and custom validation to `TicketService`.

---

## Iteration 2 — Test coverage review

**Prompt:**
> What edge cases am I missing in TicketServiceTests?

**AI response:**
Suggested tests for: cancel from Open, not-found ticket, whitespace-only title, comment with empty author, Resolved→Closed happy path.

**Action taken:**
Added unit tests for whitespace rejection and cancel path. Added integration tests for HTTP-level status codes.

---

## Iteration 3 — Documentation gap review (post-assessment)

**Prompt:**
> My assessment scored 32/100 for missing docs and thin AI evidence. What files should I add?

**AI response:**
Listed: data-model.md, ui-flow.md, database setup, full ai-prompts lifecycle, integration tests, frontend source, review-fixes.md.

**Action taken:**
See `review-fixes.md` for the complete checklist.
