using System.Collections.Generic;
using System.Linq;

namespace BuildSystem
{
    public class BuildFactory
    {
        private readonly IReadOnlyCollection<BuildPipeline> pipelines = new[]
        {
            new BuildPipeline("Client", new BuildMetadata(), new BuildDefinition("Pull", "Compile", "Assets", "Test", "Upload")),
            new BuildPipeline("Server", new BuildMetadata(), new BuildDefinition("Pull", "Compile", "Test", "Deploy", "Acceptance Tests")),
        };

        public Build CreateBuild(string buildName)
        {
            var pipeline = pipelines.First(bd => bd.Name.ToLower() == buildName.ToLower());
            ++pipeline.Metadata.BuildCount;
            return new Build(buildName, pipeline.Metadata.BuildCount, pipeline.Definition.StageNames.Select(sn => new Stage(sn)).ToArray());
        }
    }
}