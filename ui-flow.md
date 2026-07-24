# UI Flow

## Screen Layout

The React dashboard (`frontend/src/App.jsx`) is a single-page workspace with three regions:

1. **Header** — app title and live ticket count
2. **Sidebar** — create-ticket form and search/filter controls
3. **Main panel** — ticket cards with status actions and comments

## User Flows

### 1. View dashboard on load

```mermaid
flowchart LR
    A[Open app] --> B[GET /api/tickets]
    B --> C[Render ticket cards]
    C --> D[Show status pills and metrics]
```

On mount, the UI fetches all tickets from `http://localhost:5195/api/tickets` and displays them as cards sorted by creation date (newest first, server-side).

### 2. Create a ticket

```mermaid
flowchart LR
    A[Fill title, description, priority, assignee] --> B[Submit form]
    B --> C{Client validation}
    C -->|invalid| D[Show inline error]
    C -->|valid| E[POST /api/tickets]
    E --> F{API response}
    F -->|201| G[Refresh list, clear form]
    F -->|400| H[Show API error message]
```

Required fields: title, description, priority. Assignee is optional.

### 3. Search and filter

```mermaid
flowchart LR
    A[Type in search box or pick status filter] --> B[GET /api/tickets?search=&status=]
    B --> C[Re-render filtered cards]
```

Search matches title or description (case-sensitive substring on server). Status filter accepts enum names (`Open`, `InProgress`, etc.).

### 4. Advance ticket status

```mermaid
flowchart LR
    A[Click next-status button on card] --> B[PATCH /api/tickets/id/status]
    B --> C{Valid transition?}
    C -->|yes| D[Update card status pill]
    C -->|no| E[Show error banner]
```

The UI only shows buttons for valid next states:

| Current status | Available actions |
|----------------|-------------------|
| Open | Start (→ InProgress), Cancel |
| InProgress | Resolve (→ Resolved), Cancel |
| Resolved | Close (→ Closed) |
| Closed / Cancelled | No actions |

### 5. Add a comment

```mermaid
flowchart LR
    A[Enter message and author] --> B[POST /api/tickets/id/comments]
    B --> C{Success?}
    C -->|yes| D[Append comment to card]
    C -->|no| E[Show error]
```

## Error Handling (UI)

| Scenario | UI behavior |
|----------|-------------|
| API unreachable | Banner: "Could not reach the API. Is the backend running?" |
| 400 validation error | Inline message from API response body |
| Invalid status transition | Error banner with transition message |
| Empty required fields | Client-side block before API call |

## Component Map

| Component | Responsibility |
|-----------|----------------|
| `App.jsx` | Layout, data fetching, global error state |
| `TicketForm.jsx` | Create-ticket form with validation |
| `TicketFilters.jsx` | Search input and status dropdown |
| `TicketCard.jsx` | Single ticket display, status buttons, comments |
| `api/tickets.js` | Fetch wrappers for all API endpoints |
