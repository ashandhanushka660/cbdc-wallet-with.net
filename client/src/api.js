const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5005';

export const registerUser = async (userData) => {
  const response = await fetch(`${API_URL}/api/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(userData)
  });
  return response.json();
};

export const loginUser = async (credentials) => {
  const response = await fetch(`${API_URL}/api/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(credentials)
  });
  return response.json();
};

export const getHealth = async () => {
  const response = await fetch(`${API_URL}/health`);
  return response.json();
}

export const getAIScore = async (params = {}) => {
  const query = new URLSearchParams(params).toString();
  const response = await fetch(`${API_URL}/api/ai/score?${query}`);
  return response.json();
}

export const getUser = async (id) => {
  const response = await fetch(`${API_URL}/api/user/${id}`);
  return response.json();
}
