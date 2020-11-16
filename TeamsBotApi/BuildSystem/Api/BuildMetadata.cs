namespace BuildSystem
{
    public class BuildMetadata
    {
        public string DefinitionName { get; set; }
        public int BuildCount { get; set; }

        public BuildMetadata(string definitionName)
        {
            DefinitionName = definitionName;
        }
    }
}