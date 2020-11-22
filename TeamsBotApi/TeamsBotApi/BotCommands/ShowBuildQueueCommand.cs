using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using AdaptiveCards.Rendering.Html;
using BuildSystem.Api;
using CommandLine;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using TeamsBotApi.Services;

namespace TeamsBotApi.BotCommands
{
    [Verb("queue", HelpText = "Show details about the build queue.")]
    public class ShowBuildQueueCommand : BotCommand
    {
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

            await SendCardAsync(turnContext);
            await notificationService.SendReplyAsync(message.ToString(), turnContext, cancellationToken);
        }

        private async Task SendCardAsync(ITurnContext<IMessageActivity> turnContext)
        {
            var renderer = new AdaptiveCardRenderer();
            //var card = new AdaptiveCard(renderer.SupportedSchemaVersion)
            var card = new AdaptiveCard("1.0")
            {
                Id = "test",
                Type = "AdaptiveCard",
                Speak = "blah",
                Body =
                {
                    new AdaptiveTextBlock("hello"),
                    new AdaptiveRichTextBlock
                    {
                        Inlines =
                        {
                            new AdaptiveTextRun("1. Green\r2. Orange\r3. Blue")
                        }
                    },
                }
            };

            var json = JsonConvert.SerializeObject(card);

            var attachment = MessageFactory.Attachment(new Attachment("application/vnd.microsoft.card.adaptive", content: card));

            await turnContext.SendActivityAsync(attachment);
        }
    }
}