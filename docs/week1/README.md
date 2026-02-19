# Week 1 Baseline

- Date: 2026-02-19
- Owner: Michael (solo)
- Status: In Progress

## Goal
Establish the API foundation required before implementing domain endpoints.

## Completed
- Scaffolded solution and project structure:
  - `GameDbApi.sln`
  - `src/GameDb.Api`
  - `src/GameDb.Application`
  - `src/GameDb.Infrastructure`
  - `tests/GameDb.Api.IntegrationTests`
- Added baseline API startup plumbing in `src/GameDb.Api/Program.cs`:
  - Controller setup
  - Swagger/OpenAPI registration
  - JWT authentication and authorization policy wiring
  - ProblemDetails and exception handling setup
  - Liveness and readiness health endpoints
- Added centralized configuration models:
  - `src/GameDb.Api/Configuration/ApiPagingOptions.cs`
  - `src/GameDb.Api/Configuration/JwtOptions.cs`
- Added authorization constants/extensions:
  - `src/GameDb.Api/Authorization/RoleNames.cs`
  - `src/GameDb.Api/Authorization/PolicyNames.cs`
  - `src/GameDb.Api/Authorization/AuthorizationExtensions.cs`
- Added Oracle readiness health check:
  - `src/GameDb.Api/Health/OracleConnectionHealthCheck.cs`
- Added Swagger JWT security configuration:
  - `src/GameDb.Api/Configuration/SwaggerGenOptionsExtensions.cs`
- Added baseline app configuration keys:
  - `src/GameDb.Api/appsettings.json`
  - `src/GameDb.Api/appsettings.Development.json`
- Added integration smoke tests for health endpoints:
  - `tests/GameDb.Api.IntegrationTests/HealthEndpointsTests.cs`
- Removed template/sample files not used by v1 plan.

## Validation
- Build: `dotnet build GameDbApi.sln` passed.
- Tests: `dotnet test GameDbApi.sln --no-build` passed (`2` tests).

## Notes
- Installed .NET SDK 8 (`8.0.418`) in the environment.
- Added NuGet source `nuget.org` because no sources were configured.
- Added `.gitignore` to exclude `bin/` and `obj/` build artifacts.

## Next Week 1 Tasks
1. Implement request/response contracts for Priority Read endpoints in code.
2. Add first endpoint: `GET /api/v1/games/search` with `pageSize` and `botPageSize` behavior.
3. Add endpoint integration tests for auth, validation, and response shape.
4. Add initial data-access abstraction in `GameDb.Infrastructure` for game search.