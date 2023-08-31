using obs_test.Domain.Enums;

namespace obs_test.Domain.Validators
{
    public static class CommandParser
    {
        private static readonly Dictionary<string, Command> CommandMappings = new()
        {
        { "F", Command.MoveForward },
        { "B", Command.MoveBackwards },
        { "L", Command.TurnLeft },
        { "R", Command.TurnRight },
        { "S", Command.TakeSample },
        { "E", Command.ExtendPanels },
        };

        public static bool TryParse(string commandText, out Command command) => CommandMappings.TryGetValue(commandText, out command);
    }
}
