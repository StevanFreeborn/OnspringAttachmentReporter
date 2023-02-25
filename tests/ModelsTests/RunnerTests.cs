using Serilog.Events;

namespace OnspringAttachmentReporterTests.ModelsTests;

public class RunnerTests
{
  [Fact]
  public async Task Run_WhenCalledWithoutNecessaryConfigOptions_ShouldReturnOne()
  {
    var result = await Runner.Run(null, null, null, LogEventLevel.Verbose);
    result.Should().Be(1);
  }

  [Fact]
  public async Task Run_WhenCalledWithApiKeyAndAppId_ShouldReturnZero()
  {
    var result = await Runner.Run("apiKey", 1, null, LogEventLevel.Verbose);
    result.Should().Be(0);
  }

  [Fact]
  public async Task Run_WhenCalledWithConfigFile_ShouldReturnZero()
  {
    var result = await Runner.Run(null, null, "appsettings.test.json", LogEventLevel.Verbose);
    result.Should().Be(0);
  }
}