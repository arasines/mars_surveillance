using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using obs_test.Application;
using obs_test.Application.Services;
using obs_test.Domain.Models;
using obs_test.Domain.Validators;
using System.Reflection.Emit;

namespace obs_test_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RobotController : ControllerBase
    {

        private readonly ILogger<RobotController> _logger;
        private readonly IRobotSimulatorService _simulatorService;
        public RobotController(ILogger<RobotController> logger, IRobotSimulatorService simulatorService)
        {
            _logger = logger;
            _simulatorService = simulatorService;
        }

        [HttpPost]
        [Route("simulate")]
        [ProducesResponseType(typeof(JObject), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProcessJson([FromForm] IFormFile inputJsonFile)
        {
            try
            {
                // Read input data from the input JSON file
                using var inputStream = inputJsonFile.OpenReadStream();
                var inputJson = await new StreamReader(inputStream).ReadToEndAsync();
                var inputData = JsonConvert.DeserializeObject<InputData>(inputJson);
                if (inputData == null)
                {
                    return BadRequest("Error: Failed to deserialize input data from JSON.");
                }
                var validator = new InputDataValidator();
                var validationResult = validator.Validate(inputData);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
                }
                // Run simulation
                IRobot robot = new Robot(inputData);
                var result = _simulatorService.SimulateRobot(robot);

                // Create a unique filename for the output file
                var outputFileName = Guid.NewGuid().ToString() + ".json";
                // Determine the full path for the output file
                var outputPath = Path.Combine(Path.GetTempPath(), outputFileName);
                // Serialize the result to JSON
                string resultJson = JsonConvert.SerializeObject(result, Formatting.Indented);

                // Write the JSON result to the output file
                object value = File.WriteAllText(outputPath, resultJson);

                // Read the output file content
                var memory = new MemoryStream();
                using (var stream = new FileStream(outputPath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                // Serve the downloadable output file
                return File(memory, "application/json", outputFileName);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
    }
}