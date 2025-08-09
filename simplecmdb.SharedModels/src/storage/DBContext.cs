using Microsoft.EntityFrameworkCore;
using simplecmdb.SharedModels.models;

namespace simplecmdb.SharedModels.storage
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define your DbSets (tables) here
        // Removed obsolete DbSets for legacy models:
        // public DbSet<SimpleCMDB> SimpleCMDBs { get; set; }
        // public DbSet<SimpleCMDBEvent> SimpleCMDBEvents { get; set; }
        // public DbSet<SimpleCMDBEventStatusEntity> SimpleCMDBEventStatuses { get; set; }
        // public DbSet<SimpleCMDBEventSubStatusEntity> SimpleCMDBEventSubStatuses { get; set; }
        public DbSet<CmdbType> CmdbTypes { get; set; }
        public DbSet<CmdbRecord> CmdbRecords { get; set; }
    }
}
