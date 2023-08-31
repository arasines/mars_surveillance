using obs_test.Domain.Entities;
using obs_test.Domain.Enums;

namespace obs_test.Application.Extensions
{
    public static class RobotExtensions
    {

    //    private static readonly Dictionary<Direction, Location> MovementLookup = new()
    //    {
    //    { Direction.North, new Location { X = 0, Y = -1 } },
    //    { Direction.East, new Location { X = 1, Y = 0 } },
    //    { Direction.South, new Location { X = 0, Y = 1 } },
    //    { Direction.West, new Location { X = -1, Y = 0 } }
    //};

    //    private static readonly Dictionary<string, Direction> DirectionLookup = new()
    //    {
    //    { "North", Direction.North },
    //    { "East", Direction.East },
    //    { "South", Direction.South },
    //    { "West", Direction.West }
    //};
        public static Position CalculateNewPosition(this Position position)
        {
            var newPosition = new Location { X = position.Location.X, Y = position.Location.Y };
            switch (position.Facing)
            {
                case Direction.North:
                    newPosition.Y -= 1;
                    break;
                case Direction.East:
                    newPosition.X += 1;
                    break;
                case Direction.South:
                    newPosition.Y += 1;
                    break;
                case Direction.West:
                    newPosition.X -= 1;
                    break;
            }
            position.Location = newPosition;
            return position;
        }
        public static bool IsValidLocation(this Position position, string[][] terrain)
        {
            int numRows = terrain.Length;
            int numCols = terrain[0].Length;

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
                _ => Direction.North,
            };
        }

        public static Direction GetLeftTurnDirection(this Direction direction) => (Direction)(((int)direction + 3) % 4);

        public static Direction GetRightTurnDirection(this Direction direction) => (Direction)(((int)direction + 1) % 4);
    }
}
