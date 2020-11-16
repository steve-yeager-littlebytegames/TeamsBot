using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;

namespace TeamsBotApi.Services
{
    public class BotHandler : TeamsActivityHandler
    {
        private readonly CommandService commandService;

        public BotHandler(CommandService commandService)
        {
            this.commandService = commandService;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            turnContext.Activity.RemoveRecipientMention();
            var text = turnContext.Activity.Text.Trim().ToLower();

            await commandService.ProcessCommandAsync(text, turnContext, cancellationToken);
        }
    }
}