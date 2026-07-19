# Debugging Notes

## Issue 1
### Problem
The initial scaffold contained weather-forecast example code that did not match the ticketing domain.
### How I Investigated
I replaced the starter endpoint and registered the ticket service and EF Core context.
### Final Fix
The API now serves ticket-related CRUD and status transition endpoints.
