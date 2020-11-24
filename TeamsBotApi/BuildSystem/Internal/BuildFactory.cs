using System.Linq;
using System.Threading.Tasks;
using BuildSystem.Api;
using BuildSystem.Data;

namespace BuildSystem
{
    internal class BuildFactory : IBuildCreator
    {
        private readonly IBuildRepository buildRepository;

        public BuildFactory(IBuildRepository buildRepository)
        {
            this.buildRepository = buildRepository;
        }

        public event BuildCreatedDelegate BuildCreatedEvent;

        public async Task<Build> CreateBuildAsync(string definitionName)
        {
            var buildDefinition = BuildDefinitions.Definitions.First(bd => bd.Name.ToLower() == definitionName.ToLower());

            var metaData = await buildRepository.LoadAsync(definitionName);
            ++metaData.BuildCount;

            var build = new Build(definitionName, metaData.BuildCount, buildDefinition.StageNames.Select(sn => new Stage(sn)).ToArray());
            foreach(var stage in build.Stages)
            {
                stage.Build = build;
            }

            await buildRepository.SaveAsync(metaData);

            BuildCreatedEvent?.Invoke(build);

            return build;
        }
    }
}