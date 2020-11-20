using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildSystem;
using BuildSystem.Api;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TeamsBotApi.Data;

namespace TeamsBotApi.Services
{
    public class NotificationService
    {
        private const string ServiceUrl = "https://smba.trafficmanager.net/amer/";
        private readonly NotificationDbContext notificationDb;
        private readonly ILogger<NotificationService> logger;
        private readonly string appId;
        private readonly string appPassword;

        public NotificationService(NotificationDbContext notificationDb, BuildFacade buildFacade, IConfiguration configuration, ILogger<NotificationService> logger)
        {
            this.notificationDb = notificationDb;
            this.logger = logger;
            appId = configuration.GetValue<string>("MicrosoftAppId");
            appPassword = configuration.GetValue<string>("MicrosoftAppPassword");

            buildFacade.StageCompleteEvent += OnStageComplete;
        }

        public async Task AddNotificationAsync(Guid buildId, string channelId, WatchLevel watchLevel)
        {
            var notification = new NotificationDetails(buildId, channelId, watchLevel);
            logger.LogInformation($"Adding notification: '{notification}'");

            notificationDb.Add(notification);
            await notificationDb.SaveChangesAsync();
        }

        private async Task OnStageComplete(Build build, Stage stage)
        {
            bool isBuildDone = build.Status == BuildStatus.Cancelled || build.Status == BuildStatus.Failed || build.Status == BuildStatus.Succeeded;

            var notifications = await notificationDb.NotificationDetails
                .Where(nd => nd.BuildId == build.Id)
                .ToArrayAsync();

            foreach(var notification in notifications)
            {
                if(notification.WatchLevel == WatchLevel.Build)
                {
                    if(isBuildDone)
                    {
                        await SendMessageAsync($"Build {build} finished with {build.Status} in {build.BuildDuration:g}", notification);
                    }
                }
                else
                {
                    if(isBuildDone)
                    {
                        await SendMessageAsync($"Build {build} finished with {build.Status} in {build.BuildDuration:g}", notification);
                    }
                    else
                    {
                        await SendMessageAsync($"Stage {build} {stage} finished with {stage.Status} in {stage.Duration:g}", notification);
                    }
                }
            }
        }

        private async Task SendMessageAsync(string message, NotificationDetails notification)
        {
            var activity = MessageFactory.Text(message);
            activity.Summary = message;
            activity.TeamsNotifyUser();

            AppCredentials.TrustServiceUrl(ServiceUrl);

            var credentials = new MicrosoftAppCredentials(appId, appPassword);

            var connectorClient = new ConnectorClient(new Uri(ServiceUrl), credentials);
            await connectorClient.Conversations.SendToConversationAsync(notification.ChannelId, activity);
        }

        public async Task SendReplyAsync(string replyText, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }
    }
}