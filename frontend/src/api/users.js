const API_ROOT = import.meta.env.VITE_API_URL ?? 'http://localhost:5195'

export async function fetchUsers() {
  const response = await fetch(`${API_ROOT}/api/users`)
  if (!response.ok) {
    throw new Error('Failed to load users')
  }
  return response.json()
}
