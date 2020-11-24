using System.Collections.Generic;

namespace BuildSystem.Api
{
    public interface IBuildQueue
    {
        public event StageUpdateDelegate StageUpdateEvent;
        public event BuildUpdateDelegate BuildUpdateEvent;

        public IReadOnlyCollection<Build> QueuedBuilds { get; }
        public IReadOnlyCollection<BuildRunner> Agents { get; }
    }
}