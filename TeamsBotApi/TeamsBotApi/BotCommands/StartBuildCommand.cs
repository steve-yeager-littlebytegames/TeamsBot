using System.Threading;
using System.Threading.Tasks;
using BuildSystem;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace TeamsBotApi.BotCommands
{
    public class StartBuildCommand : BotCommand
    {
        public StartBuildCommand()
            : base("build")
        {
        }

        protected override (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext)
        {
            var isCorrectSize = split.Length == 2;

            if(isCorrectSize)
            {
                return (true, string.Empty);
            }

            return (false, "Don't know what build to start.");
        }

        protected override async Task ExecuteAsync(BuildFactory buildFactory, BuildMonitor buildMonitor, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var buildName = split[1];

            var build = buildFactory.CreateBuild(buildName);
            buildMonitor.AddBuild(build);
            build.BuildCompleteEvent += async b => await SendMessageAsync($"Build {build} finished with {build.Status}", turnContext, CancellationToken.None);

            await SendMessageAsync($"Created build {build}", turnContext, cancellationToken);
        }
    }
}