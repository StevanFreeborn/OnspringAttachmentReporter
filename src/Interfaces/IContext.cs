namespace OnspringAttachmentReporter.Interfaces;

public interface IContext
{
  string? ApiKey { get; }
  int AppId { get; }
  string OutputDirectory { get; }
  LogEventLevel LogLevel { get; }
}