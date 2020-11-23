﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BuildSystem.Api;
using CommandLine;
using CommandLine.Text;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using TeamsBotApi.Services;

namespace TeamsBotApi.BotCommands
{
    [Verb("info")]
    public class HelpCommand : BotCommand
    {
        protected override (bool isValid, string errorMessage) Validate(string text, string[] split, ITurnContext<IMessageActivity> turnContext)
        {
            throw new NotImplementedException();
        }

        protected override async Task ExecuteInternalAsync(BuildFacade buildFacade, NotificationService notificationService, string text, string[] split, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // TODO: Don't duplicate.
            var command = Parser.Default.ParseArguments<StartBuildCommand, ShowBuildQueueCommand, HelpCommand, ShowAgentsCommand>(new List<string>{"help"});
            var helpText = HelpText.AutoBuild(command, h =>
                {
                    h.Heading = "Infrax bot command help.";
                    h.Copyright = string.Empty;
                    h.AutoHelp = false;
                    return h;
                },
                e => e,
                true);
            var helpString = helpText.ToString();

            await notificationService.SendReplyAsync(helpString, turnContext, cancellationToken);
        }
    }
}