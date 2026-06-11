import { api } from './api';
import { Producto } from '../types';

export const productoService = {
    // Traer todos los productos
    obtenerTodos: async (): Promise<Producto[]> => {
        const response = await api.get<Producto[]>('/productos');
        return response.data;
    },

    // Traer un producto por ID (para la página de detalle)
    obtenerPorId: async (id: number): Promise<Producto> => {
        const response = await api.get<Producto>(`/productos/${id}`);
        return response.data;
    }
};