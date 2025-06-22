using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    // Represents a CMDB entity type (e.g., server, computer, etc.)
    public class CmdbEntityType
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // e.g., "cmdb_ci_server"

        public string? Description { get; set; }

        // Optionally, store a JSON schema or metadata for this type
        public string? Schema { get; set; }
    }
}
