using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildSystem.Api;

namespace BuildSystem
{
    internal class BuildMonitor : IBuildQueue
    {
        private readonly Queue<Build> queuedBuilds = new Queue<Build>();

        public event StageUpdateDelegate StageUpdateEvent;
        public event BuildUpdateDelegate BuildUpdateEvent;

        public IReadOnlyCollection<Build> QueuedBuilds => queuedBuilds;
        public IReadOnlyCollection<BuildRunner> Agents { get; } = new[] {new BuildRunner("Windows Agent 1"), new BuildRunner("OSX Agent 1"), new BuildRunner("Linux Agent 1")};

        public void QueueBuild(Build build)
        {
            var availableBuildRunner = Agents.FirstOrDefault(br => br.IsIdle);
            if(availableBuildRunner != null)
            {
                RunBuild(build, availableBuildRunner);
            }
            else
            {
                queuedBuilds.Enqueue(build);
                build.Queue();
            }
        }

        private void RunBuild(Build build, BuildRunner availableBuildRunner)
        {
            build.StageUpdateEvent += OnStageUpdate;
            build.BuildUpdateEvent += OnBuildUpdate;
            availableBuildRunner.RunBuild(build);
        }

        private async Task OnStageUpdate(Stage stage)
        {
            if(StageUpdateEvent != null)
            {
                await StageUpdateEvent?.Invoke(stage);
            }
        }

        private async Task OnBuildUpdate(Build build)
        {
            if(BuildUpdateEvent != null)
            {
                await BuildUpdateEvent.Invoke(build);
            }

            if(queuedBuilds.Count == 0)
            {
                return;
            }

            var availableBuildRunner = Agents.FirstOrDefault(br => br.IsIdle);
            if(availableBuildRunner != null)
            {
                var nextBuild = queuedBuilds.Dequeue();
                RunBuild(nextBuild, availableBuildRunner);
            }
        }
    }
}