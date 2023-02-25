using Onspring.API.SDK;

namespace OnspringAttachmentReporterTests.ModelsTests;

public class OnspringServiceTests
{
  [Fact]
  public void OnspringService_WhenConstructorIsCalledWithEmptyApiKey_ShouldThrowArgumentException()
  {
    Action action = () => { _ = new OnspringService(string.Empty); };
    action.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void OnspringService_WhenConstructorIsCalledWithWhiteSpaceApiKey_ShouldThrowArgumentException()
  {
    Action action = () => { _ = new OnspringService(" "); };
    action.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void OnspringService_WhenConstructorIsCalledWithNullApiKey_ShouldThrowArgumentException()
  {
    Action action = () => { _ = new OnspringService(null); };
    action.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void OnspringService_WhenConstructorIsCalledWithApiKey_ShouldReturnANewInstance()
  {
    var onspringService = new OnspringService("apiKey");
    onspringService.Should().NotBeNull();
    onspringService.Should().BeOfType<OnspringService>();
    onspringService._client.Should().NotBeNull();
    onspringService._client.Should().BeOfType<OnspringClient>();
  }
}