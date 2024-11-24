using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LabOrderProcessor.API.Models;

namespace LabOrderProcessor.API.Services
{
    public class FileProcessingService
    {
        private readonly PhiDetectionService _phiDetectionService;
        private readonly ILogger<FileProcessingService> _logger;

        public FileProcessingService(
            PhiDetectionService phiDetectionService,
            ILogger<FileProcessingService> logger)
        {
            _phiDetectionService = phiDetectionService;
            _logger = logger;
        }

        public async Task<List<LabOrderFile>> ProcessFilesAsync(
            IEnumerable<(string Path, Stream Content)> files,
            string outputDirectory)
        {
            var results = new List<LabOrderFile>();

            foreach (var (filePath, contentStream) in files)
            {
                var labOrderFile = new LabOrderFile
                {
                    OriginalFileName = Path.GetFileName(filePath),
                    FullPath = filePath,
                    Status = ProcessingStatus.Processing
                };

                try
                {
                    _logger.LogInformation("Processing file: {FilePath}", filePath);

                    using var reader = new StreamReader(contentStream);
                    labOrderFile.Content = await reader.ReadToEndAsync();
                    labOrderFile.SanitizedContent = _phiDetectionService.RedactPhi(labOrderFile.Content);

                    await SaveSanitizedFileAsync(labOrderFile, outputDirectory);
                    labOrderFile.Status = ProcessingStatus.Completed;

                    _logger.LogInformation("Successfully processed file: {FilePath}", filePath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing file: {FilePath}", filePath);
                    labOrderFile.Status = ProcessingStatus.Failed;
                    labOrderFile.Error = ex.Message;
                }

                results.Add(labOrderFile);
            }

            return results;
        }

        private async Task SaveSanitizedFileAsync(LabOrderFile labOrderFile, string outputDirectory)
        {
            var fileName = Path.GetFileNameWithoutExtension(labOrderFile.OriginalFileName);
            var extension = Path.GetExtension(labOrderFile.OriginalFileName);
            var sanitizedFileName = $"{fileName}_sanitized{extension}";
            var outputPath = Path.Combine(outputDirectory, sanitizedFileName);

            Directory.CreateDirectory(outputDirectory);
            await File.WriteAllTextAsync(outputPath, labOrderFile.SanitizedContent);

            labOrderFile.FullPath = outputPath;
        }
    }
}