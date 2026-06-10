# 📱 PhoneXchange

> A full-featured ASP.NET Core MVC marketplace for buying and selling second-hand and new phones.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue?logo=dotnet)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework%20Core-8.0-purple)
![MS SQL Server](https://img.shields.io/badge/Database-MS%20SQL%20Server-red?logo=microsoftsqlserver)
![NUnit](https://img.shields.io/badge/Tests-NUnit%20%2B%20Moq-green)

---

## 📋 Overview

PhoneXchange is a marketplace web application where users can post ads for phones, browse listings, send messages to sellers, leave reviews, and manage their favourite ads. Administrators have a dedicated area for user management.

---

## ✨ Features

- 🔐 **Authentication & Authorization** — Register, Login, Role-based access (Admin / User)
- 📢 **Ads** — Create, edit, delete, and browse phone listings with pagination and search
- 💬 **Messaging** — Users can contact sellers directly through the platform
- ⭐ **Reviews** — Leave ratings and comments on ads
- ❤️ **Favourites** — Save ads to a personal favourites list
- 🛡️ **Admin Area** — Manage users and roles through a dedicated admin panel
- 🗑️ **Soft Delete** — Deleted ads and messages are hidden, not permanently removed
- 🌐 **REST API endpoint** — `/api/ads` for fetching ad data

---

## 🏗️ Architecture

The project follows a layered architecture with clear separation of concerns:

```
PhoneXchange/
├── PhoneXchange.Web                  # Controllers, Views, Areas, Program.cs
├── PhoneXchange.Web.ViewModels       # View Models for all entities
├── PhoneXchange.Services.Core        # Business logic (Service layer)
├── PhoneXchange.Data                 # DbContext, Repositories, Migrations, Seeding
├── PhoneXchange.Data.Models          # Domain entities
├── PhoneXchange.Data.Common          # Shared constants and exception messages
├── PhoneXchange.GCommon              # App-wide constants, roles, helpers
├── PhoneXchange.Services.Tests       # Unit tests for the Service layer
└── PhoneXchange.Common.Tests         # Shared test utilities (TestDb, Mocks)
```

### Key design decisions

- **Repository Pattern** — Generic `BaseRepository<TEntity, TKey>` with both sync and async methods, exposing `IQueryable` via `GetAllAttached()` for flexible querying at the service layer.
- **Soft Delete** — Implemented via reflection in `BaseRepository`. Entities with an `IsDeleted` property are soft-deleted; EF Core global query filters automatically exclude them.
- **Service Layer** — All business logic lives in dedicated services, keeping controllers thin and testable.
- **Admin Area** — Separate MVC Area with `[Authorize(Roles = "Admin")]` base controller.

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 |
| Database | Microsoft SQL Server |
| Authentication | ASP.NET Core Identity |
| Testing | NUnit, Moq |
| Frontend | Razor Views, Bootstrap |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/nasko017/ASP.NET-Core-2025.git
   cd ASP.NET-Core-2025
   ```

2. **Configure the connection string**

   Open `PhoneXchange.Web/appsettings.json` and update:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=PhoneXchangeDb;Trusted_Connection=True;"
   }
   ```

3. **Apply migrations**
   ```bash
   cd PhoneXchange.Web
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```
   The app seeds initial data (brands, admin user) automatically on first run in Development mode.

### Default Admin Account

| Field | Value |
|---|---|
| Email | admin@phonexchange.com |
| Password | *(set in seeder)* |

---

## 🧪 Running the Tests

```bash
cd PhoneXchange.Services.Tests
dotnet test
```

Tests cover the Service layer using **NUnit** and **Moq** with an in-memory database for integration-style scenarios.

---

## 📁 Project Highlights

### Soft Delete with Reflection
The `BaseRepository` automatically detects if an entity has an `IsDeleted` property and sets it to `true` on delete, without any entity-specific code:

```csharp
private void PerformSoftDeleteOfEntity(TEntity entity)
{
    PropertyInfo? isDeletedProperty = typeof(TEntity)
        .GetProperties()
        .FirstOrDefault(pi => pi.Name == "IsDeleted" && pi.PropertyType == typeof(bool));

    isDeletedProperty?.SetValue(entity, true);
}
```

### Pagination & Search
Ads support server-side pagination and keyword search, executed directly in SQL via EF Core:

```csharp
var ads = await query
    .OrderByDescending(a => a.CreatedOn)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .Select(a => new AdViewModel { ... })
    .ToListAsync();
```

---

## 👤 Author

**Atanas** — [@nasko017](https://github.com/nasko017)

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
