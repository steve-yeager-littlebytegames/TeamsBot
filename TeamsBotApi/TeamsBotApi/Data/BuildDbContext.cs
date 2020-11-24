using BuildSystem;
using Microsoft.EntityFrameworkCore;

namespace TeamsBotApi.Data
{
    public class BuildDbContext : DbContext
    {
        public DbSet<BuildMetadata> Metadata { get; set; }
        public DbSet<BuildEntity> Builds { get; set; }
        public DbSet<StageEntity> Stages { get; set; }

        public BuildDbContext(DbContextOptions<BuildDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuildMetadata>().HasKey(nameof(BuildMetadata.DefinitionName));
            modelBuilder.Entity<StageEntity>().HasKey(nameof(StageEntity.BuildId), nameof(StageEntity.Name));
        }
    }
}