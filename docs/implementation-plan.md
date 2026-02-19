# GameDB API Implementation Plan

- Date: 2026-02-19
- Owner: Michael (solo)
- Status: Active

## 1) Objectives
- Replace direct Oracle access from the Discord bot with a C# REST API.
- Establish a stable backend contract for both bot and companion website.
- Ship incrementally with low risk and strong observability.

## 2) Confirmed Week 0 Decisions
- API style: REST, path versioning at `/api/v1`.
- Auth: JWT bearer only, including bot service principal.
- Errors: RFC 9457 `application/problem+json`.
- Nominations: upsert via `PUT /api/v1/gotm/rounds/{roundNumber}/nominations/users/{userId}`.
- Paging defaults: `page=1`, `pageSize=25` (max 100), `botPageSize=10` (max 100).
- `/todos` is out of API v1 scope.
- Read scope matrix is defined in `docs/week0/README.md`.

## 3) High-Level Architecture
- `src/GameDb.Api`: controllers, auth, middleware, OpenAPI.
- `src/GameDb.Application`: use-case services, validation, policies.
- `src/GameDb.Infrastructure`: Oracle access (`Oracle.ManagedDataAccess.Core` + Dapper), repositories.
- `tests/GameDb.Api.IntegrationTests`: end-to-end HTTP + DB integration tests.

## 4) Phased Execution

## Phase 1 - Foundation (Week 1)
Deliverables
- Create .NET solution/projects and dependency wiring.
- Add shared options/config model for paging limits and JWT settings.
- Add centralized exception handling and ProblemDetails responses.
- Add OpenAPI generation and baseline endpoint grouping.
- Add health endpoints: liveness + Oracle readiness.

Exit criteria
- API boots locally.
- JWT auth and role policies resolve correctly.
- ProblemDetails shape is returned for validation/auth errors.

## Phase 2 - Priority Read Endpoints (Week 2)
Deliverables
- `GET /api/v1/games/search`
- `GET /api/v1/games/{gameId}`
- `GET /api/v1/platforms`
- `GET /api/v1/gotm/rounds/{roundNumber}/nominations`
- Implement paging behavior including `botPageSize` precedence.
- Add integration tests for success, validation failure, and auth failure.

Exit criteria
- Bot can switch read commands for these domains to API calls.
- Endpoint latency and SQL performance are acceptable for expected load.

## Phase 3 - Priority Write Endpoints (Week 3)
Deliverables
- `PUT /api/v1/gotm/rounds/{roundNumber}/nominations/users/{userId}` (upsert)
- `DELETE /api/v1/gotm/rounds/{roundNumber}/nominations/{nominationId}`
- `POST /api/v1/collections/entries`
- `PATCH /api/v1/collections/entries/{entryId}`
- Enforce write authorization rules (subject match vs elevated role/service-bot).

Exit criteria
- Bot write workflows function through API with parity to current behavior.
- Upsert semantics are fully covered by tests (create path + update path).

## Phase 4 - Shared/Bot-Only Reads (Week 4)
Deliverables
- `GET /api/v1/users/{userId}/profile` (shared website/bot)
- `GET /api/v1/suggestions` (service-bot only)
- Add policy tests for matrix rules.

Exit criteria
- Scope matrix behavior is enforced by tests and manual checks.

## Phase 5 - Hardening and Release Prep (Week 5)
Deliverables
- Structured logging (request id, principal, route, status, duration).
- Basic rate limiting policies by route group and principal.
- CI checks for build + tests + OpenAPI validation.
- Deployment profile for your host environment.

Exit criteria
- API is deployable and observable in target environment.
- Rollback plan and config docs are written.

## 5) Testing Strategy
- Unit tests for validators and policy decisions.
- Integration tests per endpoint (happy path + auth + validation + not found).
- Contract checks for OpenAPI drift.
- Regression tests for nomination upsert behavior and paging precedence.

## 6) Migration Strategy
- Migrate bot commands in small batches by endpoint domain.
- Keep feature toggles per command path during transition.
- Remove direct Oracle paths only after parity verification.

## 7) Risks and Mitigations
- Risk: auth misconfiguration blocks bot.
  - Mitigation: early auth integration tests + local token generator script.
- Risk: Oracle query performance for search.
  - Mitigation: query tuning and indexing review before full cutover.
- Risk: scope mistakes expose data.
  - Mitigation: policy matrix tests for each protected route.

## 8) Definition of Done (Per Endpoint)
- OpenAPI entry and examples updated.
- Validation and ProblemDetails behavior implemented.
- Authorization policy applied and tested.
- Integration tests added.
- Bot command path migrated (where applicable).

## 9) Immediate Next Steps
1. Scaffold solution/projects and baseline middleware.
2. Implement auth/policy plumbing and ProblemDetails.
3. Build Priority Read endpoints first.