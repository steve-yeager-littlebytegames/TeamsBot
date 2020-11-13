using System.Collections.Generic;

namespace BuildSystem
{
    public readonly struct BuildDefinition
    {
        public IReadOnlyCollection<string> StageNames { get; }

        public BuildDefinition(params string[] stageNames)
        {
            StageNames = stageNames;
        }
    }
}