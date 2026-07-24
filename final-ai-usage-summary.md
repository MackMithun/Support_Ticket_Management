# Final AI Usage Summary

## Tool

**GitHub Copilot** (primary) with repository context from the exercise brief and existing files.

## Lifecycle coverage

| Phase | Prompt file | Iterations | Outcome |
|-------|-------------|------------|---------|
| Planning | `ai-prompts/planning.md` | 2 | Project structure and task breakdown agreed |
| Design | `ai-prompts/design.md` | 3 | Data model, API contract, state machine defined |
| Implementation | `ai-prompts/implementation.md` | 4 | Backend service, controller, React UI scaffolded |
| Debugging | `ai-prompts/debugging.md` | 2 | Fixed scaffold mismatch and port configuration |
| Review | `ai-prompts/review.md` | 2 | Refactored validation into service layer |

## What AI did well

- Rapid scaffolding of .NET API layers (models, service, controller)
- Generating the state-machine switch expression with correct transitions
- Creating xUnit test templates for valid/invalid transitions
- Producing React component structure and fetch wrappers

## What I corrected manually

- Replaced weather-forecast starter code with ticket domain
- Aligned README database description with actual SQL Server usage
- Tightened validation messages and whitespace checks in `TicketService`
- Added integration tests and expanded edge-case coverage after review
- Restored frontend source files that were only present as a `dist/` bundle

## Prompting patterns that worked

1. **Constrain scope** — "backend-heavy, simple React UI, no auth" kept responses focused
2. **Reference existing files** — "@TicketService.cs enforce these transitions" produced targeted edits
3. **Iterate with rejection** — When AI suggested InMemory DB but SQL Server was required, I redirected with the actual connection string
4. **Ask for tests alongside code** — Pairing "implement X" with "write xUnit tests for invalid transitions" reduced rework

## Prompting patterns to improve

- Capture prompts at each step in real time (not reconstructed after the fact)
- Ask AI to critique its own output before accepting ("review this for edge cases")
- Request integration test scaffolding earlier in the workflow

## Ownership statement

I reviewed all AI-generated code, ran tests locally, and made deliberate decisions on architecture (service-layer validation, EF Core migrations, status state machine). AI accelerated boilerplate; business rules and final structure are mine.
