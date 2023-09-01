using obs_test.Domain.Entities;

namespace obs_test.Domain.Models;

public class SimulationResult
{
    public SimulationResult()
    {
        VisitedCells = new List<Location>();
        SamplesCollected = new List<string>();
        FinalPosition = new Position(new Location());
        Battery = 0;
    }

    public List<Location> VisitedCells { get; set; }
    public List<string> SamplesCollected { get; set; }
    public int Battery { get; set; }
    public Position FinalPosition { get; set; }
}