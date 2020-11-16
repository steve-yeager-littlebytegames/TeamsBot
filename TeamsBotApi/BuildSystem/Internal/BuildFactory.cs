using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildSystem.Api;

namespace BuildSystem
{
    internal class BuildFactory : IBuildCreator
    {
        private readonly IBuildMetadataRepository buildMetadataRepository;

        private readonly IReadOnlyCollection<BuildDefinition> buildDefinitions = new[]
        {
            new BuildDefinition("Client", "Pull", "Compile", "Assets", "Test", "Upload"),
            new BuildDefinition("Server", "Pull", "Compile", "Test", "Deploy", "Acceptance Tests"),
        };

        public BuildFactory(IBuildMetadataRepository buildMetadataRepository)
        {
            this.buildMetadataRepository = buildMetadataRepository;

            foreach(var buildDefinition in buildDefinitions)
            {
                buildMetadataRepository.SaveAsync(new BuildMetadata(buildDefinition.Name)).GetAwaiter().GetResult();
            }
        }

        public async Task<Build> CreateBuildAsync(string definitionName)
        {
            var buildDefinition = buildDefinitions.First(bd => bd.Name.ToLower() == definitionName.ToLower());

            var metaData = await buildMetadataRepository.LoadAsync(definitionName);
            ++metaData.BuildCount;

            var build = new Build(definitionName, metaData.BuildCount, buildDefinition.StageNames.Select(sn => new Stage(sn)).ToArray());

            await buildMetadataRepository.SaveAsync(metaData);

            return build;
        }
    }
}