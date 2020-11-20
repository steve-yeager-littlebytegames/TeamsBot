using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildSystem.Api;

namespace BuildSystem
{
    internal class BuildMonitor : IBuildQueue
    {
        private readonly Queue<Build> queuedBuilds = new Queue<Build>();
        private readonly IReadOnlyCollection<BuildRunner> buildRunners = new[] {new BuildRunner()};

        public event StageCompleteDelegate StageCompleteEvent;
        public event BuildCompleteDelegate BuildCompleteEvent;

        public IReadOnlyCollection<Build> QueuedBuilds => queuedBuilds;

        public void QueueBuild(Build build)
        {
            var availableBuildRunner = buildRunners.FirstOrDefault(br => br.IsIdle);
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
            build.StageCompleteEvent += OnStageComplete;
            build.BuildCompleteEvent += OnBuildComplete;
            availableBuildRunner.RunBuild(build);
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
                await BuildCompleteEvent.Invoke(build);
            }

            if(queuedBuilds.Count == 0)
            {
                return;
            }

            var availableBuildRunner = buildRunners.FirstOrDefault(br => br.IsIdle);
            if(availableBuildRunner != null)
            {
                var nextBuild = queuedBuilds.Dequeue();
                RunBuild(nextBuild, availableBuildRunner);
            }
        }
    }
}