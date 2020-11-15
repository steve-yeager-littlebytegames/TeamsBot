using System;
using System.Threading.Tasks;

namespace BuildSystem
{
    public class BuildRunner
    {
        private Build activeBuild;

        public bool IsIdle { get; private set; } = true;

        public async Task RunBuild(Build build, Action<Build> onComplete)
        {
            IsIdle = false;
            activeBuild = build;
            await build.StartAsync();
            IsIdle = true;
            onComplete(build);
        }
    }
}