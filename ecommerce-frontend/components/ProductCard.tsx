import Link from "next/link";
import { ArrowRight, Package } from "lucide-react";
import { Producto } from "@/types";
import Badge from "./ui/Badge";
import { cn } from "@/lib/cn";

interface ProductCardProps {
  producto: Producto;
  index?: number;
}

function getStockBadge(stock: number) {
  if (stock <= 0) return { variant: "error" as const, label: "Agotado" };
  if (stock <= 5) return { variant: "warning" as const, label: "Últimas unidades" };
  return { variant: "success" as const, label: "En stock" };
}

export default function ProductCard({ producto, index = 0 }: ProductCardProps) {
  const stockBadge = getStockBadge(producto.stock);

  return (
    <div
      className={cn(
        "group glow-border card-shine flex flex-col rounded-2xl border border-border-subtle bg-bg-card overflow-hidden",
        "hover:border-accent/30 hover:shadow-glow-sm transition-all duration-500",
        "animate-fade-in-up opacity-0"
      )}
      style={{ animationDelay: `${0.05 * index}s`, animationFillMode: "forwards" }}
    >
      <div className="aspect-[4/3] bg-bg-surface relative overflow-hidden">
        <div className="absolute inset-0 bg-gradient-to-t from-bg-card via-transparent to-transparent z-10 opacity-60" />

        {producto.imagenUrl ? (
          <img
            src={producto.imagenUrl}
            alt={producto.nombre}
            className="w-full h-full object-cover object-center group-hover:scale-110 transition-transform duration-700"
            onError={(e) => {
              e.currentTarget.src = "https://placehold.co/400x300/1a1a1f/6b6b78?text=Sin+Imagen";
            }}
          />
        ) : (
          <div className="w-full h-full flex items-center justify-center text-text-muted">
            <Package size={40} strokeWidth={1} />
          </div>
        )}

        <div className="absolute top-3 left-3 z-20">
          <Badge variant={stockBadge.variant}>{stockBadge.label}</Badge>
        </div>

        {producto.precio >= 50000 && (
          <div className="absolute top-3 right-3 z-20">
            <Badge variant="accent">Premium</Badge>
          </div>
        )}
      </div>

      <div className="p-5 flex flex-col flex-grow">
        <div className="flex justify-between items-start mb-3 gap-3">
          <h3 className="font-semibold text-text-primary leading-tight group-hover:text-accent-bright transition-colors duration-300">
            {producto.nombre}
          </h3>
          <span className="flex-shrink-0 font-bold text-accent text-sm bg-accent-dim px-2.5 py-1 rounded-lg border border-accent/20">
            ${producto.precio.toLocaleString()}
          </span>
        </div>

        <p className="text-sm text-text-muted line-clamp-2 mb-5 flex-grow leading-relaxed">
          {producto.descripcion}
        </p>

        <Link
          href={`/producto/${producto.id}`}
          className={cn(
            "w-full flex justify-center items-center gap-2 py-2.5 px-4",
            "text-sm font-medium text-text-secondary rounded-xl",
            "border border-border-default bg-bg-surface",
            "group-hover:border-accent/40 group-hover:text-accent group-hover:bg-accent-dim",
            "transition-all duration-300"
          )}
        >
          Ver detalles
          <ArrowRight size={14} className="group-hover:translate-x-1 transition-transform" />
        </Link>
      </div>
    </div>
  );
}
