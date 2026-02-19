# API v1 Contract Draft

## Conventions
- Base path: `/api/v1`
- Content type: `application/json`
- Error type: `application/problem+json`
- Auth header: `Authorization: Bearer <token>` (JWT service principal for bot v1)

## Shared Query Parameters
- `page` (default `1`)
- `pageSize` (default `25`, max `100`)
- `botPageSize` (default `10`, max `100`)
- `sort` (endpoint-specific allow-list)

## 1) Search Games
`GET /api/v1/games/search?query={text}&platformCode={code?}&page=1&pageSize=25&botPageSize=10`

Search paging behavior:
- `pageSize` is the web default paging parameter.
- `botPageSize` is a bot-specific override with default `10`.
- If both are provided, `botPageSize` is used for effective result size.

Response `200`
```json
{
  "items": [
    {
      "gameId": 123,
      "title": "Chrono Trigger",
      "igdbId": 358,
      "thumbnailApproved": true,
      "initialReleaseDate": "1995-03-11",
      "platforms": ["SNES", "PC"]
    }
  ],
  "page": 1,
  "pageSize": 25,
  "totalCount": 1
}
```

## 2) Get Game Details
`GET /api/v1/games/{gameId}`

Response `200`
```json
{
  "gameId": 123,
  "title": "Chrono Trigger",
  "description": "...",
  "igdbId": 358,
  "igdbUrl": "https://www.igdb.com/games/chrono-trigger",
  "totalRating": 95,
  "initialReleaseDate": "1995-03-11",
  "platforms": [
    { "platformCode": "SNES", "platformName": "Super Nintendo Entertainment System" }
  ],
  "releases": [
    {
      "releaseId": 10,
      "platformCode": "SNES",
      "regionCode": "JP",
      "releaseDate": "1995-03-11",
      "format": "Physical"
    }
  ]
}
```

Response `404` (example)
```json
{
  "type": "https://api.rpgclub.local/problems/not-found",
  "title": "Resource not found",
  "status": 404,
  "detail": "Game 999999 was not found.",
  "instance": "/api/v1/games/999999",
  "traceId": "00-bf3..."
}
```

## 3) List Platforms
`GET /api/v1/platforms`

Response `200`
```json
{
  "items": [
    { "platformId": 1, "platformCode": "PC", "platformName": "PC" },
    { "platformId": 2, "platformCode": "PS5", "platformName": "PlayStation 5" }
  ]
}
```

## 4) List GOTM Nominations by Round
`GET /api/v1/gotm/rounds/{roundNumber}/nominations`

Response `200`
```json
{
  "roundNumber": 134,
  "items": [
    {
      "nominationId": 987,
      "userId": "123456789012345678",
      "gameId": 123,
      "gameTitle": "Chrono Trigger",
      "reason": "Classic RPG",
      "nominatedAt": "2026-02-10T14:05:00Z"
    }
  ]
}
```

## 5) Upsert GOTM Nomination
`PUT /api/v1/gotm/rounds/{roundNumber}/nominations/users/{userId}`

Request
```json
{
  "gameId": 123,
  "reason": "Classic RPG"
}
```

Response `201` (created)
- `Location: /api/v1/gotm/rounds/134/nominations/users/123456789012345678`

```json
{
  "nominationId": 987,
  "roundNumber": 134,
  "userId": "123456789012345678",
  "gameId": 123,
  "reason": "Classic RPG",
  "nominatedAt": "2026-02-10T14:05:00Z"
}
```

Response `200` (updated existing nomination for the same round/user)

Possible errors
- `422` if game id does not map to a valid nominatable game.

## 6) Create Collection Entry
`POST /api/v1/collections/entries`

Request
```json
{
  "userId": "123456789012345678",
  "gamedbGameId": 123,
  "platformId": 2,
  "ownershipType": "Digital",
  "note": "From backlog",
  "isShared": true
}
```

Response `201`
```json
{
  "entryId": 45,
  "userId": "123456789012345678",
  "gamedbGameId": 123,
  "platformId": 2,
  "ownershipType": "Digital",
  "note": "From backlog",
  "isShared": true,
  "createdAt": "2026-02-19T18:00:00Z",
  "updatedAt": "2026-02-19T18:00:00Z"
}
```

## 7) ProblemDetails Standard Fields
All non-2xx responses include
- `type`
- `title`
- `status`
- `detail`
- `instance`
- `traceId`
- `errors` (validation failures only)

## Security Matrix (Draft)
- Public read endpoints (`games/*`, `platforms`, `gotm` nominations list): `member`, `moderator`, `admin`, `service-bot`
- Profile and collection read endpoints (`users/{userId}/profile`, `collections/entries`, `collections/users/{userId}/entries`): `member`, `moderator`, `admin`, `service-bot`
- Bot-only read endpoints (`suggestions`): `service-bot`
- Admin read endpoints (`admin/*`): `admin` or `service-bot`
- `gotm` upsert write: `member` (own user id) or `moderator` or `admin` or `service-bot`
- `collections` write: request user must match token subject unless `moderator` or `admin` or `service-bot`

## Open Contract Decisions
- Finalize keyset pagination need for search if offset paging is too slow.
- Decide whether `DELETE nomination` is hard delete or state change.
- Decide route for image/blob retrieval to avoid oversized DTO payloads.
