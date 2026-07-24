const STATUSES = ['', 'Open', 'InProgress', 'Resolved', 'Closed', 'Cancelled']

export default function TicketFilters({ search, status, onSearchChange, onStatusChange }) {
  return (
    <div className="ticket-filters">
      <h2>Search &amp; Filter</h2>
      <label>
        Search
        <input
          type="search"
          value={search}
          onChange={(e) => onSearchChange(e.target.value)}
          placeholder="Title or description..."
        />
      </label>
      <label>
        Status
        <select value={status} onChange={(e) => onStatusChange(e.target.value)}>
          <option value="">All statuses</option>
          {STATUSES.filter(Boolean).map((s) => (
            <option key={s} value={s}>{s}</option>
          ))}
        </select>
      </label>
    </div>
  )
}
