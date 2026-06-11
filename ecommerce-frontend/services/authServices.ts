import { api } from './api';
import { LoginRequest, LoginResponse } from '../types';

export const authService = {
    login: async (credenciales: LoginRequest): Promise<LoginResponse> => {
        // OJO: Asegúrate de que esta ruta coincida con el endpoint de tu API. 
        // Si tu backend usa /api/usuarios/login, cambialo aquí abajo:
        const response = await api.post<LoginResponse>('/usuarios/login', credenciales);
        return response.data;
    }
};