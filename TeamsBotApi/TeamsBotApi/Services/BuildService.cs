using BuildSystem.Api;

namespace TeamsBotApi.Services
{
    public class BuildService
    {
        private readonly NotificationService notificationService;
        private readonly BuildFacade buildFacade;

        public BuildService(NotificationService notificationService, BuildFacade buildFacade)
        {
            this.notificationService = notificationService;
            this.buildFacade = buildFacade;
        }
    }
}