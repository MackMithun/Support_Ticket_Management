# Support Ticket Management

A .NET API and React dashboard for creating, searching, filtering, and progressing support tickets through a controlled status lifecycle.

## Stack

| Layer | Technology |
|-------|------------|
| Backend | ASP.NET Core Web API (.NET 10) |
| Frontend | React + Vite |
| Database | SQL Server via EF Core |
| Tests | xUnit (unit + integration) |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) 18+
- SQL Server Express, LocalDB, or compatible instance

## Database setup

1. Update the connection string in `src/SupportTicket.Api/appsettings.json` for your SQL Server instance.
2. See `database/README.md` for detailed setup, LocalDB alternative, and troubleshooting.

Migrations run automatically on API startup. To apply manually:

```bash
cd src/SupportTicket.Api
dotnet ef database update
```

## Run locally

### Backend

```bash
cd src/SupportTicket.Api
dotnet run
```

API: `http://localhost:5195/api/tickets`

### Frontend

```bash
cd frontend
npm install
npm run dev
```

Open the Vite URL (default `http://localhost:5173`).

## Core capabilities

- Create tickets with title, description, priority, and assignee
- Search and filter tickets by status
- Update ticket status via state machine (backend-enforced)
- Add comments to tickets
- Persist data in SQL Server

## Tests

```bash
dotnet test tests/SupportTicket.Api.Tests/SupportTicket.Api.Tests.csproj
```

Tests use EF Core InMemory — no SQL Server required for the test run.

## Repository structure

```
src/SupportTicket.Api/     # .NET API source
tests/                     # xUnit unit and integration tests
frontend/                  # React source
database/                  # Database setup docs
ai-prompts/                # AI-assisted development prompts
data-model.md              # Entity and state machine reference
ui-flow.md                 # UI user flows
```

## Documentation

| File | Purpose |
|------|---------|
| `requirements-analysis.md` | Requirements, assumptions, edge cases |
| `data-model.md` | Entities and state machine |
| `ui-flow.md` | User flows and component map |
| `api-contract.md` | REST API reference |
| `database/README.md` | SQL Server setup |
| `final-ai-usage-summary.md` | AI workflow summary |
| `review-fixes.md` | Assessment remediation checklist |
