using System;
using System.Collections.Generic;
using System.Linq;
using BuildSystem.Api;

namespace BuildSystem
{
    internal class BuildMonitor : IBuildQueue
    {
        private readonly Queue<Build> queuedBuilds = new Queue<Build>();
        private readonly IReadOnlyCollection<BuildRunner> buildRunners = new[] {new BuildRunner()};

        public event Action<Build> BuildCompleteEvent;

        public IReadOnlyCollection<Build> QueuedBuilds => queuedBuilds;

        public void QueueBuild(Build build)
        {
            var availableBuildRunner = buildRunners.FirstOrDefault(br => br.IsIdle);
            if(availableBuildRunner != null)
            {
                availableBuildRunner.RunBuild(build, OnBuildComplete);
            }
            else
            {
                queuedBuilds.Enqueue(build);
            }
        }

        private void OnBuildComplete(Build build)
        {
            BuildCompleteEvent?.Invoke(build);

            if(queuedBuilds.Count == 0)
            {
                return;
            }

            var availableBuildRunner = buildRunners.FirstOrDefault(br => br.IsIdle);
            if(availableBuildRunner != null)
            {
                var nextBuild = queuedBuilds.Dequeue();
                availableBuildRunner.RunBuild(nextBuild, OnBuildComplete);
            }
        }
    }
}