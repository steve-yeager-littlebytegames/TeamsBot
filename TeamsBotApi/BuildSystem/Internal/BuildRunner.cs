using System.Threading.Tasks;

namespace BuildSystem
{
    public class BuildRunner
    {
        public string Name { get; }
        public bool IsIdle { get; private set; } = true;
        public Build ActiveBuild { get; private set; }

        public BuildRunner(string name)
        {
            Name = name;
        }

        public async Task RunBuild(Build build)
        {
            IsIdle = false;
            ActiveBuild = build;
            try
            {
                await build.StartAsync();
            }
            finally
            {
                IsIdle = true;
            }
        }
    }
}