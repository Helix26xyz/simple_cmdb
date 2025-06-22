using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class CmdbContext : DbContext
    {
        public CmdbContext(DbContextOptions<CmdbContext> options) : base(options) { }

        public DbSet<CmdbEntity> CmdbEntities { get; set; } = null!;
        public DbSet<CmdbEntityType> CmdbEntityTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration if needed
        }
    }
}
