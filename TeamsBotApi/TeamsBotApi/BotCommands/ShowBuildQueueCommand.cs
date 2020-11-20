using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using CommandLine;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using TeamsBotApi.Services;

namespace TeamsBotApi.BotCommands
{
    [Verb("/queue")]
    public class ShowBuildQueueCommand : BotCommand
    {
        public ShowBuildQueueCommand()
            : base("queue")
        {
        }

        protected override (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext)
        {
            return (true, string.Empty);
        }

        protected override async Task ExecuteInternalAsync(BuildFacade buildFacade, NotificationService notificationService, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var message = new StringBuilder();
            message.AppendLine($"Builds in queue: {buildFacade.QueuedBuilds.Count}");
            foreach(var build in buildFacade.QueuedBuilds)
            {
                var queueTime = DateTime.Now - build.QueueTime;
                message.AppendLine($"[{queueTime:g}] {build}");
            }

            await notificationService.SendReplyAsync(message.ToString(), turnContext, cancellationToken);
        }
    }
}