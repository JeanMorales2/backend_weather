# Car Brands API

REST API that exposes a catalog of car brands stored in PostgreSQL. It is built with **ASP.NET Core 10** and **Entity Framework Core 10**, following a **hexagonal (Clean) architecture**: the business core does not depend on any framework, and all technology lives in interchangeable adapters.

## Features

- Lists every brand, ordered by name.
- Fetches a single brand by id (returns 404 when it does not exist).
- Reports its own availability and the database's (`/health`).
- Documents and lets you try every endpoint from Swagger UI.
- Creates the schema and loads **20 seed brands** automatically on startup.

## Endpoints

| Method | Route | Description | Responses |
|---|---|---|---|
| `GET` | `/api/marcasautos` | All brands, ordered by name | `200` |
| `GET` | `/api/marcasautos/{id}` | One brand by id | `200`, `404` |
| `GET` | `/health` | API and PostgreSQL status | `200` |
| `GET` | `/swagger` | Interactive documentation (OpenAPI) | `200` |

Example response for `GET /api/marcasautos/1`:

```json
{ "id": 1, "nombre": "Toyota", "paisOrigen": "Japón", "anioFundacion": 1937, "activa": true }
```

The JSON field names are in Spanish because they come from the domain language of the exercise: `nombre` (name), `paisOrigen` (country of origin), `anioFundacion` (founding year) and `activa` (active).

## Seed data

The `MarcasAutos` table is created with 20 brands through `HasData`, deterministically and with stable keys (1 to 20):

| Id | Brand | Country | Founded |
|---|---|---|---|
| 1 | Toyota | Japan | 1937 |
| 2 | Ford | United States | 1903 |
| 3 | Volkswagen | Germany | 1937 |
| 4 | Honda | Japan | 1948 |
| 5 | Chevrolet | United States | 1911 |
| 6 | Nissan | Japan | 1933 |
| 7 | BMW | Germany | 1916 |
| 8 | Mercedes-Benz | Germany | 1926 |
| 9 | Audi | Germany | 1909 |
| 10 | Hyundai | South Korea | 1967 |
| 11 | Kia | South Korea | 1944 |
| 12 | Mazda | Japan | 1920 |
| 13 | Subaru | Japan | 1953 |
| 14 | Suzuki | Japan | 1909 |
| 15 | Mitsubishi | Japan | 1870 |
| 16 | Renault | France | 1899 |
| 17 | Peugeot | France | 1810 |
| 18 | Fiat | Italy | 1899 |
| 19 | Volvo | Sweden | 1927 |
| 20 | Tesla | United States | 2003 |

The first six ship in the `InitialCreate` migration and the other fourteen in `AmpliarCatalogoMarcas`, so an already migrated database is upgraded without losing data. The country values are stored in Spanish (for example `Japón`, `Alemania`).

## Architecture

### Request flow

```
HTTP client
     ↓
Input adapter           MarcasAutosController
     ↓
Input port              IListarMarcasAutosUseCase / IObtenerMarcaAutoPorIdUseCase
     ↓
Application + Domain    ListarMarcasAutos / ObtenerMarcaAutoPorId  ·  MarcaAuto
     ↓
Output port             IMarcaAutoRepository
     ↓
Output adapter          MarcaAutoRepository (EF Core + Npgsql)
     ↓
PostgreSQL
```

### Layers and dependency rule

Each layer is a separate .NET project. References only point inward, and the compiler prevents violating that.

```
src/
├── MarcasAutos.Domain/            Business core. References nothing.
│   ├── Entities/                  MarcaAuto
│   ├── ValueObjects/              (reserved)
│   ├── Services/                  (reserved: complex domain rules)
│   ├── Events/                    (reserved: domain events)
│   └── Ports/                     (reserved: interfaces the domain may need)
│
├── MarcasAutos.Application/       Use cases. References only Domain.
│   ├── UseCases/                  ListarMarcasAutos, ObtenerMarcaAutoPorId
│   ├── Dtos/                      MarcaAutoDto
│   └── Ports/
│       ├── In/                    Use case contracts (consumed by the controller)
│       └── Out/                   IMarcaAutoRepository (implemented by infrastructure)
│
├── MarcasAutos.Infrastructure/    Adapters and technical details. References Application and Domain.
│   ├── Adapters/
│   │   ├── In/Http/               MarcasAutosController
│   │   └── Out/Persistence/       MarcaAutoRepository
│   ├── Persistence/               AppDbContext, Configurations/, Migrations/, DatabaseExtensions
│   ├── Config/                    Dependency registration (AddApplication, AddInfrastructure)
│   └── Framework/                 Controllers, Swagger and health checks
│
└── MarcasAutos.Main/              Composition root: Program.cs, appsettings and Dockerfile.
```

### Main pieces

- **`MarcaAuto`** (Domain): entity with `Id`, `Nombre`, `PaisOrigen`, `AnioFundacion` and `Activa`. It carries no EF attributes; its mapping lives in the infrastructure layer.
- **Use cases** (Application): orchestrate the query against the output port and return `MarcaAutoDto`. They know nothing about HTTP or SQL.
- **`MarcaAutoRepository`** (Infrastructure): implements the output port with EF Core. It uses `AsNoTracking` because it only reads, and orders by name.
- **`MarcaAutoConfiguration`**: defines the table, column lengths (name 100, country 80), the unique index on the name and the seed data.
- **`DatabaseExtensions`**: applies migrations on startup and retries up to 10 times, because in Docker the API can start before PostgreSQL accepts connections. If the attempts run out, startup fails visibly.
- **`Program.cs`** (Main): wires ports to adapters, applies migrations, enables Swagger and maps the controllers and the health check.

## Running with Docker

Requirements: Docker Desktop with Compose.

```sh
cp .env.example .env        # set POSTGRES_PASSWORD
docker compose up --build
```

- API: <http://localhost:8080>
- Swagger: <http://localhost:8080/swagger>
- Health: <http://localhost:8080/health>

`docker-compose.yml` defines two services: **`db`** (PostgreSQL 17 with a health check and a persistent volume) and **`api`** (built from `src/MarcasAutos.Main/Dockerfile`, it waits until the database is healthy). Inside the Compose network the API connects with `Host=db`; port 5432 is not published outside Docker.

### Environment variables

| Variable | Default | Description |
|---|---|---|
| `POSTGRES_USER` | `marcas` | PostgreSQL user |
| `POSTGRES_PASSWORD` | `marcas_local` | PostgreSQL password (change it) |
| `POSTGRES_DB` | `marcasautos` | Database name |
| `API_PORT` | `8080` | Published API port |
| `SWAGGER_ENABLED` | `true` | Enables Swagger in production (`Swagger__Enabled` in the API) |
| `ConnectionStrings__Default` | built by Compose | API connection string |

When `Swagger:Enabled` is not set, Swagger is only shown in the Development environment.

### Deploying with Dokploy

1. Create a **Docker Compose** application and load this folder.
2. Under **Environment**, set `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB`, `API_PORT` and `SWAGGER_ENABLED`.
3. Point the domain at the `api` service, internal port `8080`.
4. Check `https://your-domain/health` and `https://your-domain/api/marcasautos`.

## Running locally without Docker

Requirements: .NET 10 SDK and a PostgreSQL instance.

```sh
export ConnectionStrings__Default="Host=localhost;Port=5432;Database=marcasautos;Username=marcas;Password=marcas"
dotnet run --project src/MarcasAutos.Main
```

The application applies migrations on startup. In Development, Swagger UI is served at `/swagger`.

## Migrations

There are two migrations: `InitialCreate` (table, constraints and six brands) and `AmpliarCatalogoMarcas` (fourteen more brands). To create a new one:

```sh
dotnet tool restore
dotnet ef migrations add MigrationName \
  --project src/MarcasAutos.Infrastructure \
  --startup-project src/MarcasAutos.Main
```

## Tests and coverage

```sh
dotnet test --collect:"XPlat Code Coverage"
```

`coverlet.runsettings` is applied automatically from the test project and excludes the generated migrations and `Program.cs`. The Cobertura report is written to `tests/MarcasAutos.Tests/TestResults/`. The suite comfortably exceeds the required 70%: it covers 100% of the executable lines and branches.

The tests mirror the layer structure and need no PostgreSQL:

| Test | What it verifies |
|---|---|
| Use cases | Orchestration and DTO mapping with a fake repository |
| Repository | Ordering by name and lookup by id with EF InMemory |
| Controller | `200` with a list, `200` with one item and `404`, with a fake use case |
| EF configuration | Table, lengths, unique index and 20 seeds with no repeated names |
| Migrations on startup | Retries, final error and success path |
| Dependency wiring | Registration of use cases, repository, Swagger and health checks |

## Technical decisions

- **Hexagonal:** business logic is tested without a database, and persistence can be replaced without touching the use cases.
- **One project per layer:** the dependency rule is enforced by the compiler, not by team discipline.
- **Output DTO:** the API never exposes the entity directly.
- **Seed with `HasData`:** the catalog is identical on every installation and is versioned with the migrations.
- **Migrations on startup with retries:** the API comes up on its own with `docker compose up`.
- **No `Server` header:** the web server is not advertised in responses.
