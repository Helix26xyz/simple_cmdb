# Simple CMDB Backend

This is a .NET 8 Web API backend for a simple, extensible CMDB (Configuration Management Database).

## Features
- Extensible schema using PostgreSQL (EAV/JSONB)
- Entity Framework Core
- OpenAPI/Swagger documentation
- Docker-ready

## Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL
- Docker (optional)

### Build and Run

```bash
cd backend
# Update appsettings.json for your PostgreSQL connection
# Then run:
dotnet build
dotnet run
```

### Run with Docker

```bash
docker build -t simple-cmdb-backend .
docker run -p 8080:80 simple-cmdb-backend
```

## API Documentation
Swagger UI will be available at `/swagger` when running the app.

---

For more details, see the requirements and tech stack documentation in the root of this repository.
