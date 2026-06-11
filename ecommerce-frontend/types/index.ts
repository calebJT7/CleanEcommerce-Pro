export interface Producto {
    id: number;
    nombre: string;
    descripcion: string;
    precio: number;
    stock: number;
    imagenUrl: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
}

export interface PedidoAdmin {
    id: number;
    cliente: string;
    fecha: string;
    total: number;
    estado: string;
}

export interface Estadisticas {
    ingresosTotales: number;
    totalPedidos: number;
    pedidosPendientes: number;
}