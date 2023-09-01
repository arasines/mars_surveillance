using obs_test.Domain.Entities;

namespace obs_test.Domain.Models;

public class InputData
{
    public required string[][] Terrain { get; set; }
    public int Battery { get; set; }
    public required string[] Commands { get; set; }
    public required Position InitialPosition { get; set; }
}