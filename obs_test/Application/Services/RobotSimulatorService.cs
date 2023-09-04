using obs_test.Application.Interfaces;
using obs_test.Domain.Models;

namespace obs_test.Application.Services;

public class RobotSimulatorService : IRobotSimulatorService
{
    public SimulationResult SimulateRobot(IRobot robot)
    {
        return robot.RunSimulation();
    }
}