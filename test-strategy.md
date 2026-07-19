# Test Strategy

## Test scope
- Validate ticket state transitions.
- Ensure invalid status changes are rejected.

## Unit tests
- Ticket transitions from Open to InProgress and InProgress to Resolved.
- Invalid transitions from Resolved to Open are rejected.
