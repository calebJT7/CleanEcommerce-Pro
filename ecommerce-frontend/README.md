# Clean E-Commerce Platform (Next.js Storefront)

![Next.js](https://img.shields.io/badge/Next.js-black?style=for-the-badge&logo=next.js&logoColor=white)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)
![React](https://img.shields.io/badge/React_Context-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![Vercel](https://img.shields.io/badge/Deployed_on-Vercel-000000?style=for-the-badge&logo=vercel&logoColor=white)

The public-facing, high-performance storefront of the CleanEcommerce ecosystem. Built with **Next.js** and **Tailwind CSS**, this application provides users with a fluid, lightning-fast shopping experience while acting as a decoupled consumer of the core .NET Web API.

## Architecture Overview

This frontend operates as a completely independent presentation layer. By separating the client storefront from the administrative panel (Blazor) and the core API, the architecture ensures:

- **High Scalability:** The public storefront can scale globally via CDN/Edge networks independently from backend server loads.
- **Security:** Exclusively consumes public (`[AllowAnonymous]`) endpoints, keeping internal system logic fully isolated.
- **State Persistence:** Utilizes React's Context API combined with native browser caching (`localStorage`) for uninterrupted shopping sessions.

## Key Features Implemented

- **Dynamic Catalog Loading:** Real-time data fetching directly from the live SQL Server backend via Axios.
- **Global Cart State:** Reactive shopping cart tracking with the Context API, ensuring UI synchronization across all routes.
- **Modern UI/UX:** Fully responsive, utility-first design leveraging Tailwind CSS for seamless mobile and desktop adaptation.
- **Asynchronous Checkout:** Prepared service architecture to securely transmit cart payloads back to the C# API for order processing.

## Technologies Used

- **Framework:** Next.js (App Router)
- **Styling:** Tailwind CSS & Lucide Icons
- **State Management:** React Context API
- **HTTP Client:** Axios (Centralized API configuration)
- **Deployment:** Vercel CI/CD

## Future Roadmap

- [ ] Connect the Checkout UI to international payment gateways (e.g., Stripe, MercadoPago).
- [ ] Implement Server-Side Rendering (SSR) for product detail pages to maximize global SEO.
- [ ] Integrate NextAuth.js for unified social login and standard customer authentication.

---

# Plataforma de Comercio Electrónico (Vitrina Next.js)

La cara pública y de alto rendimiento del ecosistema CleanEcommerce. Desarrollada con **Next.js** y **Tailwind CSS**, esta aplicación provee una experiencia de compra fluida y veloz, actuando como un consumidor desacoplado de la API central en .NET.

## Resumen de la Arquitectura

Este frontend opera como una capa de presentación completamente independiente. Al separar la vitrina del cliente del panel administrativo (Blazor) y de la API, la arquitectura garantiza:

- **Escalabilidad:** Capacidad de escalar globalmente a través de redes CDN/Edge sin impactar la carga del servidor backend.
- **Seguridad:** Consumo exclusivo de endpoints públicos, manteniendo la lógica interna y administrativa completamente aislada.
- **Persistencia de Estado:** Uso de Context API combinado con caché nativo (`localStorage`) para mantener las sesiones de compra activas.

## Funcionalidades Principales

- **Catálogo Dinámico:** Carga de datos en tiempo real mediante consumo de la API de C# con Axios.
- **Estado Global del Carrito:** Seguimiento reactivo del carrito de compras asegurando la sincronización de la interfaz en todas las rutas.
- **UI/UX Moderna:** Diseño 100% responsivo y adaptable utilizando la metodología utility-first de Tailwind CSS.
- **Checkout Asíncrono:** Arquitectura de servicios preparada para transmitir de forma segura los paquetes de órdenes hacia el backend.

## Reproduction Steps / Pasos para la Reproducción

### 🇬🇧 English

1. **Clone the repository:**

```bash
git clone https://github.com/calebJT7/ecommerce-frontend-nextjs.git
```

2. **Install dependencies:**

```bash
npm install
```

3. **Environment setup:**
   Create a `.env.local` file in the root directory and add your backend API URL:

```bash
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

4. **Run the development server:**

```bash
npm run dev
```

5. **Open your browser:**
   Navigate to `http://localhost:3000` to see the application running. (Note: Ensure the CleanEcommerce .NET API is running locally to fetch product data.)

Autor
Caleb Toledo - Systems Analyst & Full-Stack Developer.
