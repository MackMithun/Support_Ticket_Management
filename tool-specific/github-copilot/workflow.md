# GitHub Copilot Workflow

## How Copilot was used

| Activity | Copilot feature | Example |
|----------|----------------|---------|
| Scaffold generation | Chat / inline | "Create TicketService with state machine" |
| Code completion | Inline suggestions | Data annotation attributes on DTOs |
| Test generation | Chat | "Write xUnit tests for invalid transitions" |
| Refactoring | Chat | "Move validation from controller to service" |
| Documentation | Chat | Draft API contract and data model sections |

## Context provided to Copilot

- Exercise brief requirements (create, filter, status transitions)
- Open files in the workspace (`TicketService.cs`, `TicketModels.cs`)
- Error messages from `dotnet test` and browser console

## Workflow steps

1. **Plan** — Asked Copilot to break requirements into tasks (`ai-prompts/planning.md`)
2. **Design** — Iterated on data model and API shape (`ai-prompts/design.md`)
3. **Implement** — Generated backend then frontend in layers (`ai-prompts/implementation.md`)
4. **Debug** — Pasted errors and asked for targeted fixes (`ai-prompts/debugging.md`)
5. **Review** — Asked Copilot to critique architecture and test gaps (`ai-prompts/review.md`)
6. **Document** — Used Copilot to draft docs, then manually verified against actual code

## What was not shared with AI

- Database passwords or production connection strings
- Personal credentials or machine-specific secrets

## Comparison with Cursor workflow

The `tool-specific/cursor-workflow/` folder documents an alternate Cursor-based workflow used during a later remediation pass. GitHub Copilot was the primary tool during the original assessment period (see `candidate-info.md`).
