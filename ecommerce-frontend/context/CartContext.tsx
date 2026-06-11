"use client";

import { createContext, useContext, useState, useEffect, ReactNode } from "react";
import { Producto } from "../types";

// Le agregamos la cantidad al producto
export interface CartItem extends Producto {
  cantidad: number;
}

interface CartContextType {
  carrito: CartItem[];
  agregarAlCarrito: (producto: Producto) => void;
  cantidadTotal: number;
  vaciarCarrito: () => void;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export function CartProvider({ children }: { children: ReactNode }) {
  const [carrito, setCarrito] = useState<CartItem[]>([]);

  // Cuando arranca la app, buscamos si había algo guardado
  useEffect(() => {
    const carritoGuardado = localStorage.getItem("carrito");
    if (carritoGuardado) {
      setCarrito(JSON.parse(carritoGuardado));
    }
  }, []);

  // Función para agregar o sumar productos
  const agregarAlCarrito = (producto: Producto) => {
    setCarrito((carritoActual) => {
      const productoExistente = carritoActual.find((item) => item.id === producto.id);
      
      let nuevoCarrito;
      if (productoExistente) {
        nuevoCarrito = carritoActual.map((item) =>
          item.id === producto.id ? { ...item, cantidad: item.cantidad + 1 } : item
        );
      } else {
        nuevoCarrito = [...carritoActual, { ...producto, cantidad: 1 }];
      }

      localStorage.setItem("carrito", JSON.stringify(nuevoCarrito));
      return nuevoCarrito;
    });
  };

  // Función para vaciar el carrito después de comprar
  const vaciarCarrito = () => {
    setCarrito([]);
    localStorage.removeItem("carrito");
  };

  // Calculamos cuántos ítems hay en total para la barra superior
  const cantidadTotal = carrito.reduce((total, item) => total + item.cantidad, 0);

  return (
    <CartContext.Provider value={{ carrito, agregarAlCarrito, cantidadTotal, vaciarCarrito }}>
      {children}
    </CartContext.Provider>
  );
}

// Un "hook" personalizado para usar el carrito en cualquier lado
export const useCart = () => {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error("useCart debe usarse dentro de un CartProvider");
  }
  return context;
};