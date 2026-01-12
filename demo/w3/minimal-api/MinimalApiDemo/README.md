
# MinimalApiDemo

Minimal API för demo av statuskoder och enkla felobjekt (utan ProblemDetails).

## Bygga & köra
```bash
cd MinimalApiDemo
 dotnet restore
 dotnet run
```
API startar normalt på `http://localhost:5280` och `https://localhost:7280` (se `Properties/launchSettings.json`).

## Endpoints
- `GET /items` → `200 OK`
- `GET /items/{id}` → `200 OK` eller `404 NotFound`
- `POST /items` (body: `{ "name": "Text", "isDone": false }`) → `201 Created`, `400 BadRequest`, eller `409 Conflict`
- `GET /items/slow?ms=5000` → `200 OK` efter fördröjning (använd för timeout/cancellation i klienten)

## Avgränsning
- Ingen EF Core, ingen autentisering, ingen global exception middleware.
- Felobjekt är enkla `{ error: "..." }`.
