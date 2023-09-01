using Microsoft.Extensions.DependencyInjection;
using obs_test;
using obs_test.Application.Extensions;
using obs_test.Application.Interfaces;
using obs_test.Application.Robots;
using obs_test.Application.Services;
using obs_test.Domain.Models;
using obs_test.Domain.Resources;
using obs_test.Domain.Validators;

namespace obs_test_console;

internal class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddServices();
        var serviceProvider = services.BuildServiceProvider();
        ProcessJsonFile(args, serviceProvider);
    }

    private static void ProcessJsonFile(IReadOnlyList<string> args, IServiceProvider serviceProvider)
    {
        // If two arguments are provided, treat as file paths
        var inputFilePath = args[0];
        var outputFilePath = args[1];
        try
        {
            var inputData = JsonFileUtils.ReadJsonObject(inputFilePath);
            if (inputData == null)
            {
                Console.WriteLine(Resources.Error_DeserializeJSON);
                return;
            }

            var validator = new InputDataValidator();
            var validationResult = validator.Validate(inputData);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors) Console.WriteLine(Resources.Message_Validation_Error, error.ErrorMessage);
                return;
            }

            // Run simulation
            IRobot robot = new Robot(inputData);
            var simulatorService = serviceProvider.GetRequiredService<IRobotSimulatorService>();
            var result = simulatorService.SimulateRobot(robot);
            JsonFileUtils.PrettyWrite(result, outputFilePath);
            Console.WriteLine(Resources.Message_Simulation_Completed, outputFilePath);
            Console.WriteLine(Resources.Message_InitialPosition, inputData.InitialPosition.Location.X, inputData.InitialPosition.Location.Y, inputData.InitialPosition.Facing);
            Console.WriteLine(Resources.Message_FinalPosition, result.FinalPosition.Location.X, result.FinalPosition.Location.Y, result.FinalPosition.Facing);
            Console.WriteLine(Resources.Message_Commands, string.Join(", ", inputData.Commands));
            Console.WriteLine(Resources.Message_RemainingBattery, result.Battery);
            Console.WriteLine(Resources.Message_SamplesCollected, string.Join(", ", result.SamplesCollected));
            GenerateColoredMap(inputData, result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(Resources.Message_GenericError + ex.Message);
        }
    }

    private static void GenerateColoredMap(InputData inputData, SimulationResult result)
    {
        const int length = 3;
        Console.WriteLine(Resources.Terrain);
        foreach (var row in inputData.Terrain)
        {
            foreach (var cell in row)
            {
                Console.ResetColor();
                Console.ForegroundColor = cell switch
                {
                    "Obs" => ConsoleColor.Red,
                    _ => Console.ForegroundColor
                };
                Console.Write($@"[ {cell.PadRight(length)[..length]} ]");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.Write(Resources.Terrain_Route);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(Resources.Mark_VisitedCell_Short);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(Resources.Mark_FinalPosition_Short);
        Console.ResetColor();
        Console.Write(")");
        Console.WriteLine();
        Console.ResetColor();
        foreach (var row in inputData.Terrain)
        {
            foreach (var cell in row)
                if (result.FinalPosition.Location.X == Array.IndexOf(row, cell) && result.FinalPosition.Location.Y == Array.IndexOf(inputData.Terrain, row))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Resources.Mark_FinalPositon);
                }
                else if (result.VisitedCells.Any(p => inputData.Terrain[p.Y][p.X] == cell))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(Resources.Mark_VisitedCell);
                }
                else
                {
                    Console.ResetColor();
                    Console.ForegroundColor = cell switch
                    {
                        "Fe" => ConsoleColor.Gray,
                        "Se" => ConsoleColor.Magenta,
                        "W" => ConsoleColor.Cyan,
                        "Si" => ConsoleColor.White,
                        "Zn" => ConsoleColor.Blue,
                        "Obs" => ConsoleColor.Red,
                        _ => Console.ForegroundColor
                    };
                    Console.Write($@"[ {cell.PadRight(length)[..length]} ]");
                }

            Console.WriteLine();
            Console.ResetColor();
        }
    }
}