using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildSystem.Api;

namespace BuildSystem
{
    internal class InMemoryBuildMetadataRepository : IBuildMetadataRepository
    {
        private readonly List<BuildMetadata> repository = new List<BuildMetadata>();

        public Task SaveAsync(BuildMetadata entity)
        {
            repository.Add(entity);
            return Task.CompletedTask;
        }

        public Task<BuildMetadata> LoadAsync(string definitionName)
        {
            var metadata = repository.First(md => md.DefinitionName == definitionName);
            return Task.FromResult(metadata);
        }
    }
}