"use client";

import { useCart } from "../../../context/CartContext";
import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import Link from "next/link";
import { productoService } from "../../../services/productoService";
import { Producto } from "../../../types";
import Navbar from "../../../components/Navbar";
import Footer from "../../../components/Footer";
import Button from "@/components/ui/Button";
import Badge from "@/components/ui/Badge";
import {
  ArrowLeft,
  Loader2,
  ShoppingCart,
  Package,
  Check,
  Shield,
  Truck,
} from "lucide-react";

function getStockInfo(stock: number) {
  if (stock <= 0) return { variant: "error" as const, label: "Agotado", available: false };
  if (stock <= 5) return { variant: "warning" as const, label: `Solo ${stock} disponibles`, available: true };
  return { variant: "success" as const, label: `${stock} en stock`, available: true };
}

export default function ProductoDetalle() {
  const params = useParams();
  const [producto, setProducto] = useState<Producto | null>(null);
  const [loading, setLoading] = useState(true);
  const [agregado, setAgregado] = useState(false);
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

  const handleAgregar = () => {
    if (!producto) return;
    agregarAlCarrito(producto);
    setAgregado(true);
    setTimeout(() => setAgregado(false), 2000);
  };

  if (loading) {
    return (
      <div className="min-h-screen flex flex-col bg-bg-base">
        <Navbar />
        <div className="flex-grow flex flex-col justify-center items-center gap-4">
          <Loader2 className="animate-spin text-accent" size={36} />
          <p className="text-text-muted text-sm">Cargando producto...</p>
        </div>
      </div>
    );
  }

  if (!producto) {
    return (
      <div className="min-h-screen flex flex-col bg-bg-base">
        <Navbar />
        <div className="flex-grow max-w-7xl mx-auto px-4 py-16 text-center">
          <Package size={48} className="text-text-muted mx-auto mb-4" strokeWidth={1} />
          <h2 className="text-2xl font-semibold text-text-primary mb-2">Producto no encontrado</h2>
          <p className="text-text-muted mb-6">El producto que buscás no existe o fue removido.</p>
          <Link href="/">
            <Button variant="outline">Volver a la tienda</Button>
          </Link>
        </div>
        <Footer />
      </div>
    );
  }

  const stockInfo = getStockInfo(producto.stock);

  return (
    <div className="min-h-screen flex flex-col bg-bg-base">
      <Navbar />

      <main className="flex-grow max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 md:py-12 w-full">
        <Link
          href="/"
          className="inline-flex items-center text-sm font-medium text-text-muted hover:text-accent transition-colors duration-300 mb-8 group"
        >
          <ArrowLeft size={16} className="mr-2 group-hover:-translate-x-1 transition-transform" />
          Volver al catálogo
        </Link>

        <div className="glow-border rounded-2xl border border-border-subtle bg-bg-card overflow-hidden shadow-card animate-fade-in opacity-0" style={{ animationFillMode: "forwards" }}>
          <div className="grid grid-cols-1 lg:grid-cols-2">
            <div className="relative bg-bg-surface aspect-square lg:aspect-auto lg:min-h-[500px] overflow-hidden">
              <div className="absolute inset-0 bg-gradient-to-br from-accent/5 via-transparent to-transparent z-10 pointer-events-none" />

              {producto.imagenUrl ? (
                <img
                  src={producto.imagenUrl}
                  alt={producto.nombre}
                  className="w-full h-full object-cover object-center absolute inset-0"
                  onError={(e) => {
                    e.currentTarget.src = "https://placehold.co/600x600/1a1a1f/6b6b78?text=Sin+Imagen";
                  }}
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center absolute inset-0">
                  <Package size={64} className="text-text-muted" strokeWidth={1} />
                </div>
              )}

              {producto.precio >= 50000 && (
                <div className="absolute top-4 left-4 z-20">
                  <Badge variant="accent">Premium</Badge>
                </div>
              )}
            </div>

            <div className="p-8 md:p-10 lg:p-12 flex flex-col justify-center">
              <div className="mb-6">
                <Badge variant={stockInfo.variant} className="mb-4">
                  {stockInfo.label}
                </Badge>
                <h1 className="text-3xl md:text-4xl lg:text-5xl font-bold text-text-primary tracking-tight leading-tight">
                  {producto.nombre}
                </h1>
              </div>

              <div className="flex items-baseline gap-3 mb-8">
                <span className="text-4xl font-bold text-gradient-accent">
                  ${producto.precio.toLocaleString()}
                </span>
                <span className="text-text-muted text-sm">ARS</span>
              </div>

              <p className="text-text-secondary leading-relaxed mb-8 text-base md:text-lg">
                {producto.descripcion}
              </p>

              <div className="grid grid-cols-2 gap-3 mb-8">
                {[
                  { icon: Truck, label: "Envío gratis" },
                  { icon: Shield, label: "Garantía 2 años" },
                ].map(({ icon: Icon, label }) => (
                  <div
                    key={label}
                    className="flex items-center gap-2 p-3 rounded-xl border border-border-subtle bg-bg-surface text-sm text-text-muted"
                  >
                    <Icon size={16} className="text-accent flex-shrink-0" />
                    {label}
                  </div>
                ))}
              </div>

              <Button
                onClick={handleAgregar}
                disabled={!stockInfo.available}
                size="lg"
                className="w-full sm:w-auto"
                variant={agregado ? "secondary" : "primary"}
              >
                {agregado ? (
                  <>
                    <Check size={18} />
                    Agregado al carrito
                  </>
                ) : (
                  <>
                    <ShoppingCart size={18} />
                    Agregar al carrito
                  </>
                )}
              </Button>
            </div>
          </div>
        </div>
      </main>

      <Footer />
    </div>
  );
}
