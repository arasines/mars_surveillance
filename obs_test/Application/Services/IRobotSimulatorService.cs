using obs_test.Application.Interfaces;
using obs_test.Domain.Models;

namespace obs_test.Application.Services;

public interface IRobotSimulatorService
{
    SimulationResult SimulateRobot(IRobot robot);
}