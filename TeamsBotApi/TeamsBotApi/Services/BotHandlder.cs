using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using TeamsBotApi.BotCommands;

namespace TeamsBotApi.Services
{
    public class BotHandlder : TeamsActivityHandler
    {
        private readonly BuildFacade buildFacade;
        private readonly IReadOnlyCollection<BotCommand> botCommands = new BotCommand[] {new StartBuildCommand(), new ShowBuildQueueCommand(),};

        public BotHandlder(BuildFacade buildFacade)
        {
            this.buildFacade = buildFacade;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            turnContext.Activity.RemoveRecipientMention();
            var text = turnContext.Activity.Text.Trim().ToLower();

            if(text.StartsWith("/"))
            {
                await ProcessCommandAsync(text, turnContext, cancellationToken);
            }
            else
            {
                var replyText = $"Did you say: {text}?";
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            }
        }

        private async Task ProcessCommandAsync(string text, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var split = text.Split();
            var botCommand = botCommands.FirstOrDefault(bc => split[0] == bc.CommandString.ToLower());

            if(botCommand == null)
            {
                const string replyText = "That command is not supported";
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            }
            else
            {
                await botCommand.RunAsync(buildFacade, text, split, turnContext, cancellationToken);
            }
        }
    }
}