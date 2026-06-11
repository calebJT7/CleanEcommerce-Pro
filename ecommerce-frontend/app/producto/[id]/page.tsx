"use client";

import { useCart } from "../../../context/CartContext";
import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import Link from "next/link";
import { productoService } from "../../../services/productoService";
import { Producto } from "../../../types";
import Navbar from "../../../components/Navbar";
import { ArrowLeft, Loader2, ShoppingCart } from "lucide-react";

export default function ProductoDetalle() {
  const params = useParams();
  const [producto, setProducto] = useState<Producto | null>(null);
  const [loading, setLoading] = useState(true);
  const { agregarAlCarrito } = useCart();

  useEffect(() => {
    const cargarProducto = async () => {
      try {
        const id = Number(params.id);
        if (id) {
          const data = await productoService.obtenerPorId(id);
          setProducto(data);
        }
      } catch (error) {
        console.error("Error al cargar el producto:", error);
      } finally {
        setLoading(false);
      }
    };

    cargarProducto();
  }, [params.id]);

  if (loading) {
    return (
      <div className="min-h-screen bg-[#F9FAFB]">
        <Navbar />
        <div className="flex justify-center items-center h-[calc(100vh-64px)]">
          <Loader2 className="animate-spin text-gray-400" size={32} />
        </div>
      </div>
    );
  }

  if (!producto) {
    return (
      <div className="min-h-screen bg-[#F9FAFB]">
        <Navbar />
        <div className="max-w-7xl mx-auto px-4 py-16 text-center">
          <h2 className="text-2xl font-medium text-gray-900">Producto no encontrado</h2>
          <Link href="/" className="text-blue-600 hover:underline mt-4 inline-block">
            Volver a la tienda
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-[#F9FAFB]">
      <Navbar />

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        {/* Botón de volver */}
        <Link 
          href="/" 
          className="inline-flex items-center text-sm font-medium text-gray-500 hover:text-gray-900 transition-colors mb-8"
        >
          <ArrowLeft size={16} className="mr-2" />
          Volver al catálogo
        </Link>

        <div className="bg-white rounded-2xl border border-gray-200 overflow-hidden shadow-sm">
          <div className="grid grid-cols-1 md:grid-cols-2">
            {/* Columna Izquierda: Imagen */}
            <div className="bg-gray-50 aspect-square md:aspect-auto border-b md:border-b-0 md:border-r border-gray-200 relative">
              {producto.imagenUrl ? (
                <img
                  src={producto.imagenUrl}
                  alt={producto.nombre}
                  className="w-full h-full object-cover object-center absolute inset-0"
                  onError={(e) => {
                    e.currentTarget.src = "https://placehold.co/600x600/f9fafb/9ca3af?text=Sin+Imagen";
                  }}
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center absolute inset-0 text-gray-400 text-sm font-medium uppercase tracking-wider">
                  Sin Imagen
                </div>
              )}
            </div>

            {/* Columna Derecha: Información */}
            <div className="p-8 md:p-12 flex flex-col justify-center">
              <div className="mb-2">
                <span className="inline-block px-3 py-1 bg-gray-100 text-gray-600 text-xs font-medium rounded-full mb-4">
                  Stock: {producto.stock} disponibles
                </span>
                <h1 className="text-3xl md:text-4xl font-semibold text-gray-900 tracking-tight leading-tight">
                  {producto.nombre}
                </h1>
              </div>
              
              <div className="text-3xl font-medium text-gray-900 mt-4 mb-8">
                ${producto.precio.toLocaleString()}
              </div>

              <p className="text-gray-500 leading-relaxed mb-10">
                {producto.descripcion}
              </p>

              <button 
                onClick={() => agregarAlCarrito(producto)} // <--- ¡Acá ocurre la magia!
                className="bg-black text-white px-8 py-3 rounded-lg hover:bg-gray-800 transition-all"
              >
                Agregar al carrito
              </button>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}