import Link from "next/link";
import { ArrowRight, Cpu, Shield, Zap } from "lucide-react";
import Button from "./ui/Button";

export default function Hero() {
  return (
    <section className="relative overflow-hidden border-b border-border-subtle">
      <div className="absolute inset-0 mesh-gradient" />
      <div className="absolute inset-0 grid-bg animate-pulse-glow" />

      <div className="absolute top-1/4 left-1/4 w-96 h-96 bg-accent/5 rounded-full blur-3xl" />
      <div className="absolute bottom-0 right-1/4 w-80 h-80 bg-accent/8 rounded-full blur-3xl" />

      <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-20 md:py-28 lg:py-32">
        <div className="max-w-3xl">
          <div className="animate-fade-in-up opacity-0">
            <span className="inline-flex items-center gap-2 px-4 py-1.5 rounded-full border border-accent/30 bg-accent-dim text-accent text-xs font-semibold uppercase tracking-widest mb-6">
              <span className="w-1.5 h-1.5 rounded-full bg-accent-bright animate-pulse" />
              Tecnología de última generación
            </span>
          </div>

          <h1 className="animate-fade-in-up stagger-1 opacity-0 text-4xl sm:text-5xl md:text-6xl lg:text-7xl font-bold tracking-tight leading-[1.1] mb-6">
            <span className="text-gradient">El futuro</span>
            <br />
            <span className="text-text-primary">está en tus manos</span>
          </h1>

          <p className="animate-fade-in-up stagger-2 opacity-0 text-lg md:text-xl text-text-secondary max-w-xl leading-relaxed mb-10">
            Hardware premium, componentes de alto rendimiento y la mejor selección
            tecnológica. Diseñado para quienes exigen excelencia.
          </p>

          <div className="animate-fade-in-up stagger-3 opacity-0 flex flex-wrap gap-4 mb-16">
            <a href="#catalogo">
              <Button size="lg" className="group">
                Explorar catálogo
                <ArrowRight size={18} className="group-hover:translate-x-1 transition-transform" />
              </Button>
            </a>
            <Link href="/login">
              <Button variant="outline" size="lg">
                Acceso corporativo
              </Button>
            </Link>
          </div>

          <div className="animate-fade-in-up stagger-4 opacity-0 grid grid-cols-1 sm:grid-cols-3 gap-4 md:gap-6">
            {[
              { icon: Zap, label: "Envío express", desc: "Entrega prioritaria" },
              { icon: Shield, label: "Garantía total", desc: "2 años de cobertura" },
              { icon: Cpu, label: "Tech premium", desc: "Marcas líderes" },
            ].map(({ icon: Icon, label, desc }) => (
              <div
                key={label}
                className="flex items-center gap-4 p-4 rounded-xl border border-border-subtle bg-bg-surface/50 hover:border-accent/20 transition-colors duration-300"
              >
                <div className="flex-shrink-0 w-10 h-10 rounded-lg bg-accent-dim border border-accent/20 flex items-center justify-center">
                  <Icon size={18} className="text-accent" />
                </div>
                <div>
                  <p className="text-sm font-semibold text-text-primary">{label}</p>
                  <p className="text-xs text-text-muted">{desc}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      <div className="absolute bottom-0 left-0 right-0 h-px bg-gradient-to-r from-transparent via-accent/50 to-transparent" />
    </section>
  );
}
