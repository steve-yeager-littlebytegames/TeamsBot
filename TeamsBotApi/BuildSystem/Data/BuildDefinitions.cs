using System.Collections.Generic;

namespace BuildSystem.Data
{
    public static class BuildDefinitions
    {
        public static readonly IReadOnlyCollection<BuildDefinition> Definitions = new[]
        {
            new BuildDefinition("Client", "Pull", "Compile", "Assets", "Test", "Upload"),
            new BuildDefinition("Server", "Pull", "Compile", "Test", "Deploy", "Acceptance Tests"),
        };
    }
}