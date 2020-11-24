using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildSystem.Api
{
    public class BuildFacade : IBuildCreator, IBuildQueue
    {
        private readonly BuildFactory buildFactory;
        private readonly BuildMonitor buildMonitor = new BuildMonitor();

        public event BuildCreatedDelegate BuildCreatedEvent;
        public event StageUpdateDelegate StageUpdateEvent;
        public event BuildUpdateDelegate BuildUpdateEvent;

        public IReadOnlyCollection<Build> QueuedBuilds => buildMonitor.QueuedBuilds;
        public IReadOnlyCollection<BuildRunner> Agents => buildMonitor.Agents;

        public BuildFacade(IBuildRepository buildBuildRepository = null)
        {
            buildBuildRepository ??= new InMemoryBuildRepository();

            buildFactory = new BuildFactory(buildBuildRepository);

            buildMonitor.StageUpdateEvent += OnStageComplete;
            buildMonitor.BuildUpdateEvent += OnBuildComplete;
        }

        private async Task OnStageComplete(Stage stage)
        {
            if(StageUpdateEvent != null)
            {
                await StageUpdateEvent?.Invoke(stage);
            }
        }

        private async Task OnBuildComplete(Build build)
        {
            if(BuildUpdateEvent != null)
            {
                await BuildUpdateEvent?.Invoke(build);
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