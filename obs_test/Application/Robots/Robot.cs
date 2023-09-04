using Ardalis.GuardClauses;
using obs_test.Application.Extensions;
using obs_test.Application.Interfaces;
using obs_test.Domain.Entities;
using obs_test.Domain.Enums;
using obs_test.Domain.Models;
using obs_test.Domain.Resources;
using obs_test.Domain.Validators;

namespace obs_test.Application.Robots;

public class Robot : IRobot
{
    private readonly List<List<RobotCommand>> _backOffStrategies;

    public Robot(InputData inputData)
    {
        Guard.Against.Null(inputData, nameof(inputData));
        InputData = inputData;
        RemainingBattery = inputData.Battery;
        CurrentPosition = inputData.InitialPosition;
        VisitedCells = new List<Location>();
        SamplesCollected = new List<string>();
        _backOffStrategies = new List<List<RobotCommand>>
        {
            new() { RobotCommand.E, RobotCommand.R, RobotCommand.F },
            new() { RobotCommand.E, RobotCommand.L, RobotCommand.F },
            new() { RobotCommand.E, RobotCommand.L, RobotCommand.L, RobotCommand.F },
            new() { RobotCommand.E, RobotCommand.B, RobotCommand.R, RobotCommand.F },
            new() { RobotCommand.E, RobotCommand.B, RobotCommand.B, RobotCommand.L, RobotCommand.F },
            new() { RobotCommand.E, RobotCommand.F, RobotCommand.F },
            new() { RobotCommand.E, RobotCommand.F, RobotCommand.L, RobotCommand.F, RobotCommand.L, RobotCommand.F }
        };
    }

    private InputData InputData { get; }
    private Position CurrentPosition { get; set; }
    private int RemainingBattery { get; set; }
    private List<Location> VisitedCells { get; }
    private List<string> SamplesCollected { get; }

    public void ExecuteCommand(string commandText)
    {
        var isValid = CommandParser.TryParse(commandText, out var command);
        if (!isValid) return;
        ExecuteCommand(command);
    }

    public SimulationResult RunSimulation()
    {
        VisitedCells.Add(CurrentPosition.Location);
        foreach (var command in InputData.Commands)
        {
            ExecuteCommand(command);
            if (RemainingBattery <= 0)
                break;
        }

        return new SimulationResult
        {
            VisitedCells = VisitedCells,
            SamplesCollected = SamplesCollected,
            Battery = RemainingBattery,
            FinalPosition = CurrentPosition
        };
    }

    private void ExecuteCommand(RobotCommand command)
    {
        switch (command)
        {
            case RobotCommand.F:
                if (!MoveForward()) ApplyBackOffStrategies();
                break;
            case RobotCommand.B:
                if (!MoveBackwards()) ApplyBackOffStrategies();
                break;
            case RobotCommand.L:
                if (!TurnLeft()) ApplyBackOffStrategies();
                break;
            case RobotCommand.R:
                if (!TurnRight()) ApplyBackOffStrategies();
                break;
            case RobotCommand.S:
                if (!TakeSample()) ApplyBackOffStrategies();
                break;
            case RobotCommand.E:
                ExtendPanels();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(RobotCommand), command, Resources.Message_InvalidCommand);
        }
    }

    private void ChargeBattery(int amount)
    {
        RemainingBattery += amount;
    }

    private bool TryConsumeBattery(int amount)
    {
        if (RemainingBattery < amount) return false; // Insufficient battery for movement
        // Deduct battery units for the movement
        RemainingBattery -= amount;
        RemainingBattery = Math.Max(RemainingBattery, 0);
        return true;
    }

    private void ExtendPanels()
    {
        ChargeBattery(10);
        TryConsumeBattery(1);
    }

    private bool MoveForward()
    {
        var newPosition = CurrentPosition.CalculateNewPosition(CurrentPosition.Facing);
        if (!newPosition.IsValidLocation(InputData.Terrain)) return false; // Position is outside the bounds of the terrain or Obstacle encountered (out of bounds)
        if (!TryConsumeBattery(3)) return false; // Insufficient battery for movement
        CurrentPosition = newPosition;
        VisitedCells.Add(CurrentPosition.Location);
        return true; // Movement successful
    }

    private bool MoveBackwards()
    {
        var newPosition = CurrentPosition.CalculateNewPosition(CurrentPosition.Facing.GetOppositeDirection());
        if (!newPosition.IsValidLocation(InputData.Terrain)) return false; // Position is outside the bounds of the terrain or Obstacle encountered (out of bounds)
        if (!TryConsumeBattery(3)) return false; // Insufficient battery for movement
        CurrentPosition = newPosition;
        VisitedCells.Add(CurrentPosition.Location);
        return true; // Movement successful
    }

    private bool TurnLeft()
    {
        if (!TryConsumeBattery(2)) return false; // Insufficient battery for movement
        CurrentPosition.Facing = CurrentPosition.Facing.GetLeftTurnDirection();
        return true; // Movement successful
    }

    private bool TurnRight()
    {
        if (!TryConsumeBattery(2)) return false; // Insufficient battery for movement
        CurrentPosition.Facing = CurrentPosition.Facing.GetRightTurnDirection();
        return true; // Movement successful
    }

    private bool TakeSample()
    {
        var sample = InputData.Terrain[CurrentPosition.Location.Y][CurrentPosition.Location.X];
        if (!TryConsumeBattery(8)) return false; // Insufficient battery for movement
        SamplesCollected.Add(sample);
        return true; // Movement successful
    }

    private void ApplyBackOffStrategies()
    {
        foreach (var strategy in _backOffStrategies)
        {
            foreach (var command in strategy) ExecuteCommand(command);
            return;
        }
    }
}