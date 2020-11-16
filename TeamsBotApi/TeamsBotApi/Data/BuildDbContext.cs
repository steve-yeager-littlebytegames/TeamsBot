using System.Threading.Tasks;
using BuildSystem;
using BuildSystem.Api;
using Microsoft.EntityFrameworkCore;

namespace TeamsBotApi.Data
{
    public class BuildDbContext : DbContext, IBuildRepository
    {
        public DbSet<BuildMetadata> Metadata { get; set; }

        public async Task SaveAsync(BuildMetadata entity)
        {
            Add(entity);
            await SaveChangesAsync();
        }

        public async Task<BuildMetadata> LoadAsync(string definitionName)
        {
            var metadata = await Metadata.FirstAsync(md => md.DefinitionName == definitionName);
            return metadata;
        }
    }
}