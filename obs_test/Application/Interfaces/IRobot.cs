using obs_test.Domain.Models;

namespace obs_test.Application.Interfaces;

public interface IRobot
{
    void ExecuteCommand(string commandText);
    public SimulationResult RunSimulation();
}