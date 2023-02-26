namespace OnspringAttachmentReporterTests.UnitTests;

public class LoggerFactoryTests
{
  [Fact]
  public void GetLogPath_WhenCalled_ReturnsProperPath()
  {
    var outputDirectory = @$"{DateTime.Now:yyyyMMddHHmm}-output";
    var expected = Path.Combine(
      AppDomain.CurrentDomain.BaseDirectory, outputDirectory,
      "log.json"
    );

    var result = LoggerFactory.GetLogPath(outputDirectory);

    result.Should().Be(expected);
  }

  [Fact]
  public void CreateLogger_WhenCalled_ReturnsLogger()
  {
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    var logLevel = LogEventLevel.Verbose;
    var result = LoggerFactory.CreateLogger(logLevel, outputDirectory);

    result.Should().NotBeNull();
    result.Should().BeOfType<Logger>();
  }
}