namespace simplecmdb.SharedModels.models
{
    // Represents a type of configuration item (e.g., Server, Application, NetworkDevice)
    public class CmdbType
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Optionally, a JSON schema for attributes
        public string? AttributeSchemaJson { get; set; }
    }

    // Represents a CMDB record (instance of a CmdbType)
    public class CmdbRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CmdbTypeId { get; set; }
        public CmdbType? CmdbType { get; set; }
        // Store attributes as JSONB in PostgreSQL
        public string AttributesJson { get; set; } = "{}";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
