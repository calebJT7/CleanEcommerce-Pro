"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { ShoppingBag, LogOut } from "lucide-react";
import { useRouter } from "next/navigation";
import { useCart } from "../context/CartContext";

export default function Navbar() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const router = useRouter();
  const { cantidadTotal } = useCart();

  // Apenas carga el menú, revisamos si hay un token guardado
  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (token) {
      setIsLoggedIn(true);
    }
  }, []);

  // Función para borrar el token y cerrar la sesión
  const handleLogout = () => {
    localStorage.removeItem("authToken");
    setIsLoggedIn(false);
    router.push("/"); // Recargamos el inicio
  };

  return (
    <header className="bg-white border-b border-gray-200 sticky top-0 z-10">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <Link href="/" className="flex items-center gap-2">
          <div className="bg-black text-white p-1.5 rounded-md">
            <ShoppingBag size={18} strokeWidth={2.5} />
          </div>
          <span className="font-semibold text-gray-900 tracking-tight">CleanEcommerce</span>
        </Link>
        <nav className="flex items-center gap-6">
          {isLoggedIn ? (
            <button
              onClick={handleLogout}
              className="flex items-center gap-2 text-sm font-medium text-gray-500 hover:text-black transition-colors"
            >
              <LogOut size={16} />
              Cerrar sesión
            </button>
          ) : (
            <Link href="/login" className="text-sm font-medium text-gray-500 hover:text-black transition-colors">
              Iniciar sesión
            </Link>
          )}
          <Link href="/carrito" className="text-sm font-medium bg-black text-white px-4 py-2 rounded-md hover:bg-gray-800 transition-all shadow-sm">
  Carrito ({cantidadTotal})
</Link>
        </nav>
      </div>
    </header>
  );
}