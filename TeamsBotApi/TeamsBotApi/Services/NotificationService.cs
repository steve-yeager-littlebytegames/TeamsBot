using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
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
using TeamsBotApi.Extensions;

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
            buildFacade.BuildCompleteEvent += OnBuildComplete;
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
            var notifications = await notificationDb.NotificationDetails
                .Where(nd => nd.BuildId == build.Id && nd.WatchLevel >= WatchLevel.Stage)
                .ToArrayAsync();

            foreach(var notification in notifications)
            {
                await SendMessageAsync($"Stage {build} {stage} finished with {stage.Status} in {stage.Duration.ToDuration()}", notification);
            }
        }

        private async Task OnBuildComplete(Build build)
        {
            var notifications = await notificationDb.NotificationDetails
                .Where(nd => nd.BuildId == build.Id && nd.WatchLevel >= WatchLevel.Build)
                .ToArrayAsync();

            foreach(var notification in notifications)
            {
                await SendMessageAsync($"Build {build} finished with {build.Status} in {build.BuildDuration.ToDuration()}", notification);
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
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
        }

        public AdaptiveCard CreateCard(params AdaptiveElement[] body)
        {
            return new AdaptiveCard("1.0")
            {
                Type = "AdaptiveCard",
                Body = body.ToList(),
            };
        }

        public async Task SendCardAsync(AdaptiveCard card, ITurnContext<IMessageActivity> turnContext)
        {
            card.Version = "1.0";
            var attachment = MessageFactory.Attachment(new Attachment("application/vnd.microsoft.card.adaptive", content: card));
            await turnContext.SendActivityAsync(attachment);
        }
    }
}