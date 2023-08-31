using obs_test.Domain.Models;

namespace obs_test.Application
{
    public interface IRobot
    {
        void ExecuteCommand(string command);
        public SimulationResult RunSimulation();
    }
}
