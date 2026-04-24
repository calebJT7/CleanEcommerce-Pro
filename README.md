# CleanEcommerce API

![CI Status](https://github.com/calebJT7/CleanEcommerce-Pro/actions/workflows/ci.yml/badge.svg)
![Docker Version](https://img.shields.io/docker/v/calebjt/clean-ecommerce-api?sort=semver&label=Docker%20Hub&logo=docker)
![.NET Version](https://img.shields.io/badge/.NET-9.0-purple)

# Clean E-Commerce Platform

A Full-Stack e-commerce platform developed using **Clean Architecture**, designed to be scalable, secure, and easy to maintain.

## Technologies Used
* **Frontend:** Blazor WebAssembly (C#), HTML, Bootstrap 5.
* **Backend:** ASP.NET Core Web API (.NET 9).
* **Database:** SQL Server with Entity Framework Core.
* **Security:** Authentication and Authorization via JWT (JSON Web Tokens).

## Main Features
* **Dynamic Catalog:** Product display featuring image URLs.
* **Shopping Cart:** Session management and secure checkout.
* **Admin Dashboard:**
    * Real-time financial and operational metrics.
    * Full product CRUD and inventory management.
    * Logistics control and order shipping status tracking.
* **Customer Portal:** Personalized purchase history.

## Reproduction Steps
1. **Clone the repository:** Copy the project to your local machine using `git clone`.
2. **Database Setup:** Update the `ConnectionStrings` in the `appsettings.json` file within the API project to point to your SQL Server instance.
3. **Apply Migrations:** Open the Package Manager Console and run `Update-Database` to create the schema.
4. **Run the Backend:** Start the ASP.NET Core Web API project.
5. **Run the Frontend:** Start the Blazor WebAssembly project.

## Author
**Caleb** - Systems Analyst and Full-Stack Developer.

---

# Plataforma de Comercio Electrónico (Clean Architecture)

Una plataforma de comercio electrónico Full-Stack desarrollada con **Clean Architecture**, diseñada para ser escalable, segura y fácil de mantener.

## Tecnologías Utilizadas
* **Frontend:** Blazor WebAssembly (C#), HTML, Bootstrap 5.
* **Backend:** ASP.NET Core Web API (.NET 9).
* **Base de Datos:** SQL Server con Entity Framework Core.
* **Seguridad:** Autenticación y Autorización mediante JWT (JSON Web Tokens).

## Funcionalidades Principales
* **Catálogo Dinámico:** Visualización de productos con imágenes mediante URL.
* **Carrito de Compras:** Gestión de sesión de compras y checkout seguro.
* **Panel de Administración (Dashboard):**
    * Métricas financieras y operativas en tiempo real.
    * CRUD completo de productos y gestión de inventario.
    * Control de logística y estados de envío de pedidos.
* **Portal del Cliente:** Historial de compras personalizado.

## Pasos para la Reproducción
1. **Clonar el repositorio:** Copia el proyecto a tu máquina local usando `git clone`.
2. **Configurar la Base de Datos:** Actualiza la sección `ConnectionStrings` en el archivo `appsettings.json` del proyecto API con tu instancia de SQL Server.
3. **Aplicar Migraciones:** Abre la Consola de Administrador de Paquetes y ejecuta `Update-Database` para crear el esquema.
4. **Ejecutar el Backend:** Inicia el proyecto ASP.NET Core Web API.
5. **Ejecutar el Frontend:** Inicia el proyecto Blazor WebAssembly.

## Autor y Contacto
**Caleb Toledo** - Analista de Sistemas y Desarrollador Full-Stack.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/caleb-toledo-356b56336/)
