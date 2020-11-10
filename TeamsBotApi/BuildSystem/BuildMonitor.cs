using System.Collections.Generic;
using System.Linq;

namespace BuildSystem
{
    public class BuildMonitor
    {
        private readonly Queue<Build> queuedBuilds;
        private readonly IReadOnlyCollection<BuildRunner> buildRunners;

        public void AddBuild(Build build)
        {
            var availableBuildRunner = buildRunners.FirstOrDefault(br => br.IsIdle);
            if(availableBuildRunner != null)
            {
                availableBuildRunner.RunBuild(build);
            }
            else
            {
                queuedBuilds.Enqueue(build);
            }
        }
    }
}