namespace BuildSystem
{
    public class BuildRunner
    {
        public Build ActiveBuild { get; }

        public bool IsIdle => ActiveBuild == null;

        public void RunBuild(Build build)
        {

        }
    }
}