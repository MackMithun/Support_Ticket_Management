import { useCallback, useEffect, useState } from 'react'
import { fetchUsers } from './api/users'
import {
  addComment,
  createTicket,
  fetchTickets,
  updateTicket,
  updateTicketStatus,
} from './api/tickets'
import TicketCard from './components/TicketCard'
import TicketDetail from './components/TicketDetail'
import TicketFilters from './components/TicketFilters'
import TicketForm from './components/TicketForm'

export default function App() {
  const [tickets, setTickets] = useState([])
  const [users, setUsers] = useState([])
  const [search, setSearch] = useState('')
  const [status, setStatus] = useState('')
  const [selectedTicketId, setSelectedTicketId] = useState(null)
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(true)

  const loadTickets = useCallback(async () => {
    setLoading(true)
    setError('')
    try {
      const data = await fetchTickets(search, status)
      setTickets(data)
    } catch {
      setError('Could not reach the API. Is the backend running on http://localhost:5195?')
    } finally {
      setLoading(false)
    }
  }, [search, status])

  useEffect(() => {
    loadTickets()
  }, [loadTickets])

  useEffect(() => {
    fetchUsers()
      .then(setUsers)
      .catch(() => setError('Could not load seeded users from the API.'))
  }, [])

  async function handleCreate(ticket) {
    await createTicket(ticket)
    await loadTickets()
  }

  async function handleStatusChange(id, newStatus) {
    await updateTicketStatus(id, newStatus)
    await loadTickets()
  }

  async function handleCommentAdded(id, comment) {
    await addComment(id, comment)
    await loadTickets()
  }

  async function handleUpdate(id, updates) {
    const updated = await updateTicket(id, updates)
    await loadTickets()
    return updated
  }

  return (
    <div className="app">
      <header className="app-header">
        <h1>Support Ticket Management</h1>
        <span className="ticket-count">{tickets.length} ticket(s)</span>
      </header>
      {error && <div className="error-banner" role="alert">{error}</div>}
      <div className="app-layout">
        <aside className="sidebar">
          <TicketForm users={users} onCreate={handleCreate} onError={setError} />
          <TicketFilters
            search={search}
            status={status}
            onSearchChange={setSearch}
            onStatusChange={setStatus}
          />
        </aside>
        <main className="ticket-list">
          {selectedTicketId ? (
            <TicketDetail
              ticketId={selectedTicketId}
              users={users}
              onBack={() => setSelectedTicketId(null)}
              onStatusChange={handleStatusChange}
              onCommentAdded={handleCommentAdded}
              onUpdate={handleUpdate}
              onError={setError}
            />
          ) : (
            <>
              {loading && <p>Loading tickets...</p>}
              {!loading && tickets.length === 0 && <p>No tickets found.</p>}
              {tickets.map((ticket) => (
                <TicketCard
                  key={ticket.id}
                  ticket={ticket}
                  onViewDetail={setSelectedTicketId}
                />
              ))}
            </>
          )}
        </main>
      </div>
    </div>
  )
}
