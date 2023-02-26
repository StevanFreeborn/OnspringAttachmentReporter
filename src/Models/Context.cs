namespace OnspringAttachmentReporter.Models;

public class Context : IContext
{
  public string? ApiKey { get; }
  public int AppId { get; }
  public string OutputDirectory { get; }

  public Context(string? apiKey, int appId, string outputDirectory)
  {
    ApiKey = apiKey;
    AppId = appId;
    OutputDirectory = outputDirectory;
  }
}