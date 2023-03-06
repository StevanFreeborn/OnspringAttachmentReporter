namespace OnspringAttachmentReporter.Models;

public class Context : IContext
{
  public string? ApiKey { get; }
  public int AppId { get; }
  public string OutputDirectory { get; }
  public LogEventLevel LogLevel { get; }
  public List<int> FilesFilter { get; } = new List<int>();

  public Context(
    string? apiKey,
    int appId,
    string outputDirectory,
    LogEventLevel logLevel
  )
  {
    ApiKey = apiKey;
    AppId = appId;
    OutputDirectory = outputDirectory;
    LogLevel = logLevel;
  }

  public Context(
    string? apiKey,
    int appId,
    string outputDirectory,
    LogEventLevel logLevel,
    List<int> filesFilter
  ) : this(apiKey, appId, outputDirectory, logLevel)
  {
    FilesFilter = filesFilter;
  }
}