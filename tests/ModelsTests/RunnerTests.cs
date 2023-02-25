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
  public async Task Run_WhenCalledWithNullApiKey_ShouldReturnOne()
  {
    var result = await Runner.Run(null, 1, null, LogEventLevel.Verbose);
    result.Should().Be(1);
  }

  [Fact]
  public async Task Run_WhenCalledWithNullAppId_ShouldReturnOne()
  {
    var result = await Runner.Run("apiKey", null, null, LogEventLevel.Verbose);
    result.Should().Be(1);
  }

  [Fact]
  public async Task Run_WhenCalledWithEmptyApiKey_ShouldReturnOne()
  {
    var result = await Runner.Run(string.Empty, 1, null, LogEventLevel.Verbose);
    result.Should().Be(1);
  }

  [Fact]
  public async Task Run_WhenCalledWithWhiteSpaceApiKey_ShouldReturnOne()
  {
    var result = await Runner.Run(" ", 1, null, LogEventLevel.Verbose);
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
    var result = await Runner.Run(null, null, "./testData/appsettings.valid.json", LogEventLevel.Verbose);
    result.Should().Be(0);
  }

  [Fact]
  public async Task Run_WhenCalledWithConfigFileWithInvalidAppId_ShouldReturnOne()
  {
    var result = await Runner.Run(null, null, "./testData/appsettings.invalidAppId.json", LogEventLevel.Verbose);
    result.Should().Be(1);
  }

  [Fact]
  public async Task Run_WhenCalledWithConfigFileWithNoAppIdOrApiKey_ShouldReturnOne()
  {
    var result = await Runner.Run(null, null, "./testData/appsettings.null.json", LogEventLevel.Verbose);
    result.Should().Be(1);
  }

  [Fact]
  public async Task Run_WhenCalledWithInvalidConfigFilePath_ShouldReturnOne()
  {
    var result = await Runner.Run(null, null, "./testData/appsettings.invalid.json", LogEventLevel.Verbose);
    result.Should().Be(1);
  }

  [Fact]
  public void GetContext_WhenCalledWithApiKeyAndAppId_ShouldReturnContext()
  {
    var context = Runner.GetContext("apiKey", 1, null);
    context.Should().NotBeNull();

    if (context is null)
    {
      return;
    }

    context.Should().BeOfType<Context>();
    context.ApiKey.Should().Be("apiKey");
    context.AppId.Should().Be(1);
  }

  [Fact]
  public void GetContext_WhenCalledWithConfigFile_ShouldReturnContext()
  {
    var context = Runner.GetContext(null, null, "./testData/appsettings.valid.json");
    context.Should().NotBeNull();

    if (context is null)
    {
      return;
    }

    context.Should().BeOfType<Context>();
    context.ApiKey.Should().Be("apiKey");
    context.AppId.Should().Be(1);
  }

  [Fact]
  public void GetContext_WhenCalledWithConfigFileWithInvalidAppId_ShouldReturnNull()
  {
    var context = Runner.GetContext(null, null, "./testData/appsettings.invalidAppId.json");
    context.Should().BeNull();
  }

  [Fact]
  public void GetContext_WhenCalledWithConfigFileWithNoAppIdOrApiKey_ShouldReturnNull()
  {
    var context = Runner.GetContext(null, null, "./testData/appsettings.null.json");
    context.Should().BeNull();
  }

  [Fact]
  public void GetContext_WhenCalledWithInvalidConfigFilePath_ShouldReturnNull()
  {
    var context = Runner.GetContext(null, null, "./testData/appsettings.fake.json");
    context.Should().BeNull();
  }

  [Fact]
  public void GetContext_WhenCalledWithNoConfigOptions_ShouldReturnNull()
  {
    var context = Runner.GetContext(null, null, null);
    context.Should().BeNull();
  }

  [Fact]
  public void GetContext_WhenCalledWithEmptyApiKey_ShouldReturnNull()
  {
    var context = Runner.GetContext(string.Empty, 1, null);
    context.Should().BeNull();
  }

  [Fact]
  public void GetContext_WhenCalledWithWhiteSpaceApiKey_ShouldReturnNull()
  {
    var context = Runner.GetContext(" ", 1, null);
    context.Should().BeNull();
  }
}