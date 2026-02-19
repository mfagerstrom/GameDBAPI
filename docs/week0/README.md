# Week 0 Kickoff Plan

## Goal
Define and lock the first API contract for the Discord bot migration and the future companion website before writing implementation code.

## Scope for Week 0
- Define the first migration slice from bot direct-SQL to HTTP API.
- Lock API baseline standards for URL structure, versioning, errors, and auth.
- Draft a v1 contract for high-value read/write operations.
- Define non-functional guardrails (validation, logging, tracing, rate limits, test gates).

## Out of Scope for Week 0
- No ASP.NET project scaffolding yet.
- No Oracle repository implementation yet.
- No deployment pipeline yet.

## Domain Inventory (from local DB docs)
- Game catalog: `GAMEDB_GAMES`, releases, platforms, regions, synonym groups.
- GOTM: `GOTM_NOMINATIONS`, `GOTM_ENTRIES`, `NR_GOTM_*`.
- Users and profiles: `RPG_CLUB_USERS` and history tables.
- Collections: `USER_GAME_COLLECTIONS`, imports.
- Community/admin: `RPG_CLUB_SUGGESTIONS`, `RPG_CLUB_TODOS`.

## Endpoint Prioritization
Priority 1 (bot-critical, high read volume)
- `GET /api/v1/games/search`
- `GET /api/v1/games/{gameId}`
- `GET /api/v1/platforms`
- `GET /api/v1/gotm/rounds/{roundNumber}/nominations`

Priority 2 (bot writes)
- `PUT /api/v1/gotm/rounds/{roundNumber}/nominations/users/{userId}` (upsert)
- `DELETE /api/v1/gotm/rounds/{roundNumber}/nominations/{nominationId}`
- `POST /api/v1/collections/entries`
- `PATCH /api/v1/collections/entries/{entryId}`

Priority 3a (shared website/bot)
- `GET /api/v1/users/{userId}/profile`

Priority 3b (bot-only)
- `GET /api/v1/suggestions`

## Baseline API Standards
- URL style: plural nouns and nested resources where relationship is explicit.
- Versioning: path-based (`/api/v1`).
- DTO boundaries: never expose Oracle table rows directly.
- Error format: RFC 9457 `application/problem+json`.
- Date format: ISO 8601 in UTC.
- Paging:
  - `page` default `1`
  - `pageSize` default `25`, hard max `100`
  - `botPageSize` default `10`, hard max `100`
- Filtering and sorting: explicit allow-list per endpoint.
- Idempotency: apply to retry-prone write endpoints used by Discord workflows.

## Security Model (initial)
- Internal bot client: JWT service principal.
- User-facing site client: JWT bearer (OIDC-ready design).
- Authorization: policy-based roles (`admin`, `moderator`, `member`, `service-bot`).
- CORS: explicit allow-list only.

## Draft Read Scope Matrix (Website + Bot)
Legend
- `Y`: allowed
- `N`: denied

| Route group / resource | member | moderator | admin | service-bot |
| --- | --- | --- | --- | --- |
| `GET /games/*` | Y | Y | Y | Y |
| `GET /platforms` | Y | Y | Y | Y |
| `GET /gotm/rounds/*/nominations` | Y | Y | Y | Y |
| `GET /users/{userId}/profile` | Y | Y | Y | Y |
| `GET /collections/entries` | Y | Y | Y | Y |
| `GET /collections/users/{userId}/entries` | Y | Y | Y | Y |
| `GET /suggestions` | N | N | N | Y |
| `GET /admin/*` | N | N | Y | Y |

Policy notes
- `admin` includes all `moderator` read permissions where moderator access exists.
- `service-bot` can read all current v1 resources used for bot workflows, including bot-only resources.
- Future public website endpoints can still allow anonymous access where needed.

## Observability and Reliability Guardrails
- Correlation ID per request.
- Structured logs with route, status, latency, and principal.
- Health checks: liveness and Oracle readiness.
- Rate limiting by client type and route group.

## Week 0 Deliverables
- This kickoff scope doc.
- `docs/week0/api-v1-contract-draft.md`.
- `docs/openapi/openapi.v1.draft.yaml`.
- ADRs for baseline API style and auth.

## Acceptance Criteria
- v1 endpoint list and semantics are finalized.
- Error contract and auth direction are finalized.
- At least one example request and response exists per Priority 1 endpoint.
- Open questions are captured with resolution notes.

## Open Questions
- None currently.

## Week 0 Status
- Week 0 complete.
