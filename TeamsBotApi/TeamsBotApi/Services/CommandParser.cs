using CommandLine;
using TeamsBotApi.BotCommands;

namespace TeamsBotApi.Services
{
    public class CommandParser
    {
        public BotCommand Parse(string text)
        {
            var textSplit = text.Split();

            var parser = new Parser(settings =>
            {
                settings.AutoHelp = false;
                settings.AutoVersion = true;
            });
            var command = parser.ParseArguments<StartBuildCommand, ShowBuildQueueCommand, HelpCommand, ShowAgentsCommand>(textSplit)
                .MapResult((BotCommand c) => c, errors => null);
            return command;
        }
    }
}