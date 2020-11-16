using System.Threading.Tasks;

namespace BuildSystem.Api
{
    public interface IBuildCreator
    {
        Task<Build> CreateBuildAsync(string definitionName);
    }
}