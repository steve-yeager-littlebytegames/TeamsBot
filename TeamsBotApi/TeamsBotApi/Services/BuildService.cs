using System;
using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

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

        public async Task ProcessCommandAsync(string message, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}