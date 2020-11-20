using System;
using System.Threading.Tasks;

namespace BuildSystem
{
    internal class BuildRunner
    {
        private Build activeBuild;

        public bool IsIdle { get; private set; } = true;

        public async Task RunBuild(Build build, Action<Build> onComplete)
        {
            IsIdle = false;
            activeBuild = build;
            try
            {
                await build.StartAsync();
            }
            finally
            {
                IsIdle = true;
                onComplete(build);
            }
        }
    }
}