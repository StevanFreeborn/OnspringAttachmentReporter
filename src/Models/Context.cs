namespace OnspringAttachmentReporter.Models;

public class Context : IContext
{
  public string? ApiKey { get; }
  public int AppId { get; }
  public string OutputDirectory { get; }
  public LogEventLevel LogLevel { get; }

  public Context(string? apiKey, int appId, string outputDirectory, LogEventLevel logLevel)
  {
    ApiKey = apiKey;
    AppId = appId;
    OutputDirectory = outputDirectory;
    LogLevel = logLevel;
  }
}