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

export default function TicketCard({ ticket, onStatusChange, onCommentAdded, onError }) {
  const actions = NEXT_ACTIONS[ticket.status] ?? []

  async function handleStatus(newStatus) {
    try {
      await onStatusChange(ticket.id, newStatus)
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
      await onCommentAdded(ticket.id, { message, createdBy })
      form.reset()
    } catch (err) {
      onError(err.message)
    }
  }

  return (
    <article className="ticket-card">
      <header className="ticket-card__header">
        <h3>{ticket.title}</h3>
        <span className={`status-pill status-${ticket.status}`}>{ticket.status}</span>
      </header>
      <p className="ticket-card__description">{ticket.description}</p>
      <dl className="ticket-card__meta">
        <div><dt>Priority</dt><dd>{ticket.priority}</dd></div>
        <div><dt>Assigned</dt><dd>{ticket.assignedTo || 'Unassigned'}</dd></div>
        <div><dt>Created</dt><dd>{new Date(ticket.createdAt).toLocaleString()}</dd></div>
      </dl>
      {actions.length > 0 && (
        <div className="ticket-card__actions">
          {actions.map((action) => (
            <button
              key={action.status}
              type="button"
              onClick={() => handleStatus(action.status)}
            >
              {action.label}
            </button>
          ))}
        </div>
      )}
      {ticket.comments?.length > 0 && (
        <ul className="comment-list">
          {ticket.comments.map((comment) => (
            <li key={comment.id}>
              <strong>{comment.createdBy}</strong>: {comment.message}
            </li>
          ))}
        </ul>
      )}
      <form className="comment-form" onSubmit={handleCommentSubmit}>
        <input name="createdBy" placeholder="Your name" />
        <input name="message" placeholder="Add a comment..." />
        <button type="submit">Comment</button>
      </form>
    </article>
  )
}
