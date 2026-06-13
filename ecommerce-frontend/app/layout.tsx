import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { CartProvider } from "../context/CartContext";

const inter = Inter({
  subsets: ["latin"],
  variable: "--font-inter",
  display: "swap",
});

export const metadata: Metadata = {
  title: "CleanEcommerce | Tecnología Premium",
  description: "Plataforma de comercio tecnológico de alto rendimiento",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="es" className={inter.variable}>
      <body className="bg-bg-base text-text-primary antialiased min-h-screen flex flex-col">
        <CartProvider>{children}</CartProvider>
      </body>
    </html>
  );
}
