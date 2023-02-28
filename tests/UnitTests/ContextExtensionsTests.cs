namespace OnspringAttachmentReporterTests.UnitTests;

public class ContextExtensionsTexts
{
  [Fact]
  public void ConfigureServices_WhenCalled_ShouldReturnAServiceCollection()
  {
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    var context = new Context("apiKey", 1, outputDirectory, LogEventLevel.Information);
    var result = context.ConfigureServices();
    result.Should().NotBeNull();
    result.Should().BeOfType<ServiceCollection>();
  }
}