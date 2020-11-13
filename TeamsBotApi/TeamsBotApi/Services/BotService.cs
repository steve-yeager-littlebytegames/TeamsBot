using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;

namespace TeamsBotApi.Services
{
    public class BotService : TeamsActivityHandler
    {
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

            var replyText = "That command is not supported";

            switch(split[0])
            {
                case "/randomnumber":
                    var min = int.Parse(split[1]);
                    var max = int.Parse(split[2]);

                    var random = new Random();
                    var value = random.Next(min, max);
                    replyText = $"You rolled a '{value}'";
                    break;
            }

            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }
    }
}