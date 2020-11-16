using System.Threading.Tasks;

namespace BuildSystem.Api
{
    public class BuildSystem : IBuildCreator
    {
        private readonly BuildFactory buildFactory;
        private readonly BuildMonitor buildMonitor = new BuildMonitor();

        public BuildSystem(IBuildMetadataRepository buildBuildMetadataRepository = null)
        {
            buildBuildMetadataRepository ??= new InMemoryBuildMetadataRepository();

            buildFactory = new BuildFactory(buildBuildMetadataRepository);
        }

        public async Task<Build> CreateBuildAsync(string definitionName)
        {
            var build = await buildFactory.CreateBuildAsync(definitionName);
            buildMonitor.QueueBuild(build);
            return build;
        }
    }
}