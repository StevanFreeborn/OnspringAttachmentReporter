namespace OnspringAttachmentReporter.ModelsTests;

public class ProcessorTests
{
  private readonly Mock<IOnspringService> _onspringServiceMock;

  public ProcessorTests()
  {
    _onspringServiceMock = new Mock<IOnspringService>();
  }

  [Fact]
  public async Task GetFileFields_WhenCalledAndNoFileFieldsFound_ItShouldReturnAnEmptyList()
  {
    _onspringServiceMock.Setup(m => m.GetAllFields(50).Result).Returns(new List<Field>());

    var processor = new Processor(_onspringServiceMock.Object);
    var result = await processor.GetFileFields();

    result.Should().BeEmpty();
    _onspringServiceMock.Verify(m => m.GetAllFields(50), Times.Once);
  }
}