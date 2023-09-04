using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using obs_test.Application.Extensions;
using obs_test.Application.UseCases.Commands;
using obs_test.Domain.Models;
using obs_test.Domain.Resources;
using obs_test.Domain.Validators;

namespace obs_test_api.Controllers;

[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Produces("application/json")]
[Route("api/[controller]")]
public class RobotController : ControllerBase
{
    private readonly ILogger<RobotController> _logger;
    private readonly IMediator _mediator;

    /// <summary>
    ///   Initializes a new instance of the <see cref="RobotController" /> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    public RobotController(ILogger<RobotController> logger, IMediator mediator)
    {
        Guard.Against.Null(mediator, nameof(mediator));
        Guard.Against.Null(logger, nameof(logger));
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    ///   Executes the Robot Simulation
    /// </summary>
    /// <param name="file">The simulation parameters</param>
    /// <response code="200">The simulation result: visited cells, samples collected and final position</response>
    /// <response code="400">Mmissing/invalid values</response>
    /// <response code="500">Oops! Can't execute the simulation right now</response>
    [HttpPost]
    [ProducesResponseType(typeof(SimulationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ProcessJson(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded." });

            // Read input data from the input JSON file
            var inputData = await JsonFileUtils.ReadJsonObjectAsync(file);
            if (inputData == null) return BadRequest(Resources.Error_DeserializeJSON);
            var validator = new InputDataValidator();
            var validationResult = await validator.ValidateAsync(inputData);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
            // Run simulation
            var result = await _mediator.Send(new RobotSimulatorCommand(inputData));

            //// Create a unique filename for the output file
            //var outputFileName = Guid.NewGuid() + ".json";
            //// Determine the full path for the output file
            //var outputFilePath = Path.Combine(Path.GetTempPath(), outputFileName);
            //// Serialize the result to JSON and Write the JSON to the output file
            //JsonFileUtils.PrettyWrite(result, outputFilePath);

            //// Read the output file content
            //var memory = new MemoryStream();
            //await using (var stream = new FileStream(outputFilePath, FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;

            // Serve the downloadable output file
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Handle exceptions and return an error response
            _logger.LogError(ex, "Error: {Message}", ex.Message);
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}