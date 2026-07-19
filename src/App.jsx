import { useEffect, useMemo, useState } from 'react';
import './App.css';

const API_URL = 'http://localhost:5014/api/tickets';

const statusColors = {
  Open: '#2563eb',
  InProgress: '#f59e0b',
  Resolved: '#10b981',
  Closed: '#6b7280',
  Cancelled: '#ef4444'
};

function App() {
  const [tickets, setTickets] = useState([]);
  const [search, setSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState('');
  const [form, setForm] = useState({ title: '', description: '', priority: 'High', assignedTo: '' });
  const [message, setMessage] = useState('');
  const [loading, setLoading] = useState(true);

  const fetchTickets = async () => {
    setLoading(true);
    const query = new URLSearchParams();
    if (search) query.set('search', search);
    if (statusFilter) query.set('status', statusFilter);
    const response = await fetch(`${API_URL}?${query.toString()}`);
    const data = await response.json();
    setTickets(data);
    setLoading(false);
  };

  useEffect(() => {
    fetchTickets();
  }, [search, statusFilter]);

  const stats = useMemo(() => ({
    total: tickets.length,
    open: tickets.filter((ticket) => ticket.status === 'Open').length,
    progress: tickets.filter((ticket) => ticket.status === 'InProgress').length,
    resolved: tickets.filter((ticket) => ticket.status === 'Resolved').length
  }), [tickets]);

  const handleCreate = async (event) => {
    event.preventDefault();
    const response = await fetch(API_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form)
    });

    if (response.ok) {
      setForm({ title: '', description: '', priority: 'High', assignedTo: '' });
      setMessage('Ticket created successfully.');
      fetchTickets();
    } else {
      setMessage('Please complete the required fields.');
    }
  };

  const handleStateChange = async (ticketId, nextStatus) => {
    const response = await fetch(`${API_URL}/${ticketId}/status`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(nextStatus)
    });

    if (response.ok) {
      setMessage(`Ticket moved to ${nextStatus}.`);
      fetchTickets();
    }
  };

  return (
    <div className="app-shell">
      <header className="hero-card">
        <div>
          <p className="eyebrow">Support Operations</p>
          <h1>Enterprise support ticket workspace</h1>
          <p>Track, triage, and move tickets through a governed workflow with a clean operational view.</p>
        </div>
        <div className="hero-metrics">
          <div><strong>{stats.total}</strong><span>Total</span></div>
          <div><strong>{stats.open}</strong><span>Open</span></div>
          <div><strong>{stats.progress}</strong><span>In Progress</span></div>
          <div><strong>{stats.resolved}</strong><span>Resolved</span></div>
        </div>
      </header>

      <section className="panel-grid">
        <form className="panel" onSubmit={handleCreate}>
          <h2>Create ticket</h2>
          <label>Title
            <input value={form.title} onChange={(e) => setForm({ ...form, title: e.target.value })} placeholder="Incident title" required />
          </label>
          <label>Description
            <textarea value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} placeholder="Describe the issue" rows="4" required />
          </label>
          <label>Priority
            <select value={form.priority} onChange={(e) => setForm({ ...form, priority: e.target.value })}>
              <option value="High">High</option>
              <option value="Medium">Medium</option>
              <option value="Low">Low</option>
            </select>
          </label>
          <label>Assigned To
            <input value={form.assignedTo} onChange={(e) => setForm({ ...form, assignedTo: e.target.value })} placeholder="Owner" />
          </label>
          <button type="submit">Submit ticket</button>
          {message ? <p className="feedback">{message}</p> : null}
        </form>

        <div className="panel">
          <div className="toolbar">
            <input value={search} onChange={(e) => setSearch(e.target.value)} placeholder="Search by title or description" />
            <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)}>
              <option value="">All statuses</option>
              <option value="Open">Open</option>
              <option value="InProgress">In Progress</option>
              <option value="Resolved">Resolved</option>
              <option value="Closed">Closed</option>
              <option value="Cancelled">Cancelled</option>
            </select>
          </div>

          {loading ? <p>Loading tickets...</p> : (
            <div className="ticket-list">
              {tickets.map((ticket) => (
                <article key={ticket.id} className="ticket-card">
                  <div className="ticket-top">
                    <div>
                      <h3>{ticket.title}</h3>
                      <p>{ticket.description}</p>
                    </div>
                    <span className="pill" style={{ backgroundColor: `${statusColors[ticket.status]}20`, color: statusColors[ticket.status] }}>{ticket.status}</span>
                  </div>
                  <div className="meta-row">
                    <span>Priority: {ticket.priority}</span>
                    <span>Owner: {ticket.assignedTo || 'Unassigned'}</span>
                  </div>
                  <div className="controls">
                    <button onClick={() => handleStateChange(ticket.id, 'InProgress')} disabled={ticket.status !== 'Open'}>Start</button>
                    <button onClick={() => handleStateChange(ticket.id, 'Resolved')} disabled={ticket.status !== 'InProgress'}>Resolve</button>
                    <button onClick={() => handleStateChange(ticket.id, 'Closed')} disabled={ticket.status !== 'Resolved'}>Close</button>
                    <button onClick={() => handleStateChange(ticket.id, 'Cancelled')} disabled={ticket.status === 'Closed' || ticket.status === 'Cancelled'}>Cancel</button>
                  </div>
                </article>
              ))}
            </div>
          )}
        </div>
      </section>
    </div>
  );
}

export default App;
