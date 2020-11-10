using System.Collections.Generic;

namespace BuildSystem
{
    public readonly struct BuildDefinition
    {
        public string Name { get; }
        public IReadOnlyCollection<string> StageNames { get; }

        public BuildDefinition(string name, IReadOnlyCollection<string> stageNames)
        {
            Name = name;
            StageNames = stageNames;
        }
    }
}