# ADR 0001: API Baseline Conventions

- Status: Accepted (Week 0)
- Date: 2026-02-19

## Context
The bot currently interacts with Oracle directly. This project needs stable HTTP contracts for both bot and future website clients.

## Decision
- Use ASP.NET Core REST API with route prefix `/api/v1`.
- Use resource-oriented routes with plural nouns.
- Use DTO contracts that do not mirror Oracle row shape 1:1.
- Use RFC 9457 ProblemDetails for all non-2xx responses.
- Use shared query defaults for paging and common filters.

## Rationale
- Path versioning reduces ambiguity for client migration.
- ProblemDetails keeps error handling consistent across bot and website.
- DTO separation prevents Oracle schema changes from breaking clients.

## Consequences
Positive
- Consistent contract behavior and easier client integration.
- Clear future path for `/api/v2`.

Tradeoff
- Some mapping overhead between DB entities and DTO contracts.

## Follow-up
- Add shared constants/options class for paging and max limits during Week 1.
- Add integration tests that assert ProblemDetails shape for 4xx/5xx.
