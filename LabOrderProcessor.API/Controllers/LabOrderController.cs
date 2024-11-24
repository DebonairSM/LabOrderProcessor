using Microsoft.AspNetCore.Mvc;
using LabOrderProcessor.API.Models;
using LabOrderProcessor.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LabOrderProcessor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabOrderController : ControllerBase
{
    private readonly FileProcessingService _fileProcessingService;
    private readonly ILogger<LabOrderController> _logger;
    private readonly IConfiguration _configuration;

    public LabOrderController(
        FileProcessingService fileProcessingService,
        ILogger<LabOrderController> logger,
        IConfiguration configuration)
    {
        _fileProcessingService = fileProcessingService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("process")]
    public async Task<IActionResult> Process([FromForm] ProcessFilesRequest request)
    {
        try
        {
            if (request.Files == null || !request.Files.Any())
            {
                return BadRequest("No files were uploaded");
            }

            var outputDirectory = _configuration.GetValue<string>("FileProcessing:OutputDirectory")
                ?? throw new InvalidOperationException("OutputDirectory configuration is missing");

            var fileInfos = new List<(string Path, Stream Content)>();

            foreach (var file in request.Files)
            {
                var sanitizedFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_sanitized" + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(outputDirectory, sanitizedFileName);

                if (System.IO.File.Exists(filePath))
                {
                    return Conflict($"The file '{sanitizedFileName}' already exists.");
                }

                using var stream = file.OpenReadStream();
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                fileInfos.Add((file.FileName, memoryStream));
            }

            var results = await _fileProcessingService.ProcessFilesAsync(
                fileInfos,
                outputDirectory);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing files");
            return StatusCode(500, "An error occurred while processing the files");
        }
    }
}

public class ProcessFilesRequest
{
    public IFormFileCollection Files { get; set; } = null!;
}