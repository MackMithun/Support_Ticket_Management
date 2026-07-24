import { useEffect, useState } from 'react'
import { fetchTicketById } from '../api/tickets'

const NEXT_ACTIONS = {
  Open: [
    { label: 'Start', status: 'InProgress' },
    { label: 'Cancel', status: 'Cancelled' },
  ],
  InProgress: [
    { label: 'Resolve', status: 'Resolved' },
    { label: 'Cancel', status: 'Cancelled' },
  ],
  Resolved: [{ label: 'Close', status: 'Closed' }],
  Closed: [],
  Cancelled: [],
}

export default function TicketDetail({
  ticketId,
  users,
  onBack,
  onStatusChange,
  onCommentAdded,
  onUpdate,
  onError,
}) {
  const [ticket, setTicket] = useState(null)
  const [loading, setLoading] = useState(true)
  const [editing, setEditing] = useState(false)
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [priority, setPriority] = useState('Medium')
  const [assignedTo, setAssignedTo] = useState('')

  useEffect(() => {
    let cancelled = false
    async function load() {
      setLoading(true)
      try {
        const data = await fetchTicketById(ticketId)
        if (!cancelled) {
          setTicket(data)
          setTitle(data.title)
          setDescription(data.description)
          setPriority(data.priority)
          setAssignedTo(data.assignedTo ?? '')
        }
      } catch (err) {
        if (!cancelled) onError(err.message)
      } finally {
        if (!cancelled) setLoading(false)
      }
    }
    load()
    return () => { cancelled = true }
  }, [ticketId, onError])

  async function handleStatus(newStatus) {
    try {
      await onStatusChange(ticketId, newStatus)
      setTicket(await fetchTicketById(ticketId))
    } catch (err) {
      onError(err.message)
    }
  }

  async function handleCommentSubmit(event) {
    event.preventDefault()
    const form = event.target
    const message = form.message.value.trim()
    const createdBy = form.createdBy.value.trim()
    if (!message || !createdBy) {
      onError('Comment message and author are required.')
      return
    }
    try {
      await onCommentAdded(ticketId, { message, createdBy })
      form.reset()
      setTicket(await fetchTicketById(ticketId))
    } catch (err) {
      onError(err.message)
    }
  }

  async function handleSaveEdit(event) {
    event.preventDefault()
    if (!title.trim() || !description.trim()) {
      onError('Title and description are required.')
      return
    }
    try {
      const updated = await onUpdate(ticketId, {
        title: title.trim(),
        description: description.trim(),
        priority,
        assignedTo: assignedTo || null,
      })
      setTicket(updated)
      setEditing(false)
    } catch (err) {
      onError(err.message)
    }
  }

  if (loading) return <p>Loading ticket...</p>
  if (!ticket) return <p>Ticket not found.</p>

  const actions = NEXT_ACTIONS[ticket.status] ?? []

  return (
    <article className="ticket-detail">
      <button type="button" className="back-button" onClick={onBack}>
        ← Back to list
      </button>
      <header className="ticket-card__header">
        <h2>{ticket.title}</h2>
        <span className={`status-pill status-${ticket.status}`}>{ticket.status}</span>
      </header>

      {!editing ? (
        <>
          <p className="ticket-card__description">{ticket.description}</p>
          <dl className="ticket-card__meta">
            <div><dt>Priority</dt><dd>{ticket.priority}</dd></div>
            <div><dt>Assigned</dt><dd>{ticket.assignedTo || 'Unassigned'}</dd></div>
            <div><dt>Created by</dt><dd>{ticket.createdBy}</dd></div>
            <div><dt>Created</dt><dd>{new Date(ticket.createdAt).toLocaleString()}</dd></div>
            <div><dt>Updated</dt><dd>{new Date(ticket.updatedAt).toLocaleString()}</dd></div>
          </dl>
          <button type="button" onClick={() => setEditing(true)}>Edit ticket</button>
        </>
      ) : (
        <form className="ticket-edit-form" onSubmit={handleSaveEdit}>
          <label>
            Title
            <input value={title} onChange={(e) => setTitle(e.target.value)} required />
          </label>
          <label>
            Description
            <textarea value={description} onChange={(e) => setDescription(e.target.value)} required />
          </label>
          <label>
            Priority
            <select value={priority} onChange={(e) => setPriority(e.target.value)}>
              <option value="Low">Low</option>
              <option value="Medium">Medium</option>
              <option value="High">High</option>
            </select>
          </label>
          <label>
            Assignee
            <select value={assignedTo} onChange={(e) => setAssignedTo(e.target.value)}>
              <option value="">Unassigned</option>
              {users.map((user) => (
                <option key={user.id} value={user.name}>{user.name} ({user.role})</option>
              ))}
            </select>
          </label>
          <div className="ticket-card__actions">
            <button type="submit">Save changes</button>
            <button type="button" onClick={() => setEditing(false)}>Cancel</button>
          </div>
        </form>
      )}

      {actions.length > 0 && (
        <div className="ticket-card__actions">
          {actions.map((action) => (
            <button key={action.status} type="button" onClick={() => handleStatus(action.status)}>
              {action.label}
            </button>
          ))}
        </div>
      )}

      <section className="comments-section">
        <h3>Comments</h3>
        {ticket.comments?.length > 0 ? (
          <ul className="comment-list">
            {ticket.comments.map((comment) => (
              <li key={comment.id}>
                <strong>{comment.createdBy}</strong>
                <span className="comment-date">{new Date(comment.createdAt).toLocaleString()}</span>
                <p>{comment.message}</p>
              </li>
            ))}
          </ul>
        ) : (
          <p className="muted">No comments yet.</p>
        )}
        <form className="comment-form" onSubmit={handleCommentSubmit}>
          <input name="createdBy" placeholder="Your name" />
          <input name="message" placeholder="Add a comment..." />
          <button type="submit">Comment</button>
        </form>
      </section>
    </article>
  )
}
