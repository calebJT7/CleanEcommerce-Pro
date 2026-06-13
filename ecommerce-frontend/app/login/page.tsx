"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { authService } from "../../services/authServices";
import { Lock, ShoppingBag } from "lucide-react";
import Input from "@/components/ui/Input";
import Button from "@/components/ui/Button";

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [clave, setClave] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const response = await authService.login({ email, password: clave });
      localStorage.setItem("authToken", response.token);
      router.push("/");
    } catch (err: any) {
      console.error("Error detallado de C#:", err.response?.data);

      if (err.response?.data?.errors) {
        setError(JSON.stringify(err.response.data.errors));
      } else if (err.response?.data) {
        setError(JSON.stringify(err.response.data));
      } else {
        setError("Credenciales incorrectas o error de conexión.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex bg-bg-base relative overflow-hidden">
      <div className="absolute inset-0 mesh-gradient" />
      <div className="absolute inset-0 grid-bg" />
      <div className="absolute top-1/3 left-1/2 -translate-x-1/2 w-[600px] h-[600px] bg-accent/5 rounded-full blur-3xl" />

      <div className="relative flex flex-col justify-center w-full py-12 px-4 sm:px-6 lg:px-8">
        <div className="sm:mx-auto sm:w-full sm:max-w-md animate-fade-in-up opacity-0" style={{ animationFillMode: "forwards" }}>
          <Link href="/" className="flex items-center justify-center gap-3 mb-8 group">
            <div className="w-10 h-10 rounded-xl bg-accent-dim border border-accent/30 flex items-center justify-center group-hover:shadow-glow-sm transition-all">
              <ShoppingBag size={20} className="text-accent" />
            </div>
            <span className="font-bold text-xl text-text-primary">
              Clean<span className="text-accent">Ecommerce</span>
            </span>
          </Link>

          <div className="text-center mb-8">
            <div className="inline-flex items-center justify-center w-14 h-14 rounded-2xl bg-accent-dim border border-accent/30 mb-4">
              <Lock size={24} className="text-accent" />
            </div>
            <h2 className="text-2xl font-bold text-text-primary tracking-tight">
              Iniciar sesión
            </h2>
            <p className="mt-2 text-sm text-text-muted">
              O{" "}
              <Link href="/" className="font-medium text-accent hover:text-accent-bright transition-colors">
                volvé a la tienda
              </Link>
            </p>
          </div>

          <div className="glow-border rounded-2xl border border-border-subtle bg-bg-card/80 backdrop-blur-xl p-8 shadow-card">
            <form className="space-y-5" onSubmit={handleSubmit}>
              {error && (
                <div className="bg-error-dim border border-error/30 text-error text-sm p-4 rounded-xl text-center">
                  {error}
                </div>
              )}

              <Input
                id="email"
                label="Correo electrónico"
                type="email"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="ejemplo@empresa.com"
              />

              <Input
                id="clave"
                label="Contraseña"
                type="password"
                required
                value={clave}
                onChange={(e) => setClave(e.target.value)}
              />

              <Button type="submit" loading={loading} className="w-full" size="lg">
                {loading ? "Iniciando sesión..." : "Ingresar"}
              </Button>
            </form>
          </div>

          <p className="mt-6 text-center text-xs text-text-muted">
            Acceso seguro con encriptación de extremo a extremo
          </p>
        </div>
      </div>
    </div>
  );
}
