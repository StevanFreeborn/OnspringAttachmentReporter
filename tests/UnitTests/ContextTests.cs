namespace OnspringAttachmentReporterTests.UnitTests;

public class ContextTests
{
  [Fact]
  public void Context_WhenConstructorIsCalled_ShouldReturnANewInstance()
  {
    var apiKey = "apiKey";
    var appId = 1;
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    var result = new Context(apiKey, appId, outputDirectory);
    result.Should().NotBeNull();
    result.Should().BeOfType<Context>();
    result.ApiKey.Should().Be("apiKey");
    result.AppId.Should().Be(1);
    result.OutputDirectory.Should().Be(outputDirectory);
  }
}