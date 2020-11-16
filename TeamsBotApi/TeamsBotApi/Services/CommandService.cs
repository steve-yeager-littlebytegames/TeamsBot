using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace TeamsBotApi.Services
{
    public class CommandService
    {
        private readonly CommandParser commandParser;
        private readonly NotificationService notificationService;
        private readonly BuildFacade buildFacade;

        public CommandService(CommandParser commandParser, NotificationService notificationService, BuildFacade buildFacade)
        {
            this.commandParser = commandParser;
            this.notificationService = notificationService;
            this.buildFacade = buildFacade;
        }

        public async Task ProcessCommandAsync(string message, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var command = commandParser.Parse(message);
            await command.ExecuteAsync(buildFacade, notificationService, message, turnContext, cancellationToken);
        }
    }
}