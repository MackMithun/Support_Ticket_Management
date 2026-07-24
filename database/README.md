# Database Setup

## Overview

The API uses **SQL Server** with EF Core migrations. The database is created automatically on first run via `db.Database.Migrate()` in `Program.cs`.

## Prerequisites

- SQL Server Express or LocalDB installed locally
- .NET 10 SDK

## Connection String

Default configuration in `src/SupportTicket.Api/appsettings.json`:

```
Data Source=localhost\SQLEXPRESS01;
Integrated Security=True;
Initial Catalog=SupportManagement;
Encrypt=True;
TrustServerCertificate=True;
```

Update `Data Source` and `Initial Catalog` to match your local SQL Server instance.

### LocalDB alternative

```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SupportManagement;Trusted_Connection=True;TrustServerCertificate=True"
```

## First-Time Setup

1. Ensure SQL Server is running.
2. Update the connection string in `appsettings.json` if needed.
3. From the API project directory:

```bash
cd src/SupportTicket.Api
dotnet ef database update
```

Alternatively, `dotnet run` applies pending migrations automatically at startup.

## Schema

| Table | Purpose |
|-------|---------|
| `Tickets` | Core ticket records |
| `Comments` | Comments linked to tickets (FK with cascade delete) |

See `data-model.md` for field definitions and `src/SupportTicket.Api/Migrations/` for the generated SQL.

## Seed Data

The initial migration inserts:

- Ticket #1: "VPN access issue" (Open, High priority)
- Ticket #2: "Invoice export problem" (InProgress, Medium priority)
- Comment #1 on ticket #1

## Testing Database

Unit and integration tests use **EF Core InMemory** — no SQL Server required for `dotnet test`. The `Testing` environment in `Program.cs` swaps the provider when `ASPNETCORE_ENVIRONMENT=Testing`.

## Troubleshooting

| Problem | Fix |
|---------|-----|
| Cannot connect to SQL Server | Verify instance name with `sqlcmd -L` or SSMS |
| Login failed | Use Windows auth or update credentials in connection string |
| Migration already applied | Safe to ignore; `Migrate()` is idempotent |
| Database locked | Stop other connections to `SupportManagement` |
