# PhoneXchange

A personal ASP.NET Core MVC project for buying and selling new and used mobile phones.

Built to practise C# web development, relational databases, authentication, authorization and automated testing.

## Overview

PhoneXchange allows users to publish phone listings, search and filter available phones, contact sellers, save favourites and leave reviews.

The application uses ASP.NET Core MVC, Entity Framework Core and SQL Server, with separate projects for the web interface, business logic, data access and tests.

The user interface is in Bulgarian. Listing prices are entered and displayed in EUR.

## Features

### Accounts and access control

- Registration and login with ASP.NET Core Identity.
- Admin and User roles.
- Ownership checks when editing or deleting listings.
- An administration area for managing users and roles.

### Phone listings

- Create, edit, delete and browse listings.
- Search by keyword.
- Filter by brand, price range and condition.
- Sort results and navigate through paginated listings.
- Soft deletion of listings through EF Core global query filters.

### Image uploads

- Upload up to three images per listing.
- Preview selected images before submitting the form.
- Server-side checks for file size, image format and dimensions.
- Resize large images and save them as JPEG files with generated names.
- Keep existing images or replace them when editing a listing.

### User interactions

- Save listings to a favourites list.
- Send messages to sellers.
- Leave reviews with ratings.
- Soft deletion of messages.

### Read-only API

- `GET /api/ads` — retrieve listings.
- `GET /api/ads/{id}` — retrieve a listing by ID.

## Technology Stack

| Area | Technology |
| --- | --- |
| Language | C# |
| Web framework | ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 |
| Database | Microsoft SQL Server |
| Authentication | ASP.NET Core Identity |
| Frontend | Razor Views, Bootstrap, HTML, CSS, JavaScript |
| Image processing | SixLabors.ImageSharp |
| Testing | NUnit, Moq, EF Core InMemory |
| Version control | Git and GitHub |

## Project Structure

The application is located inside the `CSharpWeb_PhoneXchange_2025` directory.

```text
ASP.NET-Core-2025/
└── CSharpWeb_PhoneXchange_2025/
    ├── PhoneXchange.Web
    ├── PhoneXchange.Web.ViewModels
    ├── PhoneXchange.Services.Core
    ├── PhoneXchange.Data
    ├── PhoneXchange.Data.Models
    ├── PhoneXchange.Data.Common
    ├── PhoneXchange.GCommon
    ├── PhoneXchange.Services.Tests
    └── PhoneXchange.Common.Tests
```

| Project | Responsibility |
| --- | --- |
| `PhoneXchange.Web` | Controllers, Razor views, Identity pages, MVC areas, image storage and application configuration |
| `PhoneXchange.Web.ViewModels` | Models used to receive form data and display application data |
| `PhoneXchange.Services.Core` | Application services and business operations |
| `PhoneXchange.Data` | Database context, repositories, migrations and initial data |
| `PhoneXchange.Data.Models` | Domain entities |
| `PhoneXchange.Data.Common` | Shared data-layer constants |
| `PhoneXchange.GCommon` | Application-wide constants, roles and helpers |
| `PhoneXchange.Services.Tests` | Automated service tests |
| `PhoneXchange.Common.Tests` | Shared test helpers and database setup |

## Implementation Notes

### Separation of responsibilities

Controllers handle HTTP requests, validation responses and access checks. Services handle application operations, while repositories provide access to stored data.

### Repository pattern

The generic repository provides synchronous and asynchronous operations. It also exposes queries through `GetAllAttached()`, allowing services to compose filters, sorting and pagination before executing database queries.

### Soft deletion

The repository detects an `IsDeleted` property through reflection and marks supported entities as deleted. EF Core global query filters hide deleted listings and messages from normal queries.

### Image storage

Image processing is handled by `AdImageStorage` in the web project.

Uploaded files are stored in:

```text
PhoneXchange.Web/wwwroot/uploads/ads/
```

The database stores their relative URLs rather than the image files themselves.

### Prices

All listing prices are treated as EUR. The existing development data uses sample values and is not intended to represent historical currency conversions.

## Getting Started

### Requirements

- .NET 8 SDK.
- A running SQL Server instance, such as SQL Server Developer, Express or LocalDB.
- Git.
- Visual Studio or another editor with C# support.

The connection example below uses Windows authentication and SQL Server Express LocalDB.

### 1. Clone the repository

```bash
git clone https://github.com/nasko017/ASP.NET-Core-2025.git
cd ASP.NET-Core-2025/CSharpWeb_PhoneXchange_2025
```

Run the remaining commands from this directory.

### 2. Configure the database connection

Store the local connection string with .NET User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\MSSQLLocalDB;Database=PhoneXchangeDb;Trusted_Connection=True;TrustServerCertificate=True;" --project PhoneXchange.Web
```

Replace the server name and authentication settings if you use a different SQL Server instance.

The LocalDB example requires LocalDB to be installed. `TrustServerCertificate=True` is included for local development.

### 3. Restore dependencies

```bash
dotnet restore PhoneXchange.Web
```

### 4. Trust the development HTTPS certificate

```bash
dotnet dev-certs https --trust
```

### 5. Run the application

```bash
dotnet run --project PhoneXchange.Web --launch-profile https
```

Open the HTTPS address printed in the terminal. The current launch profile uses:

```text
https://localhost:7164
```

The `https` launch profile runs the application in the Development environment.

During Development startup, the application:

- Applies pending EF Core migrations.
- Ensures the Admin and User roles exist.
- Creates the demonstration administrator if it is missing.
- Adds missing phone brands.
- Adds sample listings when the seeder finds no visible listings.

A separate migration command is not required for this startup flow. The configured database account must have permission to apply migrations.

### Local demonstration administrator

The Development seeder creates an administrator with this email:

```text
admin@phones.com
```

The demonstration password is defined in `EnsureAdminAsync` inside:

```text
PhoneXchange.Data/Seeding/ApplicationDbSeeder.cs
```

This account is intended for local development. Production deployment requires separate administrator provisioning and credentials.

## Running Tests

From `CSharpWeb_PhoneXchange_2025`, run:

```bash
dotnet test PhoneXchange.Services.Tests
```

The test project uses NUnit and Moq. Shared test helpers provide an EF Core InMemory database.

These tests check application behaviour in isolation; they do not replace testing against SQL Server or testing the complete application through a browser.

## Try the Application

After starting the application:

1. Register a user account.
2. Create a listing and upload images.
3. Search, filter and sort the catalogue.
4. Edit one of your listings.
5. Use a second account to try favourites, messages and reviews.
6. Verify that a regular user cannot edit another user's listing.

## Current Limitations

- Uploaded images are stored on the local filesystem. Deployment requires persistent storage and backups.
- Replaced image files are currently retained on disk. Automatic cleanup of unused files is not implemented.
- The API provides read-only listing endpoints.
- The application is a portfolio project; it does not process payments or arrange delivery.
- The installed ImageSharp package reports a licence requirement for Release builds. Resolve the package's licensing requirements before creating a Release deployment.

## Planned Improvements

- Cleanup of unused uploaded images.
- More tests for validation and authorization scenarios.
- Deployment with persistent image storage.
- Screenshots and a short demonstration of the main user flows.

## Author

**Atanas Uluchev**

- GitHub: [nasko017](https://github.com/nasko017)
- Project: [PhoneXchange](https://github.com/nasko017/ASP.NET-Core-2025)
