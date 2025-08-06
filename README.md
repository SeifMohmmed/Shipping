# 📦 Shipping Management System - Project Overview

## 🚀 Overview

The **Shipping Management System** is a robust, scalable, and modern web application built using **.NET 9** and **C# 13**. It is designed to manage and optimize shipping operations through well-structured APIs, featuring authentication, authorization, shipping management, and more.

Built with clean architecture in mind, the project ensures **separation of concerns**, **testability**, and **maintainability** by organizing code across multiple layers.

---
## 🏗 Project Architecture
The project follows a clean architecture approach, dividing the codebase into distinct layers under the src folder:

### 1. **📁 API Layer (`Shipping.API`)**
   - Contains controllers, middleware, filters, and extensions.
   - Handles HTTP requests and responses.

### 2. **📁 Application Layer (`Shipping.Application`)**
   - Contains business logic and service interfaces.
   - Implements core functionalities like user management, region handling, etc.

### 3. **📁 Domain Layer (`Shipping.Domain`)**
   - Defines entities, enums, and constants.
   - Represents the core of the business model.

### 4. **📁 Infrastructure Layer (`Shipping.Infrastructure`)**
   - Handles data access and external integrations.
   - Includes repositories and database configurations.

### 5. **📁 Unit Testing Project ('xUnit')**

  - Separate test projects created for each major layer and component.
  - Includes tests for services, controllers, middleware, and integration points.
  - Uses xUnit for testing and Moq for mocking dependencies.

--- 
## 📸 Screenshots
### 🗂️ Class Diagram
<p align="center">
  <img src="https://github.com/SeifMohmmed/Shipping/blob/d6c82672f397525ac90ff2611fba4989cccaad6e/Screenshots/Class%20Diagram.png" alt="image alt"/>
</p>

### 🌐 Endpoints

 <p align="center">
  <img src="https://github.com/SeifMohmmed/Shipping/blob/d6c82672f397525ac90ff2611fba4989cccaad6e/Screenshots/Endpoints.PNG" alt="image alt"/>
</p>

---
## Technologies Used 🚀
- **Backend:** ASP.NET Core 9 Web API, C#, LINQ, EF Core.
- **Database:** Microsoft SQL Server.
- **Authentication & Security:** JWT, Refresh Tokens, Role-based Access Control.
- **Architecture & Patterns:** Clean Architecture, Repository Pattern, Decorator Pattern(Implemented in custom middleware), Factory Pattern, Unit of Work, Dependency Injection, Services.
- **Data Handling:** DTOs, AutoMapper, Pagination, Fluent API.
- **Custom Middleware:**  ErrorHandlingMiddleware(Captures unhandled exceptions), RequestTimeLoggingMiddleware(Logs request durations).
- **Logging:** Serilog is integrated for structured logging.
- **Seeding:** Seeds initial data into the database.
- **Custom Filters:** `HasPermission` filter ensures permission-based access control.

---

## 📦 Core Features

### 📌 Shipping Type Management

* Add, update, delete, retrieve shipping types.

### 📌 Special City Costs

* Assign custom shipping costs to specific cities.

### 📌 Special Courier Regions

* Manage which couriers operate in which regions.

### 📌 Weight Settings

* Define weight ranges and associated shipping costs.

### 🔐 Authentication & Authorization

* Secure all endpoints with JWT and permissions.

### 🔍 Pagination Support

* Standardized pagination across endpoints like `GetRegions`, `GetAllEmployees`.

### 🛡 Custom Filters

* `[HasPermission]` attribute ensures permission-based access control.

---

## ▶️ How to Run

1. **Clone** the repository:

   ```bash
   git clone https://github.com/SeifMohmmed/Shipping-Management-System.git
   ```
2. **Restore NuGet packages**:

   ```bash
   dotnet restore
   ```
3. **Run the project**:

   ```bash
   dotnet run --project Shipping.API
   ```
4. **Access Swagger UI** at:

   ```
   https://localhost:{port}/swagger
   ```
