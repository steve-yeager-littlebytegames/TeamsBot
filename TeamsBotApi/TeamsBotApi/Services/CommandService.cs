using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace TeamsBotApi.Services
{
    public class CommandService
    {
        private readonly CommandParser commandParser;
        private readonly NotificationService notificationService;
        private readonly BuildFacade buildFacade;
        private readonly ILogger<CommandService> logger;

        public CommandService(CommandParser commandParser, NotificationService notificationService, BuildFacade buildFacade, ILogger<CommandService> logger)
        {
            this.commandParser = commandParser;
            this.notificationService = notificationService;
            this.buildFacade = buildFacade;
            this.logger = logger;
        }

        public async Task ProcessCommandAsync(string message, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            logger.LogInformation("Processing Command.");
            var command = commandParser.Parse(message);
            await command.ExecuteAsync(buildFacade, notificationService, message, turnContext, cancellationToken);
        }
    }
}