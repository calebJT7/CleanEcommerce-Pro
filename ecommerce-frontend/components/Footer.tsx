import Link from "next/link";
import { Cpu, Mail, MapPin, Phone, ShoppingBag } from "lucide-react";

export default function Footer() {
  return (
    <footer className="border-t border-border-subtle bg-bg-elevated mt-auto">
      <div className="absolute left-0 right-0 h-px bg-gradient-to-r from-transparent via-accent/30 to-transparent" />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-12">
          <div className="md:col-span-2">
            <Link href="/" className="flex items-center gap-3 mb-4">
              <div className="w-9 h-9 rounded-lg bg-accent-dim border border-accent/30 flex items-center justify-center">
                <ShoppingBag size={18} className="text-accent" />
              </div>
              <span className="font-bold text-lg text-text-primary tracking-tight">
                Clean<span className="text-accent">Ecommerce</span>
              </span>
            </Link>
            <p className="text-text-muted text-sm leading-relaxed max-w-sm mb-6">
              Tu destino premium para tecnología de alto rendimiento. Hardware, componentes
              y accesorios seleccionados para profesionales y entusiastas.
            </p>
            <div className="flex items-center gap-2 text-text-muted text-sm">
              <Cpu size={14} className="text-accent" />
              <span>Powered by next-gen commerce</span>
            </div>
          </div>

          <div>
            <h4 className="text-sm font-semibold text-text-primary uppercase tracking-wider mb-4">
              Navegación
            </h4>
            <ul className="space-y-3">
              {[
                { href: "/", label: "Catálogo" },
                { href: "/carrito", label: "Carrito" },
                { href: "/login", label: "Iniciar sesión" },
              ].map(({ href, label }) => (
                <li key={href}>
                  <Link
                    href={href}
                    className="text-sm text-text-muted hover:text-accent transition-colors duration-300"
                  >
                    {label}
                  </Link>
                </li>
              ))}
            </ul>
          </div>

          <div>
            <h4 className="text-sm font-semibold text-text-primary uppercase tracking-wider mb-4">
              Contacto
            </h4>
            <ul className="space-y-3">
              <li className="flex items-center gap-2 text-sm text-text-muted">
                <Mail size={14} className="text-accent flex-shrink-0" />
                soporte@cleanecommerce.com
              </li>
              <li className="flex items-center gap-2 text-sm text-text-muted">
                <Phone size={14} className="text-accent flex-shrink-0" />
                +54 11 4000-0000
              </li>
              <li className="flex items-center gap-2 text-sm text-text-muted">
                <MapPin size={14} className="text-accent flex-shrink-0" />
                Buenos Aires, Argentina
              </li>
            </ul>
          </div>
        </div>

        <div className="mt-12 pt-8 border-t border-border-subtle flex flex-col sm:flex-row justify-between items-center gap-4">
          <p className="text-xs text-text-muted">
            © {new Date().getFullYear()} CleanEcommerce. Todos los derechos reservados.
          </p>
          <div className="flex items-center gap-6">
            <span className="text-xs text-text-muted hover:text-accent transition-colors cursor-pointer">
              Términos
            </span>
            <span className="text-xs text-text-muted hover:text-accent transition-colors cursor-pointer">
              Privacidad
            </span>
            <span className="text-xs text-text-muted hover:text-accent transition-colors cursor-pointer">
              Soporte
            </span>
          </div>
        </div>
      </div>
    </footer>
  );
}
