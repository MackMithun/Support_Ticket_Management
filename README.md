# Support Ticket Management

This repository contains a .NET Core API and a React-based support ticket dashboard built as a practical AI capability exercise.

## Stack
- Backend: ASP.NET Core Web API
- Frontend: React + Vite
- Database: EF Core InMemory for a local, restart-friendly experience

## Run locally

### Backend
1. `cd src/SupportTicket.Api`
2. `dotnet run`
3. The API is available at `http://localhost:5195/api/tickets`

### Frontend
1. `cd frontend`
2. `npm install`
3. `npm run dev`
4. Open the Vite URL and interact with the ticket workspace

## Core capabilities
- Create tickets
- Search and filter tickets
- Update ticket status using a state machine
- Add comments
- Persist data in memory for local runs

## Tests
- `dotnet test tests/SupportTicket.Api.Tests/SupportTicket.Api.Tests.csproj`
