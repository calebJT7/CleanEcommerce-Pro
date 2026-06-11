# Clean E-Commerce Platform (Enterprise Architecture)

![CI Status](https://github.com/calebJT7/CleanEcommerce-Pro/actions/workflows/ci.yml/badge.svg)
![.NET Version](https://img.shields.io/badge/.NET-9.0-purple?style=for-the-badge&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-Clean-green?style=for-the-badge)
![Messaging](https://img.shields.io/badge/Messaging-RabbitMQ-orange?style=for-the-badge&logo=rabbitmq)

A Full-Stack e-commerce ecosystem built with **Clean Architecture** and **Domain-Driven Design (DDD)** principles. This repository contains the Core Web API and the internal Management Dashboard, designed to securely serve decoupled client applications.

##  Architecture Overview

The system features a strict separation of concerns, operating through a dual-frontend architecture:
* **Core API (.NET 9):** Centralized backend handling secure endpoints, business logic, and database operations with role-based JWT claim validation.
* **Admin Dashboard (Blazor WebAssembly):** A highly secure, private panel restricted to `Admin` roles for inventory management and order monitoring.
* **Client Storefront (Next.js):** *Hosted in a separate repository.* A public-facing, SEO-optimized application consuming public API endpoints (`[AllowAnonymous]`) for the end-user shopping experience.

## Advanced Enterprise Features

- **Asynchronous Messaging:** Integrated **RabbitMQ** with **MassTransit** to decouple order processing. The system publishes `PedidoCreated` events for background workers.
- **Strict Role Isolation:** Custom JWT claim logic ensuring airtight security between public storefront consumers and administrative staff.
- **Structured Logging:** Implemented **Serilog** with Console and File sinks for professional monitoring and troubleshooting.
- **Automated Testing & CI/CD:** Core business logic covered by **xUnit** unit tests, integrated with **GitHub Actions** to ensure code quality on every push.

##  Technologies Used

- **Backend:** ASP.NET Core Web API (.NET 9)
- **Admin Panel:** Blazor WebAssembly (C#), HTML, Bootstrap 5
- **Database:** SQL Server & Entity Framework Core
- **Messaging:** RabbitMQ & MassTransit
- **DevOps:** Docker, Docker Compose, GitHub Actions

##  Future Roadmap

- [ ] Complete database persistence integration for the incoming Next.js `/api/Orders` payload.
- [ ] Implement Refresh Token rotation to boost JWT security for long-lived admin sessions.
- [ ] Migrate file logging to an ELK Stack (Elasticsearch, Logstash, Kibana) for cloud observability.

---

# Plataforma de Comercio Electrónico (Arquitectura Empresarial)

Ecosistema Full-Stack desarrollado bajo **Clean Architecture**, optimizado para entornos corporativos con un enfoque estricto en la seguridad, escalabilidad y la separación de responsabilidades.

##  Resumen de la Arquitectura

El sistema opera mediante una arquitectura de múltiples frontends:
* **Core API (.NET 9):** Backend centralizado que maneja la lógica de negocio y exposición de endpoints seguros mediante validación de claims y roles JWT.
* **Panel Administrativo (Blazor):** Dashboard privado y de alto rendimiento, restringido exclusivamente a administradores para el control de inventario.
* **Vitrina de Clientes (Next.js):** *En repositorio independiente.* Aplicación pública optimizada que consume los endpoints abiertos de la API para el catálogo y carrito de compras.

##  Funcionalidades Avanzadas

- **Mensajería Asíncrona:** Uso de **RabbitMQ** y **MassTransit** para desacoplar el procesamiento de pedidos mediante eventos.
- **Aislamiento de Seguridad:** Implementación de tokens JWT con validación personalizada para evitar la escalación de privilegios desde aplicaciones cliente.
- **Logging Estructurado:** Trazabilidad profesional de errores y auditoría con **Serilog**.
- **Calidad de Código:** Tests unitarios con **xUnit** y automatización de integración continua (CI) mediante **GitHub Actions**.

## Reproduction Steps / Pasos para la Reproducción

### 🇬🇧 English

1. **Clone the repository:** ```bash
   git clone [https://github.com/calebJT7/CleanEcommerce-Pro.git](https://github.com/calebJT7/CleanEcommerce-Pro.git)

````

2. **Infrastructure Setup:** Run the following command to start RabbitMQ via Docker:

```bash
docker-compose up -d
````

3. **Database Setup:** Update ConnectionStrings in appsettings.json (Api project) with your SQL Server instance.

4. **Apply Migrations:** Run the following in the Package Manager Console:

```bash
PowerShell
Update-Database
```

5. **Run:** Start the Web API and Blazor projects.

## Autor

**Caleb Toledo** - Systems Analyst & Full-Stack Developer.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/caleb-toledo-356b56336/)
