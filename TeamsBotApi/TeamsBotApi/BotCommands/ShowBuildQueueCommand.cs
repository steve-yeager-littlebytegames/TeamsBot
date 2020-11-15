using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildSystem;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace TeamsBotApi.BotCommands
{
    public class ShowBuildQueueCommand: BotCommand
    {
        public ShowBuildQueueCommand()
            : base("queue")
        {
        }

        protected override (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext)
        {
            return (true, string.Empty);
        }

        protected override async Task ExecuteAsync(BuildFactory buildFactory, BuildMonitor buildMonitor, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var message = new StringBuilder();
            message.AppendLine($"Builds in queue: {buildMonitor.queuedBuilds.Count}");
            foreach(var build in buildMonitor.queuedBuilds)
            {
                message.AppendLine($"# {build}");
            }

            await SendMessageAsync(message.ToString(), turnContext, cancellationToken);
        }
    }
}