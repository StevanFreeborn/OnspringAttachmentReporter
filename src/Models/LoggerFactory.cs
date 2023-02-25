using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace OnspringAttachmentReporter.Models;

static class LoggerFactory
{
  public static string GetLogPath()
  {
    var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    return Path.Combine(currentDirectory, outputDirectory, "log.json");
  }

  public static Logger CreateLogger(string logPath, LogEventLevel logLevel)
  {
    return new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(new CompactJsonFormatter(), logPath)
    .WriteTo.Console(
      restrictedToMinimumLevel: logLevel,
      theme: AnsiConsoleTheme.Code
    )
    .CreateLogger();
  }
}