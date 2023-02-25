namespace OnspringAttachmentReporter.Models;

public class Context
{
  public string? ApiKey { get; }
  public int AppId { get; }

  public Context(string? apiKey, int appId)
  {
    ApiKey = apiKey;
    AppId = appId;
  }
}