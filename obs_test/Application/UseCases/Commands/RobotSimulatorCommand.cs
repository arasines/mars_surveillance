using Ardalis.GuardClauses;
using MediatR;
using obs_test.Application.Interfaces;
using obs_test.Application.Robots;
using obs_test.Application.Services;
using obs_test.Domain.Models;

namespace obs_test.Application.UseCases.Commands;

public class RobotSimulatorCommand : IRequest<SimulationResult>
{
    public RobotSimulatorCommand(InputData parameters)
    {
        Guard.Against.Null(parameters, nameof(parameters));
        Parameters = parameters;
    }

    /// <summary>
    /// </summary>
    public InputData Parameters { get; set; }
}

public class RobotSimulatorCommandHandler : IRequestHandler<RobotSimulatorCommand, SimulationResult>
{
    private readonly IRobotSimulatorService _simulatorService;

    public RobotSimulatorCommandHandler(IRobotSimulatorService simulatorService)
    {
        Guard.Against.Null(simulatorService, nameof(simulatorService));
        _simulatorService = simulatorService;
    }


    public Task<SimulationResult> Handle(RobotSimulatorCommand request, CancellationToken cancellationToken)
    {
        // Run simulation
        IRobot robot = new Robot(request.Parameters);
        var result = _simulatorService.SimulateRobot(robot);
        return Task.FromResult(result);
    }
}