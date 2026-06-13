"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import Navbar from "../components/Navbar";
import { productoService } from "../services/productoService";
import { Producto } from "../types";
import { Loader2 } from "lucide-react";

export default function Home() {
  const [productos, setProductos] = useState<Producto[]>([]);
  const [loading, setLoading] = useState(true);

  // Apenas carga la página, llama a tu API de C#
  useEffect(() => {
    const cargarProductos = async () => {
      try {
        const data = await productoService.obtenerTodos();
        setProductos(data);
      } catch (error) {
        console.error("Error al cargar el catálogo:", error);
      } finally {
        setLoading(false);
      }
    };

    cargarProductos();
  }, []);

  return (
    <div className="min-h-screen bg-[#F9FAFB]">
      <Navbar />

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div className="mb-10">
          <h1 className="text-3xl font-semibold text-gray-900 tracking-tight">Catálogo de Productos</h1>
          <p className="text-gray-500 mt-2">Encuentra la mejor tecnología al mejor precio.</p>
        </div>

        {loading ? (
          <div className="flex justify-center items-center h-64">
            <Loader2 className="animate-spin text-gray-400" size={32} />
          </div>
        ) : productos.length === 0 ? (
          <div className="text-center py-20 bg-white rounded-xl border border-gray-200">
            <p className="text-gray-500 text-lg">No hay productos disponibles por el momento.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
            {productos.map((producto) => (
              <div key={producto.id} className="bg-white rounded-xl border border-gray-200 overflow-hidden hover:shadow-lg transition-all duration-300 group flex flex-col">
                <div className="aspect-[4/3] bg-gray-50 border-b border-gray-100 relative overflow-hidden">
                  {producto.imagenUrl ? (
                    <img 
                      src={producto.imagenUrl} 
                      alt={producto.nombre}
                      className="w-full h-full object-cover object-center group-hover:scale-105 transition-transform duration-500"
                      onError={(e) => { e.currentTarget.src = "https://placehold.co/400x300/f9fafb/9ca3af?text=Sin+Imagen"; }}
                    />
                  ) : (
                    <div className="w-full h-full flex items-center justify-center text-gray-400 text-sm font-medium uppercase tracking-wider">
                      Sin Imagen
                    </div>
                  )}
                </div>
                
                <div className="p-5 flex flex-col flex-grow">
                  <div className="flex justify-between items-start mb-2 gap-4">
                    <h3 className="font-medium text-gray-900 leading-tight">{producto.nombre}</h3>
                    <span className="font-semibold text-gray-900 bg-gray-50 px-2 py-1 rounded-md text-sm border border-gray-100">
                      ${producto.precio.toLocaleString()}
                    </span>
                  </div>
                  
                  <p className="text-sm text-gray-500 line-clamp-2 mb-6 flex-grow">
                    {producto.descripcion}
                  </p>

                  <Link 
                    href={`/producto/${producto.id}`}
                    className="w-full flex justify-center py-2.5 px-4 bg-white border border-gray-200 text-sm font-medium text-gray-900 rounded-lg hover:border-gray-900 transition-colors"
                  >
                    Ver Detalles
                  </Link>
                </div>
              </div>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}