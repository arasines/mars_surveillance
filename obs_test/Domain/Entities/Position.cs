using obs_test.Domain.Enums;

namespace obs_test.Domain.Entities;

public class Position
{
    public Position(Location location)
    {
        Location = location;
    }
    public Location Location { get; set; }

    public Direction Facing { get; set; }
}
