using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using BuildSystem.Api;
using CommandLine;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using TeamsBotApi.Extensions;
using TeamsBotApi.Services;
using TeamsBotApi.UiHelpers;

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
            var card = notificationService.CreateCard(new List<AdaptiveElement>
            {
                new AdaptiveTextBlock($"Builds in queue: {buildFacade.QueuedBuilds.Count}"),
                new ListBuilder(buildFacade.QueuedBuilds.Select(b => $"[{(DateTime.Now - b.QueueTime).ToDuration()}] {b}"))
            });

            await notificationService.SendCardAsync(card, turnContext);
        }
    }
}