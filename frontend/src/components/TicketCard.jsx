export default function TicketCard({ ticket, onViewDetail }) {
  return (
    <article className="ticket-card ticket-card--compact">
      <header className="ticket-card__header">
        <h3>{ticket.title}</h3>
        <span className={`status-pill status-${ticket.status}`}>{ticket.status}</span>
      </header>
      <p className="ticket-card__description">{ticket.description}</p>
      <dl className="ticket-card__meta">
        <div><dt>Priority</dt><dd>{ticket.priority}</dd></div>
        <div><dt>Assigned</dt><dd>{ticket.assignedTo || 'Unassigned'}</dd></div>
        <div><dt>Comments</dt><dd>{ticket.comments?.length ?? 0}</dd></div>
      </dl>
      <button type="button" onClick={() => onViewDetail(ticket.id)}>
        View details
      </button>
    </article>
  )
}
