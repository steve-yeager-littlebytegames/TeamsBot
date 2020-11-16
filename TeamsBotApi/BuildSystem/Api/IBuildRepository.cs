using System.Threading.Tasks;

namespace BuildSystem.Api
{
    public interface IBuildRepository
    {
        public Task SaveAsync(BuildMetadata entity);
        public Task<BuildMetadata> LoadAsync(string definitionName);
    }
}