# Design Notes

## Architecture Overview
- Frontend: React + Vite
- Backend: ASP.NET Core Web API
- Data: EF Core InMemory database

## Backend Design
Ticket management is handled through a service layer that enforces the state machine and persists data through EF Core.

## Frontend Design
The UI uses cards and a structured panel layout to show metrics, create tickets, and review ticket progress.

## Validation Strategy
The backend validates required fields and enforces status transitions.
