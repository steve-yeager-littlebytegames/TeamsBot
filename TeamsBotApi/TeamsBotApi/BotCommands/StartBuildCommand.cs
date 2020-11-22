using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using CommandLine;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using TeamsBotApi.Data;
using TeamsBotApi.Services;

namespace TeamsBotApi.BotCommands
{
    [Verb("build", HelpText = "Start new builds.")]
    public class StartBuildCommand : BotCommand
    {
        private string conversationId;

        [Option('w', "watch")]
        public bool ShouldWatch { get; set; }

        protected override (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext)
        {
            var isCorrectSize = split.Length == 2;

            if(isCorrectSize)
            {
                return (true, string.Empty);
            }

            return (false, "Don't know what build to start.");
        }

        protected override async Task ExecuteInternalAsync(BuildFacade buildFacade, NotificationService notificationService, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            conversationId = turnContext.Activity.Conversation.Id;

            var buildName = split[1];

            var build = await buildFacade.CreateBuildAsync(buildName);

            await notificationService.AddNotificationAsync(build.Id, conversationId, ShouldWatch ? WatchLevel.Stage : WatchLevel.Build);

            await notificationService.SendReplyAsync($"Created build {build}", turnContext, cancellationToken);
        }
    }
}