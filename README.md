# Clean E-Commerce Platform (Enterprise Architecture)

![CI Status](https://github.com/calebJT7/CleanEcommerce-Pro/actions/workflows/ci.yml/badge.svg)
![.NET Version](https://img.shields.io/badge/.NET-9.0-purple?style=for-the-badge&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-Clean-green?style=for-the-badge)
![Messaging](https://img.shields.io/badge/Messaging-RabbitMQ-orange?style=for-the-badge&logo=rabbitmq)

A Full-Stack e-commerce ecosystem built with **Clean Architecture** and **Domain-Driven Design (DDD)** principles. This repository contains the Core Web API, Admin Dashboard, and Client Storefront—a complete monorepo solution deployed across multiple cloud platforms.

## 🚀 Live Deployments

- **API (Azure):** [https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/](https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/)
- **Client Storefront (Vercel):** [https://clean-ecommerce-frontend-p28nl2arx-calebjt7s-projects.vercel.app/](https://clean-ecommerce-frontend-p28nl2arx-calebjt7s-projects.vercel.app/)
- **Admin Dashboard (Blazor):** `http://localhost:7050` (Local development)
- **API Swagger:** [https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/swagger](https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/swagger)

## Architecture Overview

The system features a strict separation of concerns, operating through a tri-frontend architecture:

- **Core API (.NET 9):** Centralized backend handling secure endpoints, business logic, and database operations with role-based JWT claim validation. Deployed on Azure App Service with PostgreSQL.
- **Admin Dashboard (Blazor WebAssembly):** A highly secure, private panel restricted to `Admin` roles for inventory management and order monitoring. Runs locally on developers' machines.
- **Client Storefront (Next.js):** A public-facing, SEO-optimized application consuming public API endpoints (`[AllowAnonymous]`) for the end-user shopping experience. Deployed on Vercel with automatic CI/CD from GitHub.

## Advanced Enterprise Features

- **Asynchronous Messaging:** Integrated **RabbitMQ** with **MassTransit** to decouple order processing. The system publishes `PedidoCreated` events for background workers.
- **Strict Role Isolation:** Custom JWT claim logic ensuring airtight security between public storefront consumers and administrative staff.
- **Structured Logging:** Implemented **Serilog** with Console and File sinks for professional monitoring and troubleshooting.
- **Automated Testing & CI/CD:** Core business logic covered by **xUnit** unit tests, integrated with **GitHub Actions** to ensure code quality on every push.

## Technologies Used

- **Backend:** ASP.NET Core Web API (.NET 9)
- **Admin Panel:** Blazor WebAssembly (C#), HTML, Bootstrap 5
- **Database:** SQL Server & Entity Framework Core
- **Messaging:** RabbitMQ & MassTransit
- **DevOps:** Docker, Docker Compose, GitHub Actions

## Future Roadmap

- [ ] Complete database persistence integration for the incoming Next.js `/api/Orders` payload.
- [ ] Implement Refresh Token rotation to boost JWT security for long-lived admin sessions.
- [ ] Migrate file logging to an ELK Stack (Elasticsearch, Logstash, Kibana) for cloud observability.

---

# Plataforma de Comercio Electrónico (Arquitectura Empresarial)

Ecosistema Full-Stack desarrollado bajo **Clean Architecture**, optimizado para entornos corporativos con un enfoque estricto en la seguridad, escalabilidad y la separación de responsabilidades.

## Resumen de la Arquitectura

El sistema opera mediante una arquitectura de múltiples frontends:

- **Core API (.NET 9):** Backend centralizado que maneja la lógica de negocio y exposición de endpoints seguros mediante validación de claims y roles JWT.
- **Panel Administrativo (Blazor):** Dashboard privado y de alto rendimiento, restringido exclusivamente a administradores para el control de inventario.
- **Vitrina de Clientes (Next.js):** _En repositorio independiente._ Aplicación pública optimizada que consume los endpoints abiertos de la API para el catálogo y carrito de compras.

## Funcionalidades Avanzadas

- **Mensajería Asíncrona:** Uso de **RabbitMQ** y **MassTransit** para desacoplar el procesamiento de pedidos mediante eventos.
- **Aislamiento de Seguridad:** Implementación de tokens JWT con validación personalizada para evitar la escalación de privilegios desde aplicaciones cliente.
- **Logging Estructurado:** Trazabilidad profesional de errores y auditoría con **Serilog**.
- **Calidad de Código:** Tests unitarios con **xUnit** y automatización de integración continua (CI) mediante **GitHub Actions**.

## Reproduction Steps / Pasos para la Reproducción

### 🇬🇧 English

#### Local Development

1. **Clone the repository:**

```bash
git clone https://github.com/calebJT7/CleanEcommerce-Pro.git
cd CleanEcommerce
```

2. **Infrastructure Setup:** Run the following command to start services via Docker:

```bash
docker-compose up -d
```

3. **API Setup:**
   - Open `Api/appsettings.Development.json`
   - Update `ConnectionStrings:DefaultConnection` with your SQL Server instance
   - Run migrations:

   ```bash
   cd Api
   dotnet ef database update
   ```

4. **Run the API:**

```bash
dotnet run
# API will be available at https://localhost:7050
```

5. **Run the Admin Dashboard (Blazor):**

```bash
cd Web
dotnet run
# Dashboard will be available at https://localhost:7100
```

6. **Run the Client Storefront (Next.js):**

```bash
cd ecommerce-frontend
npm install
npm run dev
# Frontend will be available at http://localhost:3000
# Set NEXT_PUBLIC_API_URL=http://localhost:7050/api for local testing
```

#### Production Deployment

- **API:** Automatically deploys to Azure from `main` branch via GitHub Actions
- **Frontend:** Automatically deploys to Vercel from `main` branch when `ecommerce-frontend/` changes
- **Environment Variables for Vercel:**
  - `NEXT_PUBLIC_API_URL`: Set to `https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/api`

## Autor

**Caleb Toledo** - Systems Analyst & Full-Stack Developer.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/caleb-toledo-356b56336/)
