using Serilog.Core;
using Serilog.Events;

namespace OnspringAttachmentReporterTests.ModelsTests;

public class LoggerFactoryTests
{
  [Fact]
  public void GetLogPath_WhenCalled_ReturnsProperPath()
  {
    var outputDirectory = @$"{DateTime.Now:yyyyMMddHHmm}-output\log.json";
    var expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputDirectory);
    var result = LoggerFactory.GetLogPath();

    result.Should().Be(expected);
  }

  [Fact]
  public void CreateLogger_WhenCalled_ReturnsLogger()
  {
    var logPath = "log.json";
    var logLevel = LogEventLevel.Verbose;
    var result = LoggerFactory.CreateLogger(logPath, logLevel);

    result.Should().NotBeNull();
    result.Should().BeOfType<Logger>();
  }
}