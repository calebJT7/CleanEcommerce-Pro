"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { LogOut, ShoppingBag, User } from "lucide-react";
import { useRouter, usePathname } from "next/navigation";
import { useCart } from "../context/CartContext";
import { cn } from "@/lib/cn";

export default function Navbar() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [scrolled, setScrolled] = useState(false);
  const router = useRouter();
  const pathname = usePathname();
  const { cantidadTotal } = useCart();

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (token) setIsLoggedIn(true);
  }, []);

  useEffect(() => {
    const handleScroll = () => setScrolled(window.scrollY > 20);
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("authToken");
    setIsLoggedIn(false);
    router.push("/");
  };

  return (
    <header
      className={cn(
        "sticky top-0 z-50 transition-all duration-500",
        scrolled
          ? "glass-strong border-b border-border-subtle shadow-card"
          : "bg-transparent border-b border-transparent"
      )}
    >
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <Link href="/" className="flex items-center gap-3 group">
          <div className="relative w-9 h-9 rounded-lg bg-accent-dim border border-accent/30 flex items-center justify-center group-hover:shadow-glow-sm transition-all duration-300">
            <ShoppingBag size={18} className="text-accent" strokeWidth={2} />
            <div className="absolute inset-0 rounded-lg bg-accent/10 opacity-0 group-hover:opacity-100 transition-opacity" />
          </div>
          <span className="font-bold text-text-primary tracking-tight">
            Clean<span className="text-accent">Ecommerce</span>
          </span>
        </Link>

        <nav className="flex items-center gap-2 sm:gap-4">
          <Link
            href="/"
            className={cn(
              "hidden sm:inline-flex px-3 py-2 text-sm font-medium rounded-lg transition-all duration-300",
              pathname === "/"
                ? "text-accent bg-accent-dim"
                : "text-text-secondary hover:text-text-primary hover:bg-bg-hover"
            )}
          >
            Catálogo
          </Link>

          {isLoggedIn ? (
            <button
              onClick={handleLogout}
              className="flex items-center gap-2 px-3 py-2 text-sm font-medium text-text-secondary hover:text-text-primary hover:bg-bg-hover rounded-lg transition-all duration-300"
            >
              <LogOut size={16} />
              <span className="hidden sm:inline">Cerrar sesión</span>
            </button>
          ) : (
            <Link
              href="/login"
              className={cn(
                "flex items-center gap-2 px-3 py-2 text-sm font-medium rounded-lg transition-all duration-300",
                pathname === "/login"
                  ? "text-accent bg-accent-dim"
                  : "text-text-secondary hover:text-text-primary hover:bg-bg-hover"
              )}
            >
              <User size={16} />
              <span className="hidden sm:inline">Iniciar sesión</span>
            </Link>
          )}

          <Link
            href="/carrito"
            className={cn(
              "relative flex items-center gap-2 px-4 py-2 text-sm font-semibold rounded-xl transition-all duration-300",
              "bg-accent text-bg-base hover:bg-accent-bright shadow-glow-sm hover:shadow-glow-md",
              "border border-accent/50"
            )}
          >
            <ShoppingBag size={16} />
            <span>Carrito</span>
            {cantidadTotal > 0 && (
              <span className="absolute -top-1.5 -right-1.5 min-w-[20px] h-5 flex items-center justify-center px-1 text-[10px] font-bold rounded-full bg-bg-base text-accent border border-accent/50">
                {cantidadTotal}
              </span>
            )}
          </Link>
        </nav>
      </div>

      <div className="h-px bg-gradient-to-r from-transparent via-accent/20 to-transparent" />
    </header>
  );
}
