# PhoneXchange 📱

A full-stack web marketplace for buying and selling mobile phones, built with ASP.NET Core MVC.

## Features

- **Browse & Search** — paginated listings with keyword search across titles and descriptions
- **User Accounts** — register, login, and manage your own ads
- **Create & Manage Ads** — post ads with phone details, brand, OS, condition, and images
- **Favorites** — save ads to a personal favorites list
- **Messaging** — send messages to ad owners directly through the platform
- **Reviews & Ratings** — leave reviews with star ratings on listings
- **Admin Panel** — role-based admin area for managing users
- **Soft Delete** — deleted ads are hidden via Global Query Filters, not permanently removed

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC 8 |
| ORM | Entity Framework Core 8 |
| Database | Microsoft SQL Server |
| Authentication | ASP.NET Core Identity |
| Architecture | Repository Pattern + Service Layer |
| Testing | NUnit + Moq |
| Frontend | Razor Views + Bootstrap 5 |

## Architecture

The project is split into multiple layers:

- **PhoneXchange.Data.Models** — EF Core entities
- **PhoneXchange.Data** — DbContext, Repositories, Seeding
- **PhoneXchange.Services.Core** — Business logic (Service layer)
- **PhoneXchange.Web.ViewModels** — DTOs for views
- **PhoneXchange.Web** — Controllers, Views, Areas
- **PhoneXchange.Services.Tests** — Unit tests

## Getting Started

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

   Open `PhoneXchange.Web/appsettings.json` and update the connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=PhoneXchangeDb;Trusted_Connection=True;"
   }
   ```

3. **Apply migrations**
   ```bash
   cd CSharpWeb_PhoneXchange_2025/PhoneXchange.Web
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

   The app will seed initial data (brands, sample ads, admin user) automatically on first run in Development mode.

### Default Admin Account

| Field | Value |
|---|---|
| Email | admin@phonexchange.com |
| Password | admin123 |

## Running Tests

```bash
cd CSharpWeb_PhoneXchange_2025
dotnet test
```

Tests cover the service layer using NUnit and Moq with an in-memory database.

## License

This project was built as a portfolio project for the SoftUni C# Web course (2025).
