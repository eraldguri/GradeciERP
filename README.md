# GradeciERP Docker Setup

## Run with Docker Compose

From the project root:

```bash
docker compose up --build
```

This starts:

- `mssql` inside the Docker network (service name: `mssql`)
- `webapi` on `http://localhost:8088`

## Stop the stack

```bash
docker compose down
```

To also remove persisted SQL Server data volume:

```bash
docker compose down -v
```

## Notes

- SQL Server credentials used by compose:
  - User: `sa`
  - Password: `TiranaBruxelles123@`
- SQL Server is intentionally not published to a host port to avoid conflicts with local SQL Server instances already using `1433`.
- The API uses the service hostname `mssql` as DB host inside Docker network.
- On first run, database initialization/seeding may take a bit longer while SQL Server starts.