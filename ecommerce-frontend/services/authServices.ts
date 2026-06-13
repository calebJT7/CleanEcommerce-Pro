import { api } from './api';
import { LoginRequest, LoginResponse } from '../types';

export const authService = {
    login: async (credenciales: LoginRequest): Promise<LoginResponse> => {

        const response = await api.post<LoginResponse>('/usuarios/login', credenciales);
        return response.data;
    }
};