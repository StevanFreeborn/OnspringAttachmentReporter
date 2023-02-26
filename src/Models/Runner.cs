namespace OnspringAttachmentReporter.Models;

class Runner
{
  private readonly IProcessor _processor;
  private readonly ILogger _logger;

  public Runner(IProcessor processor, ILogger logger)
  {
    _processor = processor;
    _logger = logger;
  }

  public async Task<int> Run()
  {
    _logger.Information("Retrieving file fields.");

    var fileFields = await _processor.GetFileFields();

    if (fileFields.Count == 0)
    {
      _logger.Warning("No file fields found.");
      return 2;
    }

    _logger.Information("File fields retrieved. {Count} file fields found.", fileFields.Count);
    _logger.Information("Retrieving files that need to be requested.");

    var fileRequests = await _processor.GetFileRequests(fileFields);

    if (fileRequests.Count == 0)
    {
      _logger.Warning("No files found.");
      return 3;
    }

    _logger.Information("Files retrieved. {Count} files found.", fileRequests.Count);
    _logger.Information("Retrieving information for each file.");

    var fileInfos = await _processor.GetFileInfos(fileRequests);

    if (fileInfos.Count == 0)
    {
      _logger.Warning("No files info found.");
      return 4;
    }

    _logger.Information("File info retrieved for {Count} files.", fileInfos.Count);
    _logger.Information("Writing attachments report.");

    _processor.PrintReport(fileInfos);

    _logger.Information("Finished writing attachments report:");

    return 0;
  }
}