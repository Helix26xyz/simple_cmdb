using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace backend.Models
{
    // Base CMDB entity with extensible attributes (JSONB)
    public class CmdbEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty; // e.g., "cmdb_ci_server"

        // Standard fields (add more as needed)
        public string Name { get; set; } = string.Empty;

        // Extensible attributes stored as JSONB in PostgreSQL
        [Column(TypeName = "jsonb")]
        public JsonObject? Attributes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
