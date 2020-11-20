using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using TeamsBotApi.Services;

namespace TeamsBotApi.BotCommands
{
    public abstract class BotCommand
    {
        public string CommandString { get; }

        protected BotCommand(string commandString)
        {
            CommandString = $"/{commandString}";
        }

        protected abstract (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext);

        protected abstract Task ExecuteInternalAsync(BuildFacade buildFacade, NotificationService notificationService, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken);

        public async Task ExecuteAsync(BuildFacade buildFacade, NotificationService notificationService, string text, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var split = text.Split();
            //var (isValid, errorMessage) = Validate(text, split, turnContext);
            await ExecuteInternalAsync(buildFacade, notificationService, text, split, turnContext, cancellationToken);

            //if(isValid)
            //{
            //    await ExecuteInternalAsync(buildFacade, text, split, turnContext, cancellationToken);
            //}
            //else
            //{
            //    await SendMessageAsync(errorMessage, turnContext, cancellationToken);
            //}
        }
    }
}