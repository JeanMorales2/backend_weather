# API de marcas de autos

API REST construida con ASP.NET Core 10, Entity Framework Core 10 y PostgreSQL. El catálogo inicial incluye seis marcas.

## Requisitos

- .NET SDK 10
- Docker Desktop con Compose, para la ejecución en contenedores
- PostgreSQL, para la ejecución local sin Docker

## Ejecución con Docker

Copie `.env.example` a `.env`, ajuste la contraseña y ejecute desde esta carpeta:

```sh
docker compose up --build
```

La API estará disponible en `http://localhost:8080`; el estado de salud se consulta en `/health`.

## Despliegue con Dokploy

1. En Dokploy, cree una aplicación nueva de tipo **Docker Compose** y cargue esta carpeta del proyecto.
2. En la pestaña **Environment**, configure `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB`, `API_PORT` y `SWAGGER_ENABLED`. Use una contraseña segura para PostgreSQL. `API_PORT` puede dejarse en `8080` si no necesita otro puerto publicado.
3. Configure el dominio para que apunte al servicio `api` en el puerto interno `8080`.
4. Cuando el despliegue termine, verifique `https://su-dominio/health` y `https://su-dominio/api/marcasautos`.

Swagger se habilita con `SWAGGER_ENABLED=true` (en la API, `Swagger__Enabled=true`) y queda disponible en `/swagger`. Si no se especifica, se habilita por defecto en Development y se deshabilita en otros entornos.

## Ejecución local

Configure PostgreSQL y la variable `ConnectionStrings__Default`, y luego ejecute:

```sh
dotnet run --project src/MarcasAutos.Api
```

La aplicación aplica las migraciones al iniciar. Para crear una nueva migración:

```sh
dotnet tool restore
dotnet ef migrations add NombreMigracion --project src/MarcasAutos.Api
```

En Development, Swagger UI se sirve en `/swagger`.

## Pruebas y cobertura

```sh
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

El informe Cobertura se genera en `tests/MarcasAutos.Tests/TestResults/`. Para un resumen HTML opcional, instale `dotnet-reportgenerator-globaltool` y use `reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report`.

## Endpoints

- `GET /api/marcasautos`: lista ordenada por nombre.
- `GET /api/marcasautos/{id}`: consulta una marca por identificador (404 si no existe).
- `GET /health`: comprobación de disponibilidad de PostgreSQL.

```sh
curl http://localhost:8080/api/marcasautos
curl http://localhost:8080/api/marcasautos/1
```

## Decisiones técnicas

- El mapeo explícito fija el nombre de tabla y las restricciones de columna; el índice único evita nombres repetidos.
- `HasData` incorpora marcas deterministas en la migración inicial.
- El arranque reintenta la migración durante unos segundos para absorber el tiempo de disponibilidad de PostgreSQL.
- Las consultas usan `AsNoTracking` y devuelven DTOs para limitar los datos expuestos.
- Las pruebas usan bases InMemory aisladas para validar servicio y controlador sin requerir una instancia PostgreSQL.
