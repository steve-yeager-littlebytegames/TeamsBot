using System.Collections.Generic;

namespace BuildSystem.Api
{
    public interface IBuildQueue
    {
        public event StageCompleteDelegate StageCompleteEvent;
        public event BuildCompleteDelegate BuildCompleteEvent;

        public IReadOnlyCollection<Build> QueuedBuilds { get; }
        public IReadOnlyCollection<BuildRunner> Agents { get; }
    }
}