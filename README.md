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

## Technologies Used 🚀
- **Backend:** ASP.NET Core 9 Web API, C#, LINQ, EF Core.
- **Database:** Microsoft SQL Server.
- **Authentication & Security:** JWT, Refresh Tokens, Role-based Access Control.
- **Architecture & Patterns:** Clean Architecture, Repository Pattern,  Chain of Responsibility Pattern(Implemented in custom middleware), Service Locator Pattern with Lazy Initialization, Unit of Work Pattern, Dependency Injection.
- **Data Handling:** DTOs, AutoMapper, Pagination, Fluent API.
- **Custom Middleware:**
  - `ErrorHandlingMiddleware`: Captures and logs unhandled exceptions.
  - `RequestTimeLoggingMiddleware`: Logs the duration of HTTP requests.
- **Logging:** Serilog is integrated for structured logging.
- **Seeding:** Seeds initial data into the database.
- **Custom Filters:** `HasPermission` filter ensures permission-based access control.
- **Testing:** Uses xUnit for unit and integration testing with high feature coverage.

## 🎯 Features

## 📦 Order Management 

- **Get All Orders**: Retrieve paginated orders with related data and merchant info.
- **Get Order by ID**: Fetch order details including branch, region, and products.
- **Get Waiting Orders**: Retrieve all waiting orders in paginated form.
- **Get Orders by Status**: Filter orders by status with pagination.
- **Create Order**: Add a new order, compute cost, create a report, and handle transaction.
- **Update Order**: Update fields using DTO.
- **Delete Order**: Remove an order with not-found validation.
- **Assign Courier**: Assign courier and mark order as DeliveredToCourier.
- **Change Order Status**: Modify the order’s status (Delivered, Declined, etc.).
- **Calculate Shipping Cost**: Calculate cost based on weight and location.
- **Get Merchant Name**: Internal logic to retrieve merchant names.

## 📑 Order Report Management

- **Get All Order Reports**: Return paginated order reports with merchant and finance details.
- **Get Order Report by ID**: Fetch specific report using its ID.
- **Create Order Report**: Add a new entry with DTO.
- **Update Order Report**: Modify an existing report by ID.
- **Delete Order Report**: Remove a report with not-found check.
- **Calculate Financial Summary**: Compute merchant name, amount received, and shipping cost paid.

## 🏢 Branch Management

- **Get All Branches**: Retrieve a paginated list of all branches.
- **Get Branch by ID**: Fetch a specific branch using its unique identifier.
- **Create Branch**: Add a new branch from DTO input.
- **Update Branch**: Modify an existing branch’s details.
- **Delete Branch**: Remove a branch by ID with not-found validation.

## 👤 User Management

- **Get Account Profile**: Retrieve user profile by ID.
- **Update User Details**: Modify name, address, and store info.
- **Assign Roles**: Assign a role to a user by email.
- **Unassign Roles**: Remove assigned role from a user.
- **Create Employee**: Add new employee and assign role.
- **Create Merchant**: Add new merchant and assign role with special city config.
- **Create Courier**: Add courier and link to special regions.


## 🏙️ City Setting Management 

- **Get All City Settings**: Retrieve paginated city settings with region, orders, and users.
- **Get City Setting by ID**: Fetch a specific city setting with all relations.
- **Create City Setting**: Add a new city setting entry.
- **Update City Setting**: Modify an existing city setting.
- **Delete City Setting**: Delete a city setting by ID.
- **Get Cities by Region ID**: Return all cities under a given region.

## 📦 Product Management

- **Get All Products**: Return paginated product list.
- **Get Product by ID**: Retrieve product details using ID.
- **Create Product**: Add new product with DTO.
- **Update Product**: Modify existing product by ID.
- **Delete Product**: Remove product entry with error check.


## 🌍 Region Management 

- **Get All Regions**: Paginated list of regions with city settings.
- **Get Region by ID**: Return a region and its cities.
- **Create Region**: Add new region.
- **Update Region**: Modify existing region.
- **Delete Region**: Remove region using its ID.

## 🛡️ Role Management

- **Get All Roles**: Return all active roles.
- **Get Role by ID**: Retrieve role info with permissions.
- **Create Role**: Add new role with permission list.
- **Update Role**: Modify role name and permissions.
- **Delete Role**: Remove role entry by ID.


## 🚚 Shipping Type Management 

- **Get All Shipping Types**: List shipping types with order relation.
- **Get Shipping Type by ID**: Retrieve full shipping type info.
- **Create Shipping Type**: Add new entry.
- **Update Shipping Type**: Modify existing type.
- **Delete Shipping Type**: Remove by ID.

## 🏙️ Special City Cost Management

- **Get All Special City Costs**: Paginated list with city and merchant relations.
- **Get Special City Cost by ID**: Retrieve full cost entry.
- **Create Special City Cost**: Add new custom city cost.
- **Update Special City Cost**: Modify existing record.
- **Delete Special City Cost**: Remove entry by ID.


## 📍 Special Courier Region Management

- **Get All Special Regions**: Return all special courier regions.
- **Get Special Region by ID**: Retrieve a region with courier and region info.
- **Create Special Region**: Add new entry.
- **Update Special Region**: Modify an existing region.
- **Delete Special Region**: Remove a record by ID.

## ⚖️ Weight Setting Management 

- **Get All Weight Settings**: Paginated list of weight-based shipping costs.
- **Get Weight Setting by ID**: Return specific rule by ID.
- **Create Weight Setting**: Add new weight rule.
- **Update Weight Setting**: Modify existing setting.
- **Delete Weight Setting**: Remove setting by ID.

## 🚚 Courier Management 

- **Get All Couriers**: Paginated list of couriers as DTOs.
- **Get Couriers by Branch**: Retrieve couriers assigned to a specific branch.
- **Get Couriers by Region**: Retrieve couriers by region with fallback to special regions.


## 📊 Courier Report Management 

- **Get All Courier Reports**: Fetch courier reports grouped by courier name.
- **Get Courier Report by ID**: Retrieve report with full courier and merchant context.


## 📈 Dashboard Management

- **Get Merchant Dashboard**: Show order statuses (Delivered, Pending, etc.) for a merchant.
- **Get Employee Dashboard**: View order summaries by status for employees.


## 👨‍💼 People Management 

- **Get All Employees**: Fetch a paginated list of employees.
- **Get All Merchants**: Retrieve a list of merchants as DTOs.


> 💡 This modular service-oriented architecture improves maintainability, testability, and scalability across the shipping platform.

## 🧪 Testing

This project uses **xUnit** for testing and includes unit and integration test coverage for most major features.

### Running Tests

You can run the tests using the **Test Explorer** in Visual Studio or via CLI:

```bash
dotnet test tests/Shipping.[Layer].Tests
```

Tests cover:
- Handlers (Commands & Queries)
- Validation rules
- Controller actions
- Authorization logic
- Integration scenarios

---

## 🚀 Getting Started

### ✅ Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or later

### ⚙️ Build the Solution

```bash
Open Shipping.sln in Visual Studio and build the solution.
```

### ▶️ Run the API

```bash
Set `Shipping.API` as the startup project and run the application.
```
