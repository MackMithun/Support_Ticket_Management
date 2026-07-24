# Code Review Prompt

## Iteration 1 — Separation of concerns

**Prompt:**
> Review TicketsController.cs. Is validation in the right layer?

**AI response:**
Flagged that status transition logic should not live in the controller. Recommended service-layer enforcement.

**Action taken:** Kept `ModelState` checks in controller; moved business rules to `TicketService`.

---

## Iteration 2 — Security and CORS (rejected suggestion)

**Prompt:**
> Review Program.cs CORS policy for production readiness.

**AI response:**
Suggested `AllowAnyOrigin()` is fine for all environments.

**Rejected:** Kept permissive CORS for local development only; documented in `design-notes.md` as a known trade-off for the exercise scope.

---

## Iteration 3 — Repository hygiene (post 32/100 review)

**Prompt:**
> Reviewer says bin/, obj/, and dist/ are committed. What should I fix?

**AI response:**
Recommended `.gitignore` for build outputs and removing `dist/` since `frontend/` source exists.

**Action taken:**
- Added `.gitignore`
- Removed `dist/` from version control
- Untracked `bin/` and `obj/` folders

---

## Iteration 4 — Port inconsistency (post 62/100 review)

**Prompt:**
> dist/ bundle calls port 5014 but frontend source uses 5195. launchSettings.json says 5014. Which is correct?

**AI response:**
Identified mismatch between `launchSettings.json` (5014) and README/frontend (5195).

**Fix applied:**
- Standardized API port to **5195** in `launchSettings.json` and `SupportTicket.Api.http`
- Moved API URL to `VITE_API_URL` env variable in frontend
- Removed stale `dist/` bundle from repo
