import { useState } from 'react'

export default function TicketForm({ onCreate, onError }) {
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [priority, setPriority] = useState('Medium')
  const [assignedTo, setAssignedTo] = useState('')

  async function handleSubmit(event) {
    event.preventDefault()
    if (!title.trim() || !description.trim()) {
      onError('Title and description are required.')
      return
    }
    try {
      await onCreate({
        title: title.trim(),
        description: description.trim(),
        priority,
        assignedTo: assignedTo.trim() || null,
      })
      setTitle('')
      setDescription('')
      setAssignedTo('')
      setPriority('Medium')
    } catch (err) {
      onError(err.message)
    }
  }

  return (
    <form className="ticket-form" onSubmit={handleSubmit}>
      <h2>Create Ticket</h2>
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
        <input value={assignedTo} onChange={(e) => setAssignedTo(e.target.value)} />
      </label>
      <button type="submit">Create</button>
    </form>
  )
}
