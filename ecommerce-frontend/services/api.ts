import axios from 'axios';

const rawBaseURL = process.env.NEXT_PUBLIC_API_URL || 'https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/api';
const normalizedBaseURL = rawBaseURL.replace(/\/+$|\/api$/g, '');
const baseURL = normalizedBaseURL.endsWith('/api') ? normalizedBaseURL : `${normalizedBaseURL}/api`;

export const api = axios.create({
    // Vercel buscará esta variable. Si no la encuentra, usa la de Azure como respaldo
    baseURL,
    headers: {
        'Content-Type': 'application/json',
    },
});
// Interceptor: Antes de que salga cualquier petición, le inyectamos el Token JWT si existe
api.interceptors.request.use((config) => {
    // En Next.js (lado del cliente), leemos el token de localStorage
    if (typeof window !== 'undefined') {
        const token = localStorage.getItem('authToken')?.trim();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
    }
    return config;
}, (error) => {
    return Promise.reject(error);
});