import type { Metadata } from "next";
import "./globals.css";
import { CartProvider } from "../context/CartContext";

export const metadata: Metadata = {
  title: "CleanEcommerce",
  description: "Plataforma de compras corporativa",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="es">
      <body className="bg-[#F9FAFB] text-gray-900 antialiased">
        <CartProvider>
          {children}
        </CartProvider>
      </body>
    </html>
  );
}
