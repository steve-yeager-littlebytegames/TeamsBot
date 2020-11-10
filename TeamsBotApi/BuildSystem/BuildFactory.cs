using System.Collections.Generic;
using System.Linq;

namespace BuildSystem
{
    public class BuildFactory
    {
        private readonly IReadOnlyCollection<BuildDefinition> buildDefinitions = new[]
        {
            new BuildDefinition("Client", new[] {"Pull", "Compile", "Test", "Deploy"}),
        };

        public Build CreateBuild(string definitionName)
        {
            var definition = buildDefinitions.First(bd => bd.Name == definitionName);
            return new Build(definitionName, 0, definition.StageNames.Select(sn => new Stage(sn)).ToArray());
        }
    }
}