using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using obs_test.Application;
using obs_test.Application.Services;
using obs_test.Domain.Models;
using obs_test.Domain.Validators;

namespace obs_test;

internal class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddServices();
        var serviceProvider = services.BuildServiceProvider();

        switch (args.Length)
        {
            case 2:
                {
                    ProcessJsonFile(args, serviceProvider);
                    break;
                }
            case 0:
                // If no arguments are provided, simulate REST API behavior
                Console.WriteLine("Running REST API...");
                // Add your REST API logic here
                break;
            default:
                Console.WriteLine("Invalid arguments. Usage:");
                Console.WriteLine("To process JSON files: obs_test <inputFilePath> <outputFilePath>");
                Console.WriteLine("To run REST API: obs_test");
                break;
        }
    }
    private static void ProcessJsonFile(string[] args, ServiceProvider serviceProvider)
    {
        // If two arguments are provided, treat as file paths
        var inputFilePath = args[0];
        var outputFilePath = args[1];
        try
        {
            var inputJson = File.ReadAllText(inputFilePath);
            var inputData = JsonConvert.DeserializeObject<InputData>(inputJson);

            if (inputData == null)
            {
                Console.WriteLine("Error: Failed to deserialize input data from JSON.");
                return;
            }

            var validator = new InputDataValidator();
            var validationResult = validator.Validate(inputData);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return;
            }
            // Run simulation
            IRobot robot = new Robot(inputData);
            IRobotSimulatorService simulatorService = serviceProvider.GetRequiredService<IRobotSimulatorService>();
            var result = simulatorService.SimulateRobot(robot);

            // Write output to JSON file
            //string outputFilePath = "output.json"; // Change to your output file path
            string outputJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText(outputFilePath, outputJson);

            Console.WriteLine("Simulation complete. Output written to output.json");
            GenerateColoredMap(inputData, result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
    static void GenerateMap(InputData inputData, SimulationResult result)
    {
        for (int y = 0; y < inputData.Terrain.Length; y++)
        {
            for (int x = 0; x < inputData.Terrain[0].Length; x++)
            {
                if (result.VisitedCells.Any(cell => cell.X == x && cell.Y == y))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("X ");
                }
                else if (result.FinalPosition.Location.X == x && result.FinalPosition.Location.Y == y)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("F ");
                }
                else
                {
                    Console.Write($"{inputData.Terrain[y][x]} ");
                }
            }
            Console.WriteLine();
        }
    }
    static void GenerateColoredMap(InputData inputData, SimulationResult result)
    {
        foreach (var row in inputData.Terrain)
        {
            foreach (var cell in row)
            {
                if ((result.VisitedCells.Any(p => inputData.Terrain[p.Y][p.X] == cell)))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("X ");
                }
                else if (result.FinalPosition.Location.X == Array.IndexOf(row, cell) && result.FinalPosition.Location.Y == Array.IndexOf(inputData.Terrain, row))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("F ");
                }
                else
                {
                    Console.ResetColor();
                    switch (cell)
                    {
                        case "Fe":
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case "Se":
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        case "W":
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case "Si":
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case "Zn":
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case "Obs":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                    }
                    Console.Write($"{cell} ");
                }
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

}