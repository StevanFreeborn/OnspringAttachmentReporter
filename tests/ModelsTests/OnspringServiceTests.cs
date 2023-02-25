using Onspring.API.SDK;

namespace OnspringAttachmentReporterTests.ModelsTests;

public class OnspringServiceTests
{
  [Fact]
  public void OnspringService_WhenConstructorIsCalledWithEmptyApiKey_ShouldThrowArgumentException()
  {
    var context = new Context(string.Empty, 1);
    Action action = () => { _ = new OnspringService(context); };
    action.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void OnspringService_WhenConstructorIsCalledWithWhiteSpaceApiKey_ShouldThrowArgumentException()
  {
    var context = new Context(" ", 1);
    Action action = () => { _ = new OnspringService(context); };
    action.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void OnspringService_WhenConstructorIsCalledWithNullApiKey_ShouldThrowArgumentException()
  {
    var context = new Context(null, 1);
    Action action = () => { _ = new OnspringService(context); };
    action.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void OnspringService_WhenConstructorIsCalledWithApiKey_ShouldReturnANewInstance()
  {
    var context = new Context("apiKey", 1);
    var onspringService = new OnspringService(context);
    onspringService.Should().NotBeNull();
    onspringService.Should().BeOfType<OnspringService>();
    onspringService._client.Should().NotBeNull();
    onspringService._client.Should().BeOfType<OnspringClient>();
  }
}