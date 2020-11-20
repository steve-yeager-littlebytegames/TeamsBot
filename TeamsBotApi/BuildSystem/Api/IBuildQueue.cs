using System.Collections.Generic;

namespace BuildSystem.Api
{
    public interface IBuildQueue
    {
        public event StageCompleteDelegate StageCompleteEvent;

        public IReadOnlyCollection<Build> QueuedBuilds { get; }
    }
}