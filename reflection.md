# Reflection

## What I built
A support ticket management workspace with a .NET API, SQL Server persistence, and a React UI. The backend enforces a strict status state machine; the frontend presents create, search, filter, status transition, and comment workflows.

## How I used AI
GitHub Copilot assisted across the full lifecycle: planning, design, implementation, debugging, and review. Prompts and iterations are captured in `ai-prompts/` and summarized in `final-ai-usage-summary.md`.

## What AI helped with most
- Scaffolding the .NET project structure and EF Core migrations
- Generating the state-machine switch expression and initial xUnit tests
- Drafting React components and fetch wrappers quickly

## What I owned
- Deciding to use SQL Server instead of InMemory for real persistence
- Centralizing validation in `TicketService` after architecture review
- Identifying that only a `dist/` bundle was committed and restoring frontend source
- Responding to assessment feedback with integration tests and complete documentation

## What I would improve next
- Add authentication and role-based permissions (analyst vs admin)
- Replace permissive CORS with environment-specific origins
- Add Testcontainers-based integration tests against real SQL Server
- Capture AI prompts in real time during development, not retrospectively

## Key lesson
AI accelerates boilerplate, but assessors (and teammates) need a **complete, reproducible repository** and a **traceable development trail**. Documentation and test files are as important as the feature code.
