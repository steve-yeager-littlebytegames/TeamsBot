namespace BuildSystem
{
    public class BuildPipeline
    {
        public string Name { get; }
        public BuildMetadata Metadata { get; }
        public BuildDefinition Definition { get; }

        public BuildPipeline(string name, BuildMetadata metadata, BuildDefinition definition)
        {
            Name = name;
            Metadata = metadata;
            Definition = definition;
        }
    }
}