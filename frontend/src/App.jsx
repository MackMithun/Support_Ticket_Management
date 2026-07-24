import { useCallback, useEffect, useState } from 'react'
import { addComment, createTicket, fetchTickets, updateTicketStatus } from './api/tickets'
import TicketCard from './components/TicketCard'
import TicketFilters from './components/TicketFilters'
import TicketForm from './components/TicketForm'

export default function App() {
  const [tickets, setTickets] = useState([])
  const [search, setSearch] = useState('')
  const [status, setStatus] = useState('')
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

  return (
    <div className="app">
      <header className="app-header">
        <h1>Support Ticket Management</h1>
        <span className="ticket-count">{tickets.length} ticket(s)</span>
      </header>
      {error && <div className="error-banner" role="alert">{error}</div>}
      <div className="app-layout">
        <aside className="sidebar">
          <TicketForm onCreate={handleCreate} onError={setError} />
          <TicketFilters
            search={search}
            status={status}
            onSearchChange={setSearch}
            onStatusChange={setStatus}
          />
        </aside>
        <main className="ticket-list">
          {loading && <p>Loading tickets...</p>}
          {!loading && tickets.length === 0 && <p>No tickets found.</p>}
          {tickets.map((ticket) => (
            <TicketCard
              key={ticket.id}
              ticket={ticket}
              onStatusChange={handleStatusChange}
              onCommentAdded={handleCommentAdded}
              onError={setError}
            />
          ))}
        </main>
      </div>
    </div>
  )
}
