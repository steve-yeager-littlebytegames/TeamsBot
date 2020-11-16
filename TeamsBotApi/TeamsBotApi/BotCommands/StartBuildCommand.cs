using System;
using System.Threading;
using System.Threading.Tasks;
using BuildSystem;
using BuildSystem.Api;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;

namespace TeamsBotApi.BotCommands
{
    public class StartBuildCommand : BotCommand
    {
        private string serviceUrl; // TODO: Hardcode.
        private string conversationId;

        public StartBuildCommand()
            : base("build")
        {
        }

        protected override (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext)
        {
            var isCorrectSize = split.Length == 2;

            if(isCorrectSize)
            {
                return (true, string.Empty);
            }

            return (false, "Don't know what build to start.");
        }

        protected override async Task ExecuteAsync(BuildFacade buildFacade, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            conversationId = turnContext.Activity.Conversation.Id;
            serviceUrl = turnContext.Activity.ServiceUrl;

            var buildName = split[1];

            var build = await buildFacade.CreateBuildAsync(buildName);
            build.BuildCompleteEvent += SendMessage;
            //build.BuildCompleteEvent += async b => await SendMessageAsync($"Build {build} finished with {build.Status} in {build.EndTime-build.StartTime:g}", turnContext, CancellationToken.None);

            await SendMessageAsync($"Created build {build}", turnContext, cancellationToken);
        }

        private async Task SendMessage(Build build)
        {
            var message = $"Build {build} finished with {build.Status} in {build.BuildDuration:g}";
            var activity = MessageFactory.Text(message);
            activity.Summary = message;
            activity.TeamsNotifyUser();

            AppCredentials.TrustServiceUrl(serviceUrl);

            var credentials = new MicrosoftAppCredentials("cba04884-c4c6-4dd7-b4e9-4e23c1f5e6e1", "Aaq7yD2Yrb.70kn4d57AY.j8B.NlW2_JKP");

            var connectorClient = new ConnectorClient(new Uri(serviceUrl), credentials);
            await connectorClient.Conversations.SendToConversationAsync(conversationId, activity);
        }
    }
}