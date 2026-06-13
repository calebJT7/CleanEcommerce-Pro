"use client";

import { useEffect, useState } from "react";
import Navbar from "../components/Navbar";
import Hero from "../components/Hero";
import Footer from "../components/Footer";
import ProductCard from "../components/ProductCard";
import { productoService } from "../services/productoService";
import { Producto } from "../types";
import { Loader2, Package, Sparkles } from "lucide-react";

export default function Home() {
  const [productos, setProductos] = useState<Producto[]>([]);
  const [loading, setLoading] = useState(true);

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
    <div className="min-h-screen flex flex-col bg-bg-base">
      <Navbar />
      <Hero />

      <main id="catalogo" className="flex-grow max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16 md:py-20 w-full">
        <div className="mb-12 flex flex-col sm:flex-row sm:items-end sm:justify-between gap-4">
          <div>
            <div className="flex items-center gap-2 mb-3">
              <Sparkles size={16} className="text-accent" />
              <span className="text-xs font-semibold uppercase tracking-widest text-accent">
                Catálogo
              </span>
            </div>
            <h2 className="text-3xl md:text-4xl font-bold text-text-primary tracking-tight">
              Productos destacados
            </h2>
            <p className="text-text-secondary mt-2 max-w-lg">
              Explorá nuestra selección de hardware y tecnología de última generación.
            </p>
          </div>
          {!loading && productos.length > 0 && (
            <div className="flex items-center gap-2 px-4 py-2 rounded-xl border border-border-subtle bg-bg-surface text-sm text-text-muted">
              <Package size={14} className="text-accent" />
              {productos.length} producto{productos.length !== 1 ? "s" : ""} disponible{productos.length !== 1 ? "s" : ""}
            </div>
          )}
        </div>

        {loading ? (
          <div className="flex flex-col justify-center items-center h-64 gap-4">
            <Loader2 className="animate-spin text-accent" size={36} />
            <p className="text-text-muted text-sm">Cargando catálogo...</p>
          </div>
        ) : productos.length === 0 ? (
          <div className="text-center py-24 rounded-2xl border border-border-subtle bg-bg-card">
            <Package size={48} className="text-text-muted mx-auto mb-4" strokeWidth={1} />
            <p className="text-text-secondary text-lg font-medium">No hay productos disponibles</p>
            <p className="text-text-muted text-sm mt-2">Volvé pronto para descubrir novedades.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 md:gap-8">
            {productos.map((producto, index) => (
              <ProductCard key={producto.id} producto={producto} index={index} />
            ))}
          </div>
        )}
      </main>

      <Footer />
    </div>
  );
}
