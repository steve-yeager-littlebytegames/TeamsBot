using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildSystem.Api
{
    public class BuildFacade : IBuildCreator, IBuildQueue
    {
        private readonly BuildFactory buildFactory;
        private readonly BuildMonitor buildMonitor = new BuildMonitor();

        public event StageCompleteDelegate StageCompleteEvent;
        public event BuildCompleteDelegate BuildCompleteEvent;

        public IReadOnlyCollection<Build> QueuedBuilds => buildMonitor.QueuedBuilds;
        public IReadOnlyCollection<BuildRunner> Agents => buildMonitor.Agents;

        public BuildFacade(IBuildRepository buildBuildRepository = null)
        {
            buildBuildRepository ??= new InMemoryBuildRepository();

            buildFactory = new BuildFactory(buildBuildRepository);

            buildMonitor.StageCompleteEvent += OnStageComplete;
            buildMonitor.BuildCompleteEvent += OnBuildComplete;
        }

        private async Task OnStageComplete(Build build, Stage stage)
        {
            if(StageCompleteEvent != null)
            {
                await StageCompleteEvent?.Invoke(build, stage);
            }
        }

        private async Task OnBuildComplete(Build build)
        {
            if(BuildCompleteEvent != null)
            {
                await BuildCompleteEvent?.Invoke(build);
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