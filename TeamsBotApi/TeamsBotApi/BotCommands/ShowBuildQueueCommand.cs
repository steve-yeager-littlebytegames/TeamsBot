﻿using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using CommandLine;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace TeamsBotApi.BotCommands
{
    [Verb("/queue")]
    public class ShowBuildQueueCommand : BotCommand
    {
        public ShowBuildQueueCommand()
            : base("queue")
        {
        }

        protected override (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext)
        {
            return (true, string.Empty);
        }

        protected override async Task ExecuteInternalAsync(BuildFacade buildFacade, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var message = new StringBuilder();
            message.AppendLine($"Builds in queue: {buildFacade.QueuedBuilds.Count}");
            foreach(var build in buildFacade.QueuedBuilds)
            {
                message.AppendLine($"# {build}");
            }

            await SendMessageAsync(message.ToString(), turnContext, cancellationToken);
        }
    }
}