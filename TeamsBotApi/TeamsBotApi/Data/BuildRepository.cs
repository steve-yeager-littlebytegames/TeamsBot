using System.Threading.Tasks;
using BuildSystem;
using BuildSystem.Api;
using Microsoft.EntityFrameworkCore;

namespace TeamsBotApi.Data
{
    public class BuildRepository : IBuildRepository
    {
        private readonly BuildDbContext buildDb;

        public BuildRepository(BuildDbContext buildDb)
        {
            this.buildDb = buildDb;
        }

        public async Task SaveAsync(BuildMetadata entity)
        {
            buildDb.Update(entity);
            await buildDb.SaveChangesAsync();
        }

        public async Task<BuildMetadata> LoadAsync(string definitionName)
        {
            var metadata = await buildDb.Metadata.FirstAsync(md => md.DefinitionName.ToLower() == definitionName);
            return metadata;
        }
    }
}