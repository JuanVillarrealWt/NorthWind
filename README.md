# 🛒 NorthWind Sales — Point of Sale (POS) Web Application

> **Sistema web de Punto de Venta (Facturación)** desarrollado con **Clean Architecture**, **ASP.NET Minimal API**, **Blazor WebAssembly** y **SQL Server**.

---

## 📋 Table of Contents

1. [Overview](#overview)
2. [Architecture — Clean Architecture](#architecture--clean-architecture)
3. [Solution Structure](#solution-structure)
4. [Technology Stack](#technology-stack)
5. [Prerequisites](#prerequisites)
6. [Initial Setup — Step by Step](#initial-setup--step-by-step)
7. [Configuration Files](#configuration-files)
8. [Running the Application](#running-the-application)
9. [Application Pages](#application-pages)
10. [API Endpoints](#api-endpoints)
11. [Implemented Requirements](#implemented-requirements)
12. [Database Schema](#database-schema)

---

## Overview

NorthWind Sales is a **Point of Sale (POS)** web application that allows users to:

- Create **sales orders** (invoices) with a master/detail structure (order header + order details).
- Search for **customers** and **products** using smart search modals with pagination.
- Generate and **visualize invoices as PDF** from previously saved orders.
- Perform full **CRUD operations** on Customers and Products.
- Automatically **deduct inventory** when an order is saved.
- Validate stock availability, prevent duplicate products, and calculate taxes (IVA).

---

## Architecture — Clean Architecture

The solution follows **Robert C. Martin's Clean Architecture** (also known as Onion/Hexagonal Architecture). Dependencies always point **inward** — outer layers depend on inner layers, never the reverse.

```
┌──────────────────────────────────────────────────────────┐
│                 Frameworks & Drivers                     │
│  ┌─────────────────┐  ┌──────────────────────────────┐   │
│  │  BlazorClient    │  │  WebApi (ASP.NET Minimal)    │   │
│  │  (Blazor WASM)   │  │  + EF Core DataContexts      │   │
│  └────────┬─────────┘  └──────────────┬───────────────┘   │
│           │     IoC (Dependency       │                   │
│           │      Injection)           │                   │
│  ┌────────┴───────────────────────────┴───────────────┐   │
│  │              Interface Adapters                     │   │
│  │  Controllers │ Presenters │ Repositories │ Views    │   │
│  │  WebApiGateways (Frontend ↔ Backend HTTP)           │   │
│  └────────────────────────┬───────────────────────────┘   │
│  ┌────────────────────────┴───────────────────────────┐   │
│  │           Application Business Rules               │   │
│  │    UseCases │ Backend BusinessObjects │ Frontend BO │   │
│  └────────────────────────┬───────────────────────────┘   │
│  ┌────────────────────────┴───────────────────────────┐   │
│  │           Enterprise Business Rules                │   │
│  │  Entities │ Validators │ Transactions │ Events      │   │
│  │  Exceptions │ DomainLogs │ Result                   │   │
│  └────────────────────────────────────────────────────┘   │
└──────────────────────────────────────────────────────────┘
```

### Dependency Rule

| Layer | Depends On |
|---|---|
| **Entities** | Nothing (core domain) |
| **UseCases / BusinessObjects** | Entities only |
| **Controllers / Presenters / Repositories** | UseCases + Entities |
| **Frameworks (WebApi, Blazor, EF Core)** | All inner layers (via IoC) |

---

## Solution Structure

The solution file `NorthWind.slnx` organizes projects into **4 main solution folders** matching Clean Architecture layers:

### 🔵 Enterprise Business Rules (Core Domain)

| Project | Description |
|---|---|
| `NorthWind.Entities` | Base domain entities shared across the application |
| `NorthWind.Sales.Entities` | Sales-specific entities, DTOs, value objects (Endpoints, OrderDetail) |
| `NorthWind.Sales.Validators.Entities` | FluentValidation validators for order creation |
| `NorthWind.Validation.Entities` | Generic validation abstractions |
| `NorthWind.Result.Entities` | Result pattern for returning success/failure |
| `NorthWind.Transactions.Entities` | Transaction abstractions (`IDomainTransaction`) |
| `NorthWind.Events.Entities` | Domain event abstractions |
| `NorthWind.Exceptions.Entities` | Custom domain exceptions |
| `NorthWind.DomainLogs.Entities` | Domain logging abstractions |
| `NorthWind.Membership.Entities` | Authentication/membership entities and endpoints |
| `NorthWind.HttpDelegatingHandlers` | HTTP message handlers (Bearer token injection) |

### 🟢 Application Business Rules

| Project | Description |
|---|---|
| `NorthWind.Sales.Backend.BusinessObjects` | Backend interfaces (ICommandsRepository, IQueriesRepository), aggregates, value objects (ProductUnitsInStock), specifications, guards |
| `NorthWind.Sales.Backend.UseCases` | Application use cases: `CreateOrder`, `SearchCustomers`, `SearchProducts`, `SearchOrders`, `GetOrderById`. Each use case has its own Interactor + InputPort/OutputPort |
| `NorthWind.Sales.Frontend.BusinessObjects` | Frontend interfaces (ICreateOrderGateway, ISearchCustomersGateway, ICustomerCrudGateway, etc.) and ViewModels |

### 🟡 Interface Adapters

| Project | Description |
|---|---|
| `NorthWind.Sales.Backend.Controllers` | Minimal API controllers: `CreateOrder`, `SearchCustomers`, `SearchProducts`, `SearchOrders`, `GetOrderById`, `CustomerCrud`, `ProductCrud` |
| `NorthWind.Sales.Backend.Presenters` | Presenters that transform use case output to HTTP responses |
| `NorthWind.Sales.Backend.Repositories` | Repository implementations (CommandsRepository, QueriesRepository) + persistence entity mappings |
| `NorthWind.Sales.Frontend.WebApiGateways` | HTTP client gateways that call the WebApi from Blazor |
| `NorthWind.Sales.Frontend.Views` | Blazor Razor pages and components (UI layer) |
| `NorthWind.RazorComponents` | Shared reusable Razor components |
| `NorthWind.Membership.Frontend.RazorViews` | Login/Register pages |

### 🔴 Frameworks & Drivers

| Project | Description |
|---|---|
| `NorthWind.Sales.WebApi` | **ASP.NET Minimal API** host — entry point for the backend. Configures DI, JWT auth, CORS, Swagger, and maps all endpoints |
| `NorthWind.Sales.BlazorClient` | **Blazor WebAssembly** host — entry point for the frontend SPA |
| `NorthWind.Sales.Backend.DataContexts.EFCore` | **Entity Framework Core** DbContext, migrations, entity configurations, and data context implementations |
| `NorthWind.Sales.Backend.IoC` | Backend dependency injection container + endpoints registration |
| `NorthWind.Sales.Frontend.IoC` | Frontend dependency injection container |
| `NorthWind.Sales.Backend.SmtpGateways` | SMTP email gateway implementation |
| `NorthWind.ValidationService.FluentValidation` | FluentValidation service implementation |
| `NorthWind.UserServices` | User service utilities |
| `NorthWind.Membership.Backend.AspNetIdentity` | ASP.NET Identity implementation |
| `NorthWind.Membership.Backend.Core` | Membership core services |

---

## Technology Stack

| Component | Technology |
|---|---|
| **Backend** | ASP.NET (.NET 10) — Minimal API |
| **Frontend** | Blazor WebAssembly |
| **Database** | SQL Server (LocalDB) |
| **ORM** | Entity Framework Core |
| **Authentication** | JWT Bearer Tokens + ASP.NET Identity |
| **Validation** | FluentValidation |
| **Architecture** | Clean Architecture (4 layers) |
| **PDF Generation** | Browser `window.print()` via JS Interop |

---

## Prerequisites

Before running the project, ensure you have:

1. **.NET 10 SDK** (or later) — [Download](https://dotnet.microsoft.com/download)
2. **SQL Server LocalDB** — Comes bundled with Visual Studio, or install separately via [SQL Server Express LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
3. **Visual Studio 2022/2025** (recommended) or **VS Code** with C# Dev Kit
4. A **modern web browser** (Chrome, Edge, Firefox)

Verify installation:

```bash
dotnet --version       # Should output 10.x.x
sqllocaldb info        # Should list MSSQLLocalDB
```

---

## Initial Setup — Step by Step

### Step 1: Clone or Open the Solution

Open the solution in Visual Studio:

```
NorthWind.slnx
```

Or from the command line:

```bash
cd "C:\Users\Lenovo\Desktop\Aplicaciones Moviles\NorthWind"
```

### Step 2: Restore NuGet Packages

```bash
dotnet restore NorthWind.slnx
```

### Step 3: Configure the Connection Strings

Open the file:

```
NorthWind.Sales.WebApi/appsettings.json
```

Verify or update the connection strings under `DBOptions`:

```json
{
  "DBOptions": {
    "ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=NorthWindDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
    "DomainLogsConnectionString": "Server=(localdb)\\mssqllocaldb;Database=NorthWindLogsDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },
  "MembershipDBOptions": {
    "ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=NorthWindUsersDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

> **Note:** If you are NOT using LocalDB, change `(localdb)\\mssqllocaldb` to your SQL Server instance name (e.g., `Server=localhost;` or `Server=.\SQLEXPRESS;`).

### Step 4: Create the Database (EF Core Migrations)

The project includes EF Core migrations that will automatically create and seed the database tables.

From the solution root directory, run:

```bash
dotnet ef database update --project NorthWind.Sales.Backend.DataContexts.EFCore --startup-project NorthWind.Sales.WebApi
```

This will:
- Create the `NorthWindDB` database in SQL Server
- Create the `Customers`, `Products`, `Orders`, and `OrderDetails` tables
- Seed initial data (5 sample products and 5 sample customers)

> **If `dotnet ef` is not installed**, install it first:
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### Step 5: Verify Frontend Configuration

The Blazor client needs to know the WebApi URL. Check this file:

```
NorthWind.Sales.BlazorClient/wwwroot/appsettings.json
```

```json
{
  "WebApiAddress": "https://localhost:7154"
}
```

This must match the `applicationUrl` in `NorthWind.Sales.WebApi/Properties/launchSettings.json`:

```json
{
  "profiles": {
    "https": {
      "applicationUrl": "https://localhost:7154;http://localhost:5254"
    }
  }
}
```

### Step 6: Build the Solution

```bash
dotnet build NorthWind.slnx
```

Expected output: `Compilación correcta. 0 Advertencia(s), 0 Errores`.

---

## Configuration Files

| File | Location | Purpose |
|---|---|---|
| `appsettings.json` | `NorthWind.Sales.WebApi/` | Backend: DB connection strings, JWT config, SMTP settings |
| `appsettings.json` | `NorthWind.Sales.BlazorClient/wwwroot/` | Frontend: WebApi base address URL |
| `launchSettings.json` | `NorthWind.Sales.WebApi/Properties/` | Backend launch profile: ports and environment |
| `NorthWind.slnx` | Root | Solution file — references all projects |
| `NuGet.Config` | Root | NuGet package sources configuration |

---

## Running the Application

### Option A: Visual Studio (Recommended)

1. Open `NorthWind.slnx` in Visual Studio.
2. Set **multiple startup projects**:
   - `NorthWind.Sales.WebApi` → Start
   - `NorthWind.Sales.BlazorClient` → Start
3. Press **F5** or click **Start**.

### Option B: Command Line

Open **two terminals**:

**Terminal 1 — Backend (WebApi):**

```bash
cd NorthWind.Sales.WebApi
dotnet run
```

The API will start on `https://localhost:7154`.

**Terminal 2 — Frontend (Blazor Client):**

```bash
cd NorthWind.Sales.BlazorClient
dotnet run
```

The Blazor app will start on the port shown in the terminal output (e.g., `https://localhost:7xxx`).

### Accessing the Application

| Component | URL |
|---|---|
| **Blazor Frontend** | `https://localhost:<blazor-port>` |
| **WebApi Swagger** | `https://localhost:7154/swagger` |

---

## Application Pages

The application has **4 main pages** plus CRUD maintenance:

### 1. 🛒 Point of Sale — Create Order (`/CreateOrder`)

The master/detail order creation page:
- **Header (Master):** Select a customer via smart search modal, order date (auto-set).
- **Detail:** Add products via smart search modal with quantity.
- **Calculations:** Automatic subtotal, IVA (12%), and total.
- **Validations:** No duplicate products, stock availability check, required fields.
- **Actions:** Save order (creates invoice), clear form.

### 2. 🔍 Customer Search Modal (Popup within Create Order)

- Smart search by **Customer ID** and **Customer Name** simultaneously.
- Server-side **pagination** for optimized data retrieval.
- Click to select a customer for the order header.

### 3. 📦 Product Search Modal (Popup within Create Order)

- Smart search by **Product Name** and **Product ID** simultaneously.
- Only shows products with **stock > 0** (Requirement 8).
- Server-side **pagination** for optimized data retrieval.
- Click to add a product to the order detail.

### 4. 🧾 Invoices — Search & View (`/invoices`)

- Search saved orders by **Order Number** or **Customer Name**.
- Reconstructs the full invoice (header + detail) for visualization.
- **Print to PDF** functionality via browser print dialog.

### 5. 👥 Customers Maintenance (`/customers`)

- Full CRUD: Create, Read, Update, Delete customers.
- Field validation (ID format, name required, balance).

### 6. 📦 Products Maintenance (`/products`)

- Full CRUD: Create, Read, Update, Delete products.
- Field validation (name required, price > 0, stock ≥ 0).

---

## API Endpoints

All endpoints are defined in `NorthWind.Sales.Entities/ValueObjects/Endpoints.cs`:

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/CreateOrder` | Create a new sales order with details |
| `GET` | `/SearchCustomers` | Search customers with pagination |
| `GET` | `/SearchProducts` | Search products with pagination (stock > 0) |
| `GET` | `/SearchOrders` | Search saved orders |
| `GET` | `/GetOrderById` | Get full order details by ID |
| `POST` | `/CreateCustomer` | Create a new customer |
| `PUT` | `/UpdateCustomer` | Update an existing customer |
| `DELETE` | `/DeleteCustomer` | Delete a customer |
| `POST` | `/CreateProduct` | Create a new product |
| `PUT` | `/UpdateProduct` | Update an existing product |
| `DELETE` | `/DeleteProduct` | Delete a product |
| `POST` | `/user/Register` | Register a new user |
| `POST` | `/user/Login` | Authenticate and get JWT token |

---

## Implemented Requirements

| # | Requirement | Status |
|---|---|---|
| 1 | Entity-Relationship model with normalization | ✅ |
| 2 | Create sales order with proper fields and tables | ✅ |
| 3 | Product appears only once in the order (no duplicates) | ✅ |
| 4 | Calculate totals, subtotals, and IVA | ✅ |
| 5 | Edit only necessary fields, block the rest | ✅ |
| 6 | Field validations (letters only, numbers only, no blanks, email format, ID format) | ✅ |
| 7 | Automatic inventory deduction on save | ✅ |
| 8 | Show only products with stock > 0 | ✅ |
| 9 | Cannot sell more than available stock | ✅ |
| 10 | Allow removing a product from the order before saving | ✅ |
| 11 | Allow updating/replacing a product in the order before saving | ✅ |
| 12 | View/reconstruct a saved invoice with PDF download | ✅ |
| 13 | Usability standards in UI design | ✅ |
| 14 | Error control with custom messages | ✅ |
| 15 | Smart search with popup modals (2+ fields) + pagination | ✅ |
| 16 | Clean code best practices, all in English | ✅ |
| 17 | Transactions and concurrency handling with frontend feedback | ✅ |
| 18 | CRUD maintenance for Customers and Products (+ manual SQL) | ✅ |

### 4 Required Pages

| # | Page | Status |
|---|---|---|
| 1 | Point of Sale — Create Order (master/detail) | ✅ `/CreateOrder` |
| 2 | Customer smart search popup modal | ✅ `CustomerSearchModal.razor` |
| 3 | Product smart search popup modal | ✅ `ProductSearchModal.razor` |
| 4 | Invoice search and visualization | ✅ `/invoices` |

---

## Database Schema

```
┌──────────────────┐       ┌──────────────────┐
│    Customers     │       │    Products      │
├──────────────────┤       ├──────────────────┤
│ Id (PK, varchar) │       │ Id (PK, int)     │
│ Name             │       │ Name             │
│ CurrentBalance   │       │ UnitPrice        │
└──────┬───────────┘       │ UnitsInStock     │
       │                   └──────┬───────────┘
       │ 1:N                      │ 1:N
       │                          │
┌──────┴───────────┐              │
│     Orders       │              │
├──────────────────┤              │
│ Id (PK, int)     │              │
│ CustomerId (FK)  │──────────────┘
│ OrderDate        │              │
└──────┬───────────┘              │
       │ 1:N                      │
       │                          │
┌──────┴───────────┐              │
│  OrderDetails    │              │
├──────────────────┤              │
│ Id (PK, int)     │              │
│ OrderId (FK)     │              │
│ ProductId (FK)   │──────────────┘
│ Quantity         │
│ UnitPrice        │
└──────────────────┘
```

**Seed Data:** The migrations automatically seed 5 sample products (Chai, Chang, Aniseed Syrup, Chef Anton's Cajun Seasoning, Chef Anton's Gumbo Mix) and 5 sample customers.

---

## Project File Tree

```
NorthWind/
├── NorthWind.slnx                              # Solution file
├── NuGet.Config                                # NuGet package sources
├── README.md                                   # This file
│
├── NorthWind.Entities/                         # Core domain entities
├── NorthWind.Sales.Entities/                   # Sales DTOs, Value Objects, Endpoints
│   ├── Dtos/
│   │   ├── SearchCustomers/CustomerDto.cs
│   │   └── SearchProducts/ProductDto.cs
│   └── ValueObjects/Endpoints.cs
│
├── NorthWind.Sales.Validators.Entities/        # FluentValidation validators
│   └── CreateOrder/
├── NorthWind.Validation.Entities/              # Validation abstractions
├── NorthWind.Result.Entities/                  # Result pattern
├── NorthWind.Transactions.Entities/            # Transaction interfaces
├── NorthWind.Events.Entities/                  # Domain events
├── NorthWind.Exceptions.Entities/              # Custom exceptions
├── NorthWind.DomainLogs.Entities/              # Logging abstractions
├── NorthWind.Membership.Entities/              # Auth/membership entities
├── NorthWind.HttpDelegatingHandlers/           # HTTP Bearer token handler
│
├── NorthWind.Sales.Backend.BusinessObjects/    # Backend interfaces & domain logic
│   ├── Aggregates/OrderAggregate
│   ├── Interfaces/
│   │   ├── Repositories/ICommandsRepository.cs
│   │   ├── Repositories/IQueriesRepository.cs
│   │   ├── CreateOrder/
│   │   ├── SearchCustomers/
│   │   ├── SearchProducts/
│   │   ├── SearchOrders/
│   │   └── GetOrderById/
│   ├── ValueObjects/ProductUnitsInStock.cs
│   ├── Guards/
│   └── Specifications/
│
├── NorthWind.Sales.Backend.UseCases/           # Application use cases
│   ├── CreateOrder/CreateOrderInteractor.cs
│   ├── SearchCustomers/
│   ├── SearchProducts/
│   ├── SearchOrders/
│   ├── GetOrderById/
│   └── Resources/CreateOrderMessages.cs
│
├── NorthWind.Sales.Frontend.BusinessObjects/   # Frontend interfaces & ViewModels
│   ├── Interfaces/
│   │   ├── ICreateOrderGateway.cs
│   │   ├── ISearchCustomersGateway.cs
│   │   ├── ISearchProductsGateway.cs
│   │   ├── ISearchOrdersGateway.cs
│   │   ├── ICustomerCrudGateway.cs
│   │   └── IProductCrudGateway.cs
│   └── ViewModels/
│       └── ProductItemViewModel.cs
│
├── NorthWind.Sales.Backend.Controllers/        # Minimal API controllers
│   ├── CreateOrder/
│   ├── SearchCustomers/
│   ├── SearchProducts/
│   ├── SearchOrders/
│   ├── GetOrderById/
│   ├── CustomerCrud/CustomerCrudController.cs
│   └── ProductCrud/ProductCrudController.cs
│
├── NorthWind.Sales.Backend.Presenters/         # Response presenters
│   ├── CreateOrder/
│   ├── SearchCustomers/
│   ├── SearchProducts/
│   ├── SearchOrders/
│   └── GetOrderById/
│
├── NorthWind.Sales.Backend.Repositories/       # Repository implementations
│   ├── Repositories/
│   │   ├── CommandsRepository.cs               # Write operations (CRUD + Orders)
│   │   └── QueriesRepository.cs                # Read operations (Search + Get)
│   ├── Entities/                               # Persistence entities (Product, Customer, etc.)
│   └── Interfaces/                             # DataContext interfaces
│
├── NorthWind.Sales.Backend.DataContexts.EFCore/ # EF Core implementation
│   ├── DataContexts/NorthWindContext.cs        # DbContext
│   ├── Services/
│   │   └── NorthWindSalesCommandsDataContext.cs # DB operations implementation
│   ├── Configurations/                         # Entity type configurations + seed data
│   ├── Migrations/                             # EF Core migrations
│   │   ├── 20261009234819_InitialCreate.cs
│   │   └── 20261010010000_AddProductAndCustomer.cs
│   ├── Guards/
│   └── Options/DBOptions.cs
│
├── NorthWind.Sales.Frontend.WebApiGateways/    # HTTP client gateways (Frontend → Backend)
│   ├── CreateOrderGateway.cs
│   ├── SearchCustomersGateway.cs
│   ├── SearchProductsGateway.cs
│   ├── SearchOrdersGateway.cs
│   ├── CustomerCrudGateway.cs
│   ├── ProductCrudGateway.cs
│   └── DependencyContainer.cs
│
├── NorthWind.Sales.Frontend.Views/             # Blazor UI (Razor Pages & Components)
│   ├── Pages/
│   │   ├── CreateOrder.razor                   # POS page entry
│   │   ├── InvoicesPage.razor                  # Invoice search & PDF view
│   │   ├── CustomersPage.razor                 # Customer CRUD page
│   │   ├── ProductsPage.razor                  # Product CRUD page
│   │   └── Home.razor                          # Landing page
│   ├── Components/
│   │   ├── CreateOrderComponent.razor          # Master/detail order form
│   │   ├── CustomerSearchModal.razor           # Smart search popup
│   │   └── ProductSearchModal.razor            # Smart search popup
│   ├── Layout/
│   │   └── MainLayout.razor                    # App layout with navigation
│   └── ViewModels/CreateOrder/
│       └── CreateOrderViewModel.cs
│
├── NorthWind.Sales.Backend.IoC/                # Backend DI container
│   ├── DependencyContainer.cs                  # Service registration
│   └── EndpointsContainer.cs                   # Endpoint mapping
│
├── NorthWind.Sales.Frontend.IoC/               # Frontend DI container
│   └── DependencyContainer.cs
│
├── NorthWind.Sales.WebApi/                     # ★ BACKEND HOST (ASP.NET)
│   ├── Program.cs                              # Entry point
│   ├── Startup.cs                              # App configuration
│   ├── appsettings.json                        # ⚙️ CONNECTION STRINGS + JWT CONFIG
│   └── Properties/launchSettings.json          # ⚙️ PORTS (7154)
│
├── NorthWind.Sales.BlazorClient/               # ★ FRONTEND HOST (Blazor WASM)
│   ├── Program.cs                              # Entry point
│   ├── App.razor                               # Root component
│   └── wwwroot/
│       ├── appsettings.json                    # ⚙️ WEBAPI ADDRESS
│       ├── index.html                          # HTML host page
│       └── css/                                # Stylesheets
│
├── NorthWind.RazorComponents/                  # Shared Razor components
├── NorthWind.Sales.Backend.SmtpGateways/       # Email gateway
├── NorthWind.ValidationService.FluentValidation/ # FluentValidation service
├── NorthWind.UserServices/                     # User services
├── NorthWind.Membership.Backend.AspNetIdentity/ # ASP.NET Identity
├── NorthWind.Membership.Backend.Core/          # Membership core
└── NorthWind.Membership.Frontend.RazorViews/   # Login/Register views
```

---

## Troubleshooting

| Problem | Solution |
|---|---|
| `dotnet ef` not found | Run: `dotnet tool install --global dotnet-ef` |
| Database connection fails | Verify SQL Server LocalDB is running: `sqllocaldb start MSSQLLocalDB` |
| Blazor cannot connect to API | Check that `WebApiAddress` in `BlazorClient/wwwroot/appsettings.json` matches the WebApi port |
| CORS errors in browser | The WebApi already configures `AllowAnyOrigin()`. Ensure both projects are running |
| Swagger not showing | Navigate to `https://localhost:7154/swagger` while running in Development mode |

---

## License

This project is an academic assignment for the **Mobile Applications** course. All rights reserved.
