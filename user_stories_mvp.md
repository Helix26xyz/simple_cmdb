# MVP User Stories for Custom CMDB Application

## Epic: Core CMDB Functionality

### 1. As an authenticated user, I can log in via OIDC and be assigned to a group/role so that my access is managed securely.

### 2. As an admin, I can create, update, and delete CMDB tables/entities (with base/extended fields) via the UI so that I can model my organization's assets.

### 3. As a user, I can create, read, update, and delete individual CMDB records in any table I have access to, via the UI and API.

### 4. As an admin, I can add, edit, or remove fields/attributes from existing tables/entities via the UI so that the schema can evolve as needed.

### 5. As a user, I can see real-time updates to records and tables I am viewing so that I always have the latest information.

### 6. As an admin, I can export the current configuration (tables, fields, schemas) to a portable format (e.g., JSON) and import it into another instance, with validation and conflict resolution.

### 7. As an admin, I can register webhook endpoints and select which events (table/record create, update, delete) trigger webhooks, so that external systems can be notified of changes.

### 8. As a user, I can search for CMDB records using full-text search via the UI and API so that I can quickly find relevant data.

### 9. As an admin, I can view and adjust API rate limiting settings via the UI to ensure fair usage and prevent abuse.

### 10. As an admin, I can install and manage plugins that add new UI components, API endpoints, or business rules, and author scripts for CMDB record lifecycle events (onCreate, onUpdate, onDelete) in a secure, sandboxed environment.

---

These user stories cover the MVP for the core CMDB functionality, including extensible schema, CRUD, real-time updates, configuration management, webhooks, search, rate limiting, and extensibility. Additional stories can be added for advanced features as the project evolves.
