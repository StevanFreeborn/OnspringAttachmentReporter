namespace OnspringAttachmentReporterTests.UnitTests;

public class ContextTests
{
  [Fact]
  public void Context_WhenConstructorIsCalled_ShouldReturnANewInstance()
  {
    var apiKey = "apiKey";
    var appId = 1;
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    var logLevel = LogEventLevel.Information;
    var filesFilter = new List<int>();
    var result = new Context(
      apiKey,
      appId,
      outputDirectory,
      logLevel,
      filesFilter
    );

    result.Should().NotBeNull();
    result.Should().BeOfType<Context>();
    result.ApiKey.Should().Be("apiKey");
    result.AppId.Should().Be(1);
    result.OutputDirectory.Should().Be(outputDirectory);
    result.LogLevel.Should().Be(logLevel);
    result.FilesFilter.Should().BeEquivalentTo(filesFilter);
  }
}