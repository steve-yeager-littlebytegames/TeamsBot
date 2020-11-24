using System.Linq;
using System.Threading.Tasks;
using BuildSystem;
using BuildSystem.Api;
using Microsoft.Extensions.Logging;
using TeamsBotApi.Data;

namespace TeamsBotApi.Services
{
    public class MetricsService
    {
        private readonly BuildDbContext buildDb;
        private readonly ILogger<MetricsService> logger;

        public MetricsService(BuildDbContext buildDb, BuildFacade buildFacade, ILogger<MetricsService> logger)
        {
            this.buildDb = buildDb;
            this.logger = logger;

            buildFacade.BuildCreatedEvent += OnBuildCreated;
            buildFacade.StageUpdateEvent += OnStageUpdate;
            buildFacade.BuildUpdateEvent += OnBuildUpdate;
        }

        private async Task OnBuildCreated(Build build)
        {
            logger.LogInformation($"{build} → {build.Status}");
            buildDb.Builds.Add(build);
            buildDb.Stages.AddRange(build.Stages.Cast<StageEntity>());
            await buildDb.SaveChangesAsync();
        }

        private async Task OnStageUpdate(Stage stage)
        {
            logger.LogInformation($"{stage} → {stage.Status}");
            buildDb.Stages.Update(stage);
            await buildDb.SaveChangesAsync();
        }

        private async Task OnBuildUpdate(Build build)
        {
            logger.LogInformation($"{build} → {build.Status}");
            buildDb.Builds.Update(build);
            await buildDb.SaveChangesAsync();
        }
    }
}