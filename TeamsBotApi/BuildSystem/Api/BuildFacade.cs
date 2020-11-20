using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildSystem.Api
{
    public class BuildFacade : IBuildCreator, IBuildQueue
    {
        private readonly BuildFactory buildFactory;
        private readonly BuildMonitor buildMonitor = new BuildMonitor();

        public event StageCompleteDelegate StageCompleteEvent;

        public IReadOnlyCollection<Build> QueuedBuilds => buildMonitor.QueuedBuilds;

        public BuildFacade(IBuildRepository buildBuildRepository = null)
        {
            buildBuildRepository ??= new InMemoryBuildRepository();

            buildFactory = new BuildFactory(buildBuildRepository);
        }

        public async Task<Build> CreateBuildAsync(string definitionName)
        {
            var build = await buildFactory.CreateBuildAsync(definitionName);
            buildMonitor.QueueBuild(build);
            return build;
        }
    }
}