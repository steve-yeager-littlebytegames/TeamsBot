using CommandLine;
using TeamsBotApi.BotCommands;

namespace TeamsBotApi.Services
{
    public class CommandParser
    {
        public BotCommand Parse(string text)
        {
            var textSplit = text.Split();
            var command = Parser.Default.ParseArguments<StartBuildCommand, ShowBuildQueueCommand, HelpCommand, ShowAgentsCommand>(textSplit)
                .MapResult((BotCommand c) => c, errors => null);
            return command;
        }
    }
}