namespace OnspringAttachmentReporterTests.ModelsTests;

public class ContextTests
{
  [Fact]
  public void Context_WhenConstructorIsCalled_ShouldReturnANewInstance()
  {
    var result = new Context("apiKey", 1);
    result.Should().NotBeNull();
    result.Should().BeOfType<Context>();
  }
}