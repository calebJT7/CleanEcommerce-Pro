import { api } from './api';
import { CartItem } from '../context/CartContext';

export const pedidoService = {
  crearPedido: async (carrito: CartItem[], total: number) => {
    // Guarda el token de sesión si existe y es válido
    const token = localStorage.getItem("authToken")?.trim();
    const validToken = token && token !== "undefined" && token !== "null" ? token : null;

    if (!validToken) {
      throw new Error("Necesitas iniciar sesión para completar la compra.");
    }

    // Arma el paquete que espera el backend
    const payload = {
      productoIds: carrito.flatMap(item => Array.from({ length: item.cantidad }, () => item.id))
    };

    const response = await api.post('/Pedidos', payload, {
      headers: { Authorization: `Bearer ${validToken}` }
    });
    
    return response.data;
  }
};