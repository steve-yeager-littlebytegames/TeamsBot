using System.Threading.Tasks;

namespace BuildSystem.Api
{
    public delegate Task BuildCreatedDelegate(Build build);

    public interface IBuildCreator
    {
        public event BuildCreatedDelegate BuildCreatedEvent;

        Task<Build> CreateBuildAsync(string definitionName);
    }
}