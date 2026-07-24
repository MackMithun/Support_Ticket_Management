const API_BASE = 'http://localhost:5195/api/tickets'

async function handleResponse(response) {
  if (!response.ok) {
    const text = await response.text()
    throw new Error(text || `Request failed (${response.status})`)
  }
  return response.json()
}

export async function fetchTickets(search = '', status = '') {
  const params = new URLSearchParams()
  if (search) params.set('search', search)
  if (status) params.set('status', status)
  const query = params.toString()
  const url = query ? `${API_BASE}?${query}` : API_BASE
  const response = await fetch(url)
  return handleResponse(response)
}

export async function createTicket(ticket) {
  const response = await fetch(API_BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(ticket),
  })
  return handleResponse(response)
}

export async function updateTicketStatus(id, status) {
  const response = await fetch(`${API_BASE}/${id}/status`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(status),
  })
  return handleResponse(response)
}

export async function addComment(id, comment) {
  const response = await fetch(`${API_BASE}/${id}/comments`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(comment),
  })
  return handleResponse(response)
}
