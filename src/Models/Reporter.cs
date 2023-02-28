namespace OnspringAttachmentReporter.Models;

public class Reporter
{
  private readonly IContext _context;
  private readonly IProcessor _processor;
  private readonly ILogger _logger;

  public Reporter(IContext context, IProcessor processor, ILogger logger)
  {
    _context = context;
    _processor = processor;
    _logger = logger;
  }

  public async Task<int> Run()
  {
    _logger.Information("Starting Onspring Attachment Reporter.");

    _logger.Information("Retrieving file fields.");

    var fileFields = await _processor.GetFileFields();

    if (fileFields.Count == 0)
    {
      _logger.Warning("No file fields could be found.");
      return 2;
    }

    _logger.Information(
      "File fields retrieved. {Count} file fields found.",
      fileFields.Count
    );

    _logger.Information("Retrieving files that need to be requested.");

    var fileRequests = await _processor.GetFileRequests(fileFields);

    if (fileRequests.Count == 0)
    {
      _logger.Warning("No files could be found.");
      return 3;
    }

    _logger.Information(
      "Files retrieved. {Count} files found.",
      fileRequests.Count
    );

    _logger.Information("Retrieving information for each file.");

    var fileInfos = await _processor.GetFileInfos(fileRequests);

    if (fileInfos.Count == 0)
    {
      _logger.Warning("No files information could be found.");
      return 4;
    }

    _logger.Information(
      "File info retrieved for {Count} of {Total} files.",
      fileInfos.Count,
      fileRequests.Count
    );

    _logger.Information("Start writing attachments report.");

    _processor.PrintReport(fileInfos);

    _logger.Information("Finished writing attachments report:");

    _logger.Information("Onspring Attachment Reporter finished.");

    _logger.Information(
      "You can find the log and report files in the output directory: {OutputDirectory}",
      Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _context.OutputDirectory)
    );

    await Log.CloseAndFlushAsync();

    return 0;
  }

  public static Context GetContext(
    string? apiKeyOption,
    int? appIdOption,
    LogEventLevel logLevelOption,
    string? configFileOption
  )
  {
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";

    if (
      string.IsNullOrWhiteSpace(apiKeyOption) is false &&
      appIdOption is not null
    )
    {
      return new Context(apiKeyOption!, appIdOption.Value, outputDirectory, logLevelOption);
    }

    if (string.IsNullOrWhiteSpace(configFileOption) is false)
    {
      var config = new ConfigurationBuilder()
        .AddJsonFile(configFileOption, optional: true, reloadOnChange: true)
        .Build();

      var apiKey = config["ApiKey"];
      var appId = config["AppId"];

      if (
        apiKey is not null &&
        appId is not null &&
        int.TryParse(appId, out var appIdInt) is true
      )
      {
        return new Context(apiKey, appIdInt, outputDirectory, logLevelOption);
      }
    }

    throw new ArgumentException("Unable to get context from config file or command line options.");
  }
}