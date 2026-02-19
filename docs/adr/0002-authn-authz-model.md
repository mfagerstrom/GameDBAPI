# ADR 0002: Initial Authentication and Authorization Model

- Status: Accepted (Week 0)
- Date: 2026-02-19

## Context
Two clients need access patterns with different risk profiles:
- Discord bot service integration.
- Companion website with user sessions.

## Decision
- Support JWT bearer authentication as the primary long-term model.
- Start the Discord bot on JWT service-principal authentication from v1.
- Implement policy-based authorization with normalized roles/scopes:
  - `service-bot`
  - `member`
  - `moderator`
  - `admin`
- Enforce subject ownership checks for user-owned mutations unless elevated role.

## Rationale
- JWT model aligns with future website requirements.
- Starting with JWT avoids dual-auth complexity and removes API key lifecycle risk.
- Policy-based authorization keeps role logic centralized.

## Consequences
Positive
- Clear migration path from service-only access to mixed client ecosystem.
- Lower chance of authorization drift across controllers.

Tradeoff
- Bot deployment must provision and rotate JWT signing/verification configuration immediately.

## Follow-up
- Add auth integration tests per policy and route group.
- Document token claims contract in API docs before first release.
