# Clean E-Commerce — Full-Stack Architecture Project

![.NET 9](https://img.shields.io/badge/.NET-9.0-purple?style=for-the-badge&logo=dotnet)
![Next.js](https://img.shields.io/badge/Next.js-16.2.7-black?style=for-the-badge&logo=nextdotjs)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-blue?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-0078D6?style=for-the-badge)

A multi-client e-commerce platform built around a single ASP.NET Core API, demonstrating clean architecture principles, API-first design, and end-to-end ownership across backend, frontend, and admin tooling.

## Technical Highlights

The system is organized into distinct layers — Domain, Application, Infrastructure, and API — enforcing separation of concerns and a unidirectional dependency flow toward the domain core. Two independent clients consume the same backend through a versioned, documented API surface: a Blazor WebAssembly admin dashboard and a Next.js public storefront. Authentication and authorization are implemented with JWT, protected routes, and role-based access control, applied consistently across both client applications. Core e-commerce workflows are fully modeled, including product catalog management, cart state, order creation, and payment registration with historical tracking. Backend logic and controller behavior are covered by an automated xUnit test suite running against an in-memory database. The solution is containerized with Docker Compose for local development and is structured to support distributed deployment, with scaffolding already in place for asynchronous messaging via MassTransit and RabbitMQ.

## Project Summary

The application is composed of three independently deployable units sharing a common backend contract:

- **API** (`Api/`): the core ASP.NET Core Web API, exposing endpoints for products, orders, payments, customers, and user accounts. It encapsulates all business logic and acts as the single source of truth for both clients.
- **Admin Dashboard** (`Web/`): a Blazor WebAssembly application providing authenticated administrative access for product management and order lifecycle operations, consuming the API exclusively through JWT-protected endpoints.
- **Client Storefront** (`ecommerce-frontend/`): a Next.js and React application serving the public-facing catalog, cart, and checkout experience, with client-side state management and HTTP communication handled through Axios.

## Architecture Overview

### Backend

The backend is implemented in ASP.NET Core (.NET 9) following clean architecture conventions, with controllers, services, repositories, and DTOs organized into clearly bounded layers. Persistence is handled through Entity Framework Core against SQL Server, with a connection string compatible with local development environments. JWT-based authentication enforces role-based authorization at the endpoint level. Structured logging is implemented with Serilog, and the API surface is documented and testable through Swagger.

### Frontend

The public storefront is built with Next.js 16 and React 19, using React Context for shopping cart state management and Axios for all API communication. TypeScript and Tailwind CSS are used throughout for type safety and consistent styling. The admin dashboard is implemented separately in Blazor WebAssembly, using `AuthenticationStateProvider` to manage JWT-based session state and `HttpClient` for API integration, kept fully decoupled from the storefront codebase.

### DevOps

Local development is orchestrated through Docker Compose, allowing the API, database, and supporting services to be provisioned consistently. The repository structure is CI-ready, and the API and storefront are configured for independent cloud deployment, currently hosted on Azure App Service and Vercel respectively.

## Main Features

- Public product catalog with images, pricing, and detail pages.
- Secure user registration and authentication.
- Authenticated cart and checkout flow with order submission.
- Administrative CRUD operations over the product catalog.
- Order detail views with status management.
- Payment registration with customer balance and history tracking.
- Centralized request and error logging via Serilog.
- Automated unit tests covering controller and service-layer behavior.

## Repository Structure

```
Api/                      Core Web API project
Web/                      Blazor admin dashboard project
ecommerce-frontend/       Next.js public storefront
Application/              DTOs, interfaces, and shared service contracts
Domain/                   Business entities and domain models
Infrastructure/           EF Core DbContext, repositories, and persistence logic
CleanEcommerce.UnitTest/  Unit tests for API controllers and services
Tests/                    Additional integration and validation scenarios
```

## Local Setup

### 1. Clone the repository

```bash
git clone https://github.com/calebJT7/CleanEcommerce-Pro.git
cd CleanEcommerce
```

### 2. Start required services

```bash
docker-compose up -d
```

### 3. Configure the backend database

Open `Api/appsettings.Development.json` and update `ConnectionStrings:DefaultConnection` to point to your SQL Server instance.

### 4. Apply migrations and run the API

```bash
cd Api
dotnet ef database update
dotnet run
```

### 5. Run the admin dashboard

```bash
cd Web
dotnet run
```

### 6. Run the storefront

```bash
cd ecommerce-frontend
npm install
npm run dev
```

### 7. Configure the storefront environment

Set the following environment variable in the storefront project:

```
NEXT_PUBLIC_API_URL=http://localhost:7050/api
```

## Live Demo

- **API:** https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/
- **API Documentation (Swagger):** https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/swagger
- **Client Storefront:** https://clean-ecommerce-frontend-p28nl2arx-calebjt7s-projects.vercel.app/

The Blazor admin dashboard is currently configured for local execution and connects to the same backend API as the public storefront.
