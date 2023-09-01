using obs_test.Domain.Enums;

namespace obs_test.Domain.Validators;

public static class CommandParser
{
    public static bool TryParse(string commandText, out RobotCommand command) => Enum.TryParse(commandText, out command);
}