namespace OnspringAttachmentReporterTests.UnitTests;

public class ContextExtensionsTexts
{
  [Fact]
  public void ConfigureServices_WhenCalled_ShouldReturnAServiceCollection()
  {
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    var context = new Context(
      "apiKey",
      1,
      outputDirectory,
      LogEventLevel.Information,
      new List<int>()
    );
    var result = context.ConfigureServices();
    result.Should().NotBeNull();
    result.Should().BeOfType<ServiceCollection>();
  }
}