const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5005';

export const registerUser = async (userData) => {
    const response = await fetch(`${API_URL}/api/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(userData)
    });
    return response.json();
};

export const getHealth = async () => {
    const response = await fetch(`${API_URL}/health`);
    return response.json();
}
