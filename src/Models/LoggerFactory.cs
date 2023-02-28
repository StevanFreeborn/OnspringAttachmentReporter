using Serilog.Core;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace OnspringAttachmentReporter.Models;

static class LoggerFactory
{
  public static Logger CreateLogger(LogEventLevel logLevel, string outputDirectory)
  {
    return new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(new CompactJsonFormatter(), GetLogPath(outputDirectory))
    .WriteTo.Console(
      restrictedToMinimumLevel: logLevel,
      theme: AnsiConsoleTheme.Code
    )
    .CreateLogger();
  }

  internal static string GetLogPath(string outputDirectory)
  {
    var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    return Path.Combine(currentDirectory, outputDirectory, "log.json");
  }
}