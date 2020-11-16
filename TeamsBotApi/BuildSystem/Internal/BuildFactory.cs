using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildSystem.Api;

namespace BuildSystem
{
    internal class BuildFactory : IBuildCreator
    {
        private readonly IBuildRepository buildRepository;

        private readonly IReadOnlyCollection<BuildDefinition> buildDefinitions = new[]
        {
            new BuildDefinition("Client", "Pull", "Compile", "Assets", "Test", "Upload"),
            new BuildDefinition("Server", "Pull", "Compile", "Test", "Deploy", "Acceptance Tests"),
        };

        public BuildFactory(IBuildRepository buildRepository)
        {
            this.buildRepository = buildRepository;

            // TODO: Only if not seeded.
            foreach(var buildDefinition in buildDefinitions)
            {
                buildRepository.SaveAsync(new BuildMetadata(buildDefinition.Name)).GetAwaiter().GetResult();
            }
        }

        public async Task<Build> CreateBuildAsync(string definitionName)
        {
            var buildDefinition = buildDefinitions.First(bd => bd.Name.ToLower() == definitionName.ToLower());

            var metaData = await buildRepository.LoadAsync(definitionName);
            ++metaData.BuildCount;

            var build = new Build(definitionName, metaData.BuildCount, buildDefinition.StageNames.Select(sn => new Stage(sn)).ToArray());

            await buildRepository.SaveAsync(metaData);

            return build;
        }
    }
}