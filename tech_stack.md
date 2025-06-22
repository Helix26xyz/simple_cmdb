# Recommended Technology Stack for MVP CMDB Application

## Frontend
- **Framework:** React (with TypeScript)
- **UI Library:** Material-UI (MUI) or Ant Design
- **State Management:** Redux Toolkit or Zustand
- **Auth:** oidc-client-js or Auth0 React SDK
- **WebSockets:** native WebSocket API or socket.io-client

## Backend API
- **Language/Framework:** C# (.NET 8 Web API)
- **Auth:** Microsoft.AspNetCore.Authentication.OpenIdConnect (OIDC middleware)
- **WebSockets:** SignalR (.NET)
- **API Rate Limiting:** AspNetCoreRateLimit (C#)
- **Plugin System:** .NET MEF/Assembly loading for plugins and sandboxed scripting
- **Business Rules:** Custom plugin loader with lifecycle hooks

## API Documentation
- **Requirement:** All backend API endpoints must be documented using OpenAPI (Swagger), with interactive documentation available for developers and integrators.
- **Tools:** .NET 8 Web API provides automatic OpenAPI/Swagger generation and UI out of the box.

## Database
- **Primary DB:** PostgreSQL (with JSONB columns for extensibility)
- **ORM:** Entity Framework Core (C#)
- **Audit Logging:** Separate audit table(s) in PostgreSQL

## Search
- **Engine:** OpenSearch (self-managed or managed service)

## Observability
- **Tracing/Metrics/Logs:** OpenTelemetry SDKs (.NET and React)
- **Stack:** Prometheus (metrics), Grafana (dashboards), Loki (logs)

## CI/CD & Deployment
- **Containerization:** Docker
- **Orchestration:** Kubernetes (with Kustomize overlays)
- **CI/CD:** GitHub Actions or GitLab CI
- **Registry:** Docker Hub or GitHub Container Registry

## Other Integrations
- **OIDC Provider:** Auth0, Okta, or your organization’s IdP
- **Webhooks:** Outbound HTTP(s) with retry logic
- **Plugin Management:** UI and backend support for upload, enable/disable, and versioning
