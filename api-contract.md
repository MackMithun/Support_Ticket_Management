# API Contract

Base URL: `http://localhost:5195/api/tickets`

## GET /api/tickets

Retrieve tickets with optional search and status filtering.

**Query parameters:**

| Param | Type | Required | Description |
|-------|------|----------|-------------|
| search | string | no | Substring match on title or description |
| status | string | no | Enum name: Open, InProgress, Resolved, Closed, Cancelled |

**Response:** `200 OK` — `Ticket[]`

```json
[
  {
    "id": 1,
    "title": "VPN access issue",
    "description": "Sales team cannot connect to the VPN.",
    "priority": "High",
    "status": "Open",
    "assignedTo": "Mina",
    "createdBy": "System",
    "createdAt": "2026-07-18T08:00:00Z",
    "updatedAt": "2026-07-18T08:00:00Z",
    "comments": []
  }
]
```

---

## GET /api/tickets/{id}

**Response:** `200 OK` — `Ticket` | `404 Not Found`

---

## POST /api/tickets

Create a new ticket. Status defaults to `Open`.

**Request body:**

```json
{
  "title": "Login issue",
  "description": "User cannot sign in",
  "priority": "High",
  "assignedTo": "Asha"
}
```

**Responses:**
- `201 Created` — returns created `Ticket` with `Location` header
- `400 Bad Request` — validation or business rule error

---

## PUT /api/tickets/{id}

Update ticket metadata (not status).

**Request body** (all fields optional):

```json
{
  "title": "Updated title",
  "description": "Updated description",
  "priority": "Low",
  "assignedTo": "Jordan"
}
```

**Responses:**
- `200 OK` — updated `Ticket`
- `400 Bad Request` — not found or validation error

---

## PATCH /api/tickets/{id}/status

Change ticket status. Body is a raw enum value:

```json
"InProgress"
```

**Valid transitions:**

| From | Allowed values |
|------|----------------|
| Open | InProgress, Cancelled |
| InProgress | Resolved, Cancelled |
| Resolved | Closed |

**Responses:**
- `200 OK` — updated `Ticket`
- `400 Bad Request` — invalid transition or not found

Example error:

```json
"Invalid transition from Resolved to Open."
```

---

## POST /api/tickets/{id}/comments

**Request body:**

```json
{
  "message": "Investigating the VPN profile.",
  "createdBy": "Mina"
}
```

**Responses:**
- `200 OK` — created `Comment`
- `400 Bad Request` — not found or missing fields

---

## Status enum values

`Open` = 0, `InProgress` = 1, `Resolved` = 2, `Closed` = 3, `Cancelled` = 4

JSON serialization uses string enum names.
