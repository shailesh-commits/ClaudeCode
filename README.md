# ClaudeCode — Online Catalog API

A spec-driven, **.NET 8 Clean Architecture** reference project: a production-grade REST API for an online catalog, built end-to-end using **Claude Code**. This repository doubles as a worked example of the full spec-driven workflow — organizational guidance, a structured brief, a technical spec, and the resulting generated codebase — used in the Technology Reboot Agentic AI / Claude Code training.

## What's in This Repo

This repo contains two things side by side:

1. **The spec-driven workflow artifacts** (`OnlineCatalogApi/`) — the original scaffold project plus the documents that drove its build: an organizational guidance file defining a 7-persona development process, a product brief, and a full technical specification.
2. **The generated solution** (`src/`, `tests/`) — the actual Clean Architecture .NET 8 API produced by following that spec: `OnlineCatalog.Api`, `OnlineCatalog.Application`, `OnlineCatalog.Domain`, `OnlineCatalog.Infrastructure`, and a unit test project.

## The Online Catalog API

A secure REST API that lets authenticated clients browse catalog items organized by category, and lets users maintain a personal wishlist.

**Core features**
- API-key-secured endpoints — no request reaches the API without a valid key
- User management (CRUD)
- Category management (CRUD)
- Catalog browsing with pagination, category filtering, and search
- Per-user wishlist (add / list / remove), filterable by category

### Architecture

Clean Architecture with strict inward-pointing dependencies:

```
OnlineCatalog.sln
├── src/
│   ├── OnlineCatalog.Api/             # Controllers, middleware, Program.cs — thin, delegates to MediatR
│   ├── OnlineCatalog.Application/     # CQRS commands/queries, handlers, validators, DTOs, mappings
│   ├── OnlineCatalog.Domain/          # Entities, exceptions, repository interfaces — pure C#, zero dependencies
│   └── OnlineCatalog.Infrastructure/  # EF Core, repositories, API key auth, services
└── tests/
    └── OnlineCatalog.UnitTests/       # xUnit + Moq + FluentAssertions
```

| Layer | Project | Rule |
|---|---|---|
| API | `OnlineCatalog.Api` | Thin controllers only — calls `mediator.Send()`, no business logic |
| Application | `OnlineCatalog.Application` | CQRS handlers, FluentValidation, AutoMapper — no EF Core references |
| Domain | `OnlineCatalog.Domain` | Pure C#, zero NuGet dependencies, owns all business rules |
| Infrastructure | `OnlineCatalog.Infrastructure` | EF Core, repositories, API key authentication, external services |

### Tech Stack

| Purpose | Library |
|---|---|
| CQRS / Mediator | MediatR |
| Validation | FluentValidation |
| Object mapping | AutoMapper |
| ORM | Entity Framework Core 8 (SQL Server) |
| Auth | Custom `X-Api-Key` authentication handler |
| Password hashing | BCrypt.Net |
| Logging | Serilog (structured, console sink) |
| API docs | Swashbuckle (Swagger UI) |
| API versioning | Asp.Versioning |
| Rate limiting | ASP.NET Core built-in rate limiter (fixed window) |
| Caching | `Microsoft.Extensions.Caching.Memory` |
| Testing | xUnit, Moq, FluentAssertions, coverlet |

### API Endpoints

All routes are versioned (`/api/v{version}/...`) and require a valid `X-Api-Key` header except `/health`.

| Resource | Method | Route |
|---|---|---|
| Users | `POST` | `/api/v1/users` |
| Users | `GET` | `/api/v1/users/{id}` |
| Users | `PUT` | `/api/v1/users/{id}` |
| Users | `DELETE` | `/api/v1/users/{id}` |
| Categories | `GET` | `/api/v1/categories` |
| Categories | `GET` | `/api/v1/categories/{id}` |
| Categories | `POST` | `/api/v1/categories` |
| Categories | `PUT` | `/api/v1/categories/{id}` |
| Categories | `DELETE` | `/api/v1/categories/{id}` |
| Catalog | `GET` | `/api/v1/catalog` (paginated, filterable by `categoryId`) |
| Catalog | `GET` | `/api/v1/catalog/{id}` |
| Catalog | `POST` | `/api/v1/catalog` |
| Catalog | `PUT` | `/api/v1/catalog/{id}` |
| Catalog | `DELETE` | `/api/v1/catalog/{id}` |
| Wishlist | `GET` | `/api/v1/wishlist` (filterable by `categoryId`) |
| Wishlist | `POST` | `/api/v1/wishlist` |
| Wishlist | `DELETE` | `/api/v1/wishlist/{itemId}` |
| Health | `GET` | `/health` |

Key business rules: deleting a category with linked catalog items returns `409 Conflict`; creating a catalog item with a non-existent `categoryId` returns `422 Unprocessable Entity`; duplicate wishlist entries return `409 Conflict`; missing/invalid API keys return `401 Unauthorized`; requests exceeding 100/min per key return `429 Too Many Requests`; all errors are returned in a `{ status, message, traceId }` envelope.

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB works out of the box on Windows; update the connection string for other environments)

## Build & Run

```bash
git clone https://github.com/technology-reboot/ClaudeCode.git
cd ClaudeCode

dotnet restore OnlineCatalog.slnx
dotnet build OnlineCatalog.slnx

dotnet run --project src/OnlineCatalog.Api
```

Database migrations are applied automatically on startup. Once running, Swagger UI is available at `/swagger` in the Development environment, with an `X-Api-Key` security scheme pre-configured for testing authenticated endpoints.

### Configuration

Connection string and other settings live in `src/OnlineCatalog.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OnlineCatalog;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Cache": {
    "DefaultTtlSeconds": 60
  }
}
```

Override `DefaultConnection` to point at your own SQL Server instance, and adjust `appsettings.Development.json` for local overrides.

## Running Tests

```bash
dotnet test OnlineCatalog.slnx
```

`tests/OnlineCatalog.UnitTests` covers representative handlers across the Catalog, Categories, and Users features using Moq and FluentAssertions.

## The Spec-Driven Workflow (`OnlineCatalogApi/`)

This folder documents how the `src/OnlineCatalog.*` solution was actually produced, and is useful as a template for similar Claude Code–driven builds:

- **`CLAUDE-ORG-GUIDANCE.md`** — an organizational guidance template defining a 7-persona development process (Product Owner → Business Analyst → Architect → UX Designer → Technical Lead → ...) that every feature, fix, or change runs through in order.
- **`SPEC.MD`** — the full technical specification: architecture, layer responsibilities, key libraries, and detailed functional requirements (FR-AUTH, FR-USER, FR-CAT, FR-CATALOG, FR-WISH) with explicit HTTP behaviors for each.
- **`docs/briefs/online-catalog-api.md`** — the product brief: goals, success definition, scope boundaries, user stories, edge cases/business rules, and acceptance criteria that the eventual implementation was validated against.
- **`Program.cs` / `appsettings.json`** — the original default ASP.NET Core Web API scaffold (still containing the default `WeatherForecastController`) that this project started from before the Clean Architecture solution under `src/` was generated.

## About

Part of the **Technology Reboot** Agentic AI training series, used to demonstrate Claude Code's spec-driven development workflow — taking a project from organizational guidance and a structured brief through to a fully implemented, tested, production-shaped .NET solution.
