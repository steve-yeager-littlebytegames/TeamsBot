using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using BuildSystem.Api;
using CommandLine;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using TeamsBotApi.Services;
using TeamsBotApi.UiHelpers;

namespace TeamsBotApi.BotCommands
{
    [Verb("agents", HelpText = "Show agent statuses.")]
    public class ShowAgentsCommand : BotCommand
    {
        protected override (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext)
        {
            throw new NotImplementedException();
        }

        protected override async Task ExecuteInternalAsync(BuildFacade buildFacade, NotificationService notificationService, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var card = notificationService.CreateCard(
                new AdaptiveTextBlock($"{buildFacade.Agents.Count(a => !a.IsIdle)}/{buildFacade.Agents.Count} agents are executing."),
                new ListBuilder(buildFacade.Agents.Select(a => $"{a.Name} is {(a.IsIdle ? "idle" : $"executing {a.ActiveBuild.Name}")}")));

            await notificationService.SendCardAsync(card, turnContext);
        }
    }
}