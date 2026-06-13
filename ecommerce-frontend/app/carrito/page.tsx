"use client";

import { useState } from "react";
import Link from "next/link";
import Navbar from "../../components/Navbar";
import { useCart } from "../../context/CartContext";
import { pedidoService } from "../../services/pedidoService";
import { Trash2, ShoppingBag, CheckCircle, Loader2 } from "lucide-react";

export default function CarritoPage() {
  const { carrito, vaciarCarrito } = useCart();
  const [procesando, setProcesando] = useState(false);
  const [compraExitosa, setCompraExitosa] = useState(false);
  const [error, setError] = useState("");

  const total = carrito.reduce((suma, item) => suma + item.precio * item.cantidad, 0);

  const handleFinalizarCompra = async () => {
    setProcesando(true);
    setError("");

    try {
      // 1. Envia el pedido a C#
      await pedidoService.crearPedido(carrito, total);
      
      // 2. Vacia el carrito local
      vaciarCarrito();
      
      // 3. Muestra la pantalla de éxito
      setCompraExitosa(true);
    } catch (err) {
      console.error(err);
      setError("Hubo un problema al procesar tu compra. Por favor, intenta de nuevo.");
    } finally {
      setProcesando(false);
    }
  };

  // --- PANTALLA DE ÉXITO ---
  if (compraExitosa) {
    return (
      <div className="min-h-screen bg-[#F9FAFB]">
        <Navbar />
        <main className="max-w-4xl mx-auto px-4 py-20 text-center">
          <div className="bg-white rounded-xl border border-gray-200 p-12 flex flex-col items-center shadow-sm">
            <CheckCircle size={64} className="text-green-500 mb-6" />
            <h1 className="text-3xl font-bold text-gray-900 mb-2">¡Compra confirmada!</h1>
            <p className="text-gray-500 mb-8 text-lg">Tu pedido ha sido procesado correctamente. En breve nos pondremos en contacto.</p>
            <Link href="/" className="bg-black text-white px-8 py-3 rounded-lg font-medium hover:bg-gray-800 transition-colors">
              Seguir comprando
            </Link>
          </div>
        </main>
      </div>
    );
  }

  // --- PANTALLA DEL CARRITO (Con el botón funcional) ---
  return (
    <div className="min-h-screen bg-[#F9FAFB]">
      <Navbar />
      
      <main className="max-w-4xl mx-auto px-4 py-12">
        <h1 className="text-3xl font-semibold text-gray-900 mb-8">Tu Carrito</h1>

        {error && (
          <div className="mb-6 p-4 bg-red-50 text-red-700 rounded-lg border border-red-200">
            {error}
          </div>
        )}

        {carrito.length === 0 ? (
          <div className="bg-white rounded-xl border border-gray-200 p-12 text-center flex flex-col items-center">
            <ShoppingBag size={48} className="text-gray-300 mb-4" />
            <h2 className="text-xl font-medium text-gray-900 mb-2">Tu carrito está vacío</h2>
            <Link href="/" className="bg-black text-white px-6 py-3 rounded-lg font-medium mt-6 hover:bg-gray-800 transition-colors">
              Volver a la tienda
            </Link>
          </div>
        ) : (
          <div className="bg-white rounded-xl border border-gray-200 overflow-hidden shadow-sm">
            {/* Lista de productos  */}
            <ul className="divide-y divide-gray-200">
              {carrito.map((item) => (
                <li key={item.id} className="p-6 flex items-center justify-between">
                  <div className="flex items-center gap-6">
                    <div className="w-24 h-24 bg-gray-100 rounded-md overflow-hidden">
                      {item.imagenUrl ? <img src={item.imagenUrl} className="w-full h-full object-cover" /> : <div className="w-full h-full text-xs flex items-center text-gray-400">Sin foto</div>}
                    </div>
                    <div>
                      <h3 className="text-lg font-medium text-gray-900">{item.nombre}</h3>
                      <p className="text-gray-500 text-sm mt-1">Cantidad: {item.cantidad}</p>
                    </div>
                  </div>
                  <span className="text-lg font-semibold">${(item.precio * item.cantidad).toLocaleString()}</span>
                </li>
              ))}
            </ul>
            
            <div className="bg-gray-50 p-6 border-t border-gray-200 flex flex-col sm:flex-row justify-between items-center gap-4">
              <div>
                <p className="text-sm text-gray-500">Total a pagar</p>
                <p className="text-2xl font-bold text-gray-900">${total.toLocaleString()}</p>
              </div>
              
              {/* BOTÓN MÁGICO */}
              <button 
                onClick={handleFinalizarCompra}
                disabled={procesando}
                className="w-full sm:w-auto bg-blue-600 text-white px-8 py-3 rounded-lg font-medium hover:bg-blue-700 transition-all shadow-sm flex items-center justify-center disabled:bg-blue-400 gap-2"
              >
                {procesando ? (
                  <><Loader2 size={18} className="animate-spin" /> Procesando...</>
                ) : (
                  "Finalizar Compra"
                )}
              </button>
            </div>
          </div>
        )}
      </main>
    </div>
  );
}
