using obs_test.Domain.Entities;
using obs_test.Domain.Enums;
using obs_test.Domain.Resources;

namespace obs_test.Application.Extensions;

public static class RobotExtensions
{
    public static Position CalculateNewPosition(this Position position, Direction facing)
    {
        var newLocation = new Location { X = position.Location.X, Y = position.Location.Y };
        switch (position.Facing)
        {
            case Direction.North:
                newLocation.Y--;
                break;
            case Direction.East:
                newLocation.X++;
                break;
            case Direction.South:
                newLocation.Y++;
                break;
            case Direction.West:
                newLocation.X--;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Direction), position.Facing, Resources.Message_InvalidDirection);
        }

        return new Position(newLocation)
        {
            Facing = facing
        };
    }

    public static bool IsValidLocation(this Position position, string[][] terrain)
    {
        var numRows = terrain.Length;
        var numCols = terrain[0].Length;

        // Check if the next position is within the bounds of the terrain or if the next cell is an obstacle
        return position.Location.X >= 0 && position.Location.X < numCols &&
               position.Location.Y >= 0 && position.Location.Y < numRows &&
               terrain[position.Location.Y][position.Location.X] != "Obs";
    }

    public static Direction GetOppositeDirection(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.East => Direction.West,
            Direction.South => Direction.North,
            Direction.West => Direction.East,
            _ => Direction.North
        };
    }

    public static Direction GetLeftTurnDirection(this Direction direction)
    {
        return (Direction)(((int)direction + 3) % 4);
    }

    public static Direction GetRightTurnDirection(this Direction direction)
    {
        return (Direction)(((int)direction + 1) % 4);
    }
}