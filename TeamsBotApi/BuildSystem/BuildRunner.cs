using System;
using System.Threading.Tasks;

namespace BuildSystem
{
    public class BuildRunner
    {
        private Build activeBuild;

        public bool IsIdle => activeBuild == null;

        public async Task RunBuild(Build build, Action<Build> onComplete)
        {
            activeBuild = build;
            await build.StartAsync();
            onComplete(build);
        }
    }
}