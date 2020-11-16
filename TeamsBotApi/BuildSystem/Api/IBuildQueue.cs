using System.Collections.Generic;

namespace BuildSystem.Api
{
    public interface IBuildQueue
    {
        public IReadOnlyCollection<Build> QueuedBuilds { get; }
    }
}