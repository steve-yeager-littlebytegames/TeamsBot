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

            buildMonitor.StageCompleteEvent += OnStageComplete;
        }

        private async Task OnStageComplete(Build build, Stage stage)
        {
            if(StageCompleteEvent != null)
            {
                await StageCompleteEvent?.Invoke(build, stage);
            }
        }

        public async Task<Build> CreateBuildAsync(string definitionName)
        {
            var build = await buildFactory.CreateBuildAsync(definitionName);
            buildMonitor.QueueBuild(build);
            return build;
        }
    }
}