"use client";

import { useState } from "react";
import Link from "next/link";
import Navbar from "../../components/Navbar";
import Footer from "../../components/Footer";
import Button from "@/components/ui/Button";
import Badge from "@/components/ui/Badge";
import { useCart } from "../../context/CartContext";
import { pedidoService } from "../../services/pedidoService";
import {
  ShoppingBag,
  CheckCircle,
  ArrowRight,
  CreditCard,
  Package,
} from "lucide-react";

export default function CarritoPage() {
  const { carrito, vaciarCarrito } = useCart();
  const [procesando, setProcesando] = useState(false);
  const [compraExitosa, setCompraExitosa] = useState(false);
  const [error, setError] = useState("");

  const total = carrito.reduce((suma, item) => suma + item.precio * item.cantidad, 0);
  const subtotal = total;

  const handleFinalizarCompra = async () => {
    setProcesando(true);
    setError("");

    try {
      await pedidoService.crearPedido(carrito, total);
      vaciarCarrito();
      setCompraExitosa(true);
    } catch (err) {
      console.error(err);
      setError("Hubo un problema al procesar tu compra. Por favor, intenta de nuevo.");
    } finally {
      setProcesando(false);
    }
  };

  if (compraExitosa) {
    return (
      <div className="min-h-screen flex flex-col bg-bg-base">
        <Navbar />
        <main className="flex-grow flex items-center justify-center px-4 py-20">
          <div className="max-w-lg w-full text-center animate-fade-in-up opacity-0" style={{ animationFillMode: "forwards" }}>
            <div className="glow-border rounded-2xl border border-border-subtle bg-bg-card p-12 shadow-glow-md">
              <div className="w-20 h-20 rounded-full bg-accent-dim border border-accent/30 flex items-center justify-center mx-auto mb-6">
                <CheckCircle size={40} className="text-accent" />
              </div>
              <h1 className="text-3xl font-bold text-text-primary mb-3">¡Compra confirmada!</h1>
              <p className="text-text-secondary mb-8 leading-relaxed">
                Tu pedido ha sido procesado correctamente. En breve nos pondremos en contacto.
              </p>
              <Link href="/">
                <Button size="lg" className="group">
                  Seguir comprando
                  <ArrowRight size={18} className="group-hover:translate-x-1 transition-transform" />
                </Button>
              </Link>
            </div>
          </div>
        </main>
        <Footer />
      </div>
    );
  }

  return (
    <div className="min-h-screen flex flex-col bg-bg-base">
      <Navbar />

      <main className="flex-grow max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-12 md:py-16 w-full">
        <div className="mb-10">
          <div className="flex items-center gap-2 mb-3">
            <ShoppingBag size={16} className="text-accent" />
            <span className="text-xs font-semibold uppercase tracking-widest text-accent">
              Checkout
            </span>
          </div>
          <h1 className="text-3xl md:text-4xl font-bold text-text-primary tracking-tight">
            Tu carrito
          </h1>
          {carrito.length > 0 && (
            <p className="text-text-muted mt-2">
              {carrito.reduce((sum, item) => sum + item.cantidad, 0)} artículo
              {carrito.reduce((sum, item) => sum + item.cantidad, 0) !== 1 ? "s" : ""} en tu pedido
            </p>
          )}
        </div>

        {error && (
          <div className="mb-6 p-4 bg-error-dim text-error rounded-xl border border-error/30">
            {error}
          </div>
        )}

        {carrito.length === 0 ? (
          <div className="rounded-2xl border border-border-subtle bg-bg-card p-16 text-center flex flex-col items-center">
            <div className="w-16 h-16 rounded-2xl bg-bg-surface border border-border-subtle flex items-center justify-center mb-6">
              <ShoppingBag size={32} className="text-text-muted" strokeWidth={1.5} />
            </div>
            <h2 className="text-xl font-semibold text-text-primary mb-2">Tu carrito está vacío</h2>
            <p className="text-text-muted text-sm mb-8 max-w-sm">
              Explorá nuestro catálogo y encontrá la tecnología que necesitás.
            </p>
            <Link href="/">
              <Button size="lg">Volver a la tienda</Button>
            </Link>
          </div>
        ) : (
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            <div className="lg:col-span-2">
              <div className="rounded-2xl border border-border-subtle bg-bg-card overflow-hidden">
                <ul className="divide-y divide-border-subtle">
                  {carrito.map((item) => (
                    <li
                      key={item.id}
                      className="p-5 md:p-6 flex items-center justify-between gap-4 hover:bg-bg-hover/50 transition-colors duration-300"
                    >
                      <div className="flex items-center gap-4 md:gap-6 min-w-0">
                        <div className="w-20 h-20 md:w-24 md:h-24 rounded-xl bg-bg-surface border border-border-subtle overflow-hidden flex-shrink-0">
                          {item.imagenUrl ? (
                            <img
                              src={item.imagenUrl}
                              alt={item.nombre}
                              className="w-full h-full object-cover"
                            />
                          ) : (
                            <div className="w-full h-full flex items-center justify-center">
                              <Package size={20} className="text-text-muted" />
                            </div>
                          )}
                        </div>
                        <div className="min-w-0">
                          <h3 className="text-base md:text-lg font-semibold text-text-primary truncate">
                            {item.nombre}
                          </h3>
                          <div className="flex items-center gap-2 mt-2">
                            <Badge variant="outline">x{item.cantidad}</Badge>
                            <span className="text-text-muted text-sm">
                              ${item.precio.toLocaleString()} c/u
                            </span>
                          </div>
                        </div>
                      </div>
                      <span className="text-lg font-bold text-accent flex-shrink-0">
                        ${(item.precio * item.cantidad).toLocaleString()}
                      </span>
                    </li>
                  ))}
                </ul>
              </div>
            </div>

            <div className="lg:col-span-1">
              <div className="glow-border sticky top-24 rounded-2xl border border-border-subtle bg-bg-card p-6 shadow-card">
                <h3 className="text-lg font-semibold text-text-primary mb-6 flex items-center gap-2">
                  <CreditCard size={18} className="text-accent" />
                  Resumen del pedido
                </h3>

                <div className="space-y-3 mb-6">
                  <div className="flex justify-between text-sm">
                    <span className="text-text-muted">Subtotal</span>
                    <span className="text-text-secondary">${subtotal.toLocaleString()}</span>
                  </div>
                  <div className="flex justify-between text-sm">
                    <span className="text-text-muted">Envío</span>
                    <span className="text-accent font-medium">Gratis</span>
                  </div>
                  <div className="h-px bg-border-subtle" />
                  <div className="flex justify-between">
                    <span className="text-text-secondary font-medium">Total</span>
                    <span className="text-2xl font-bold text-text-primary">
                      ${total.toLocaleString()}
                    </span>
                  </div>
                </div>

                <Button
                  onClick={handleFinalizarCompra}
                  loading={procesando}
                  className="w-full"
                  size="lg"
                >
                  {procesando ? "Procesando..." : "Finalizar compra"}
                </Button>

                <p className="text-xs text-text-muted text-center mt-4">
                  Pago seguro · Confirmación inmediata
                </p>
              </div>
            </div>
          </div>
        )}
      </main>

      <Footer />
    </div>
  );
}
