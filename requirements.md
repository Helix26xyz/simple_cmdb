# Custom CMDB Application Requirements

## Functional Requirements

1. **Extensible Database Table Pattern**
   - The system must implement a flexible, extensible schema, inspired by ServiceNow, to support:
     - Definition of base tables/entities (e.g., "cmdb_ci", "cmdb_ci_server").
     - Inheritance or extension of base tables to create specialized tables/entities with additional fields.
     - Dynamic addition of custom fields/attributes to any table/entity at runtime, without requiring database migrations or downtime.
     - Storage of both standard and custom attributes in a scalable, queryable manner (e.g., EAV pattern, JSONB columns, or similar).
     - Support for field-level metadata (type, validation, display name, etc.).

2. **CRUD Interface**
   - Provide a user-friendly web interface and RESTful API for:
     - Creating, reading, updating, and deleting individual CMDB records.
     - Managing (creating, updating, deleting) CMDB tables/entities themselves.
     - Bulk operations (import/export, batch updates, etc.).
     - Validation and error handling for all CRUD operations.

3. **Dynamic Table Creation**
   - Users must be able to:
     - Define new CMDB tables/entities from the UI, specifying base table (for inheritance), name, and initial fields.
     - Add, edit, or remove fields/attributes from existing tables/entities via the UI.
     - Set field types, validation rules, and display properties.
     - View a list of all tables/entities and their schemas.

4. **Real-Time Updates**
   - The system must:
     - Use websockets, SignalR, or a similar technology to push updates to all clients viewing a record or table when changes occur.
     - Support real-time notifications for record changes, table schema changes, and user actions (e.g., creation, deletion).
     - Ensure data consistency and handle concurrent updates gracefully.

5. **User/API Token Support**
   - Implement robust authentication and authorization:
     - Support user accounts with role-based access control (RBAC) for different actions (view, edit, admin, etc.).
     - Provide API token management for programmatic access, with the ability to create, revoke, and scope tokens.
     - Audit logging for all user and API actions.

6. **Configuration Export/Import**
   - The system must support exporting and importing CMDB configurations, including:
     - Ability for users to export the current configuration (tables, fields, schemas, and possibly data) to a portable format (e.g., JSON, YAML).
     - Ability to import a configuration file into another instance of the application, provided it is the same or compatible version.
     - Validation and conflict resolution during import (e.g., handling existing tables, fields, or data).
     - Support for versioning and tracking of configuration exports/imports for audit and rollback purposes.

7. **Event-Driven Alerting and Webhooks**
   - The system must support event-driven alerting by sending webhooks on key events, including:
     - Table creation, update, or deletion.
     - Record creation, update, or deletion.
     - Schema changes or configuration updates.
   - Users should be able to:
     - Register and manage webhook endpoints via the UI or API.
     - Select which events trigger webhooks for each endpoint.
     - Receive event payloads in a standard, documented format (e.g., JSON).
     - View delivery status and logs for webhook events.
   - The system should support retry logic and error handling for failed webhook deliveries.

8. **Plugin and Extensibility Framework**
   - The system should provide a plugin architecture to allow for easy extensibility and customization.
   - Support for user- or admin-installed plugins that can:
     - Add new UI components, API endpoints, or integrations.
     - Define custom business logic and automation (similar to ServiceNow Business Rules).
   - CMDB records and tables should support scripting hooks for lifecycle events, such as:
     - `onCreate`, `onUpdate`, `onDelete`, etc.
     - Scripts can be authored in a secure, sandboxed environment and managed via the UI.
   - Plugins and scripts should be versioned, auditable, and have access controls.

## Non-Functional Requirements

1. **Kubernetes-First Architecture**
   - All components (frontend, backend, database, websockets, etc.) must:
     - Be containerized and deployable as Kubernetes workloads (Deployments, StatefulSets, etc.).
     - Use Kubernetes-native configuration (ConfigMaps, Secrets, etc.).
     - Support horizontal scaling and high availability.
     - Provide health checks, readiness/liveness probes, and resource requests/limits.

2. **CI/CD Automation**
   - The project must include:
     - Automated build and test pipelines (e.g., GitHub Actions, GitLab CI, Azure Pipelines).
     - Container image builds and publishing to a registry.
     - Automated deployment to Kubernetes using Kustomize overlays for different environments (dev, staging, prod).
     - Rollback and promotion strategies for safe releases.
     - Automated database migrations (if needed) as part of the pipeline.

3. **Observability and Monitoring**
   - The system must support OpenTelemetry for distributed tracing, metrics, and logging.
   - Integration with existing observability stack (Grafana, Loki, Prometheus) for monitoring, alerting, and log aggregation.
   - Provide dashboards and alerts for key system health and usage metrics.

4. **Search Capabilities**
   - The system should provide full-text search functionality using OpenSearch.
   - Support indexing of CMDB records and configuration data for fast, flexible search and filtering.
   - Expose search via both the UI and API.

5. **Authentication and Authorization**
   - Users must authenticate via OIDC (OpenID Connect), supporting integration with external identity providers.
   - Users are assigned to groups/roles within the application for RBAC.
   - Field-level permissions are desirable for future implementation, but not required for initial release.

6. **API Rate Limiting and Throttling**
   - The API must implement basic rate limiting and throttling to prevent abuse and ensure fair usage.
   - Rate limiting policies should be configurable, with sensible defaults.
   - Administrators should be able to view and adjust rate limiting settings via the UI.
   - The system should provide clear error messages and logs when rate limits are exceeded.

## Other Notes

- Technology choices (database, backend, etc.) are flexible, but must support scalability, high availability, and extensibility.
- The system should be designed for easy integration with external systems (e.g., ITSM, monitoring, discovery tools) via APIs or webhooks.
- Documentation and onboarding guides should be provided for both end-users and developers.
