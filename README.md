# Clean E-Commerce Platform (Pro Edition)

![CI Status](https://github.com/calebJT7/CleanEcommerce-Pro/actions/workflows/ci.yml/badge.svg)
![.NET Version](https://img.shields.io/badge/.NET-9.0-purple?style=for-the-badge&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-Clean-green?style=for-the-badge)
![Messaging](https://img.shields.io/badge/Messaging-RabbitMQ-orange?style=for-the-badge&logo=rabbitmq)

A Full-Stack e-commerce platform built with **Clean Architecture** and **Domain-Driven Design (DDD)** principles, designed for high scalability and enterprise-grade reliability.

## 🚀 Advanced Enterprise Features (New!)

- **Asynchronous Messaging:** Integrated **RabbitMQ** with **MassTransit** to decouple order processing. The system publishes `PedidoCreated` events for background workers.
- **Structured Logging:** Implemented **Serilog** with Console and File sinks for professional monitoring and troubleshooting.
- **Automated Testing:** High-reliability core business logic covered by **xUnit** unit tests.
- **CI/CD Pipeline:** Fully automated build and test workflows using **GitHub Actions** to ensure code quality on every push.

## Technologies Used

- **Backend:** ASP.NET Core Web API (.NET 9).
- **Frontend:** Blazor WebAssembly (C#), HTML, Bootstrap 5.
- **Database:** SQL Server & Entity Framework Core.
- **Messaging:** RabbitMQ & MassTransit.
- **DevOps:** Docker, Docker Compose, GitHub Actions.

## Main Features

- **Dynamic Catalog:** Real-time product management.
- **Secure Checkout:** JWT-based authentication and secure transaction flow.
- **Admin Dashboard:** Financial metrics, inventory CRUD, and logistics tracking.
- **Event-Driven Flow:** Orders trigger asynchronous notifications.

---

# Plataforma de Comercio Electrónico (Versión Pro)

Plataforma Full-Stack desarrollada bajo **Clean Architecture**, optimizada para entornos empresariales con enfoque en escalabilidad y mantenimiento.

## 🚀 Funcionalidades Avanzadas Recientes

- **Mensajería Asíncrona:** Uso de **RabbitMQ** y **MassTransit** para desacoplar el procesamiento de pedidos mediante eventos.
- **Logging Estructurado:** Implementación de **Serilog** para trazabilidad profesional de errores y auditoría.
- **Calidad de Código:** Tests unitarios con **xUnit** y automatización de integración continua (CI) mediante **GitHub Actions**.
- **Arquitectura:** Separación estricta de responsabilidades (Domain, Application, Infrastructure, API).

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
