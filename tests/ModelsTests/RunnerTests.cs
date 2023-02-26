namespace OnspringAttachmentReporterTests.ModelsTests;

public class RunnerTests
{
  private readonly Mock<IProcessor> _processorMock;
  private readonly Mock<ILogger> _loggerMock;

  public RunnerTests()
  {
    _processorMock = new Mock<IProcessor>();
    _loggerMock = new Mock<ILogger>();
  }

  [Fact]
  public async Task Run_WhenCalledAndNoFileFieldsFound_ItShouldReturnNonZeroValue()
  {
    _processorMock.Setup(m => m.GetFileFields().Result).Returns<List<Field>?>(null);

    var runner = new Runner(_processorMock.Object, _loggerMock.Object);
    var result = await runner.Run();

    result.Should().BeGreaterThan(0);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
  }

  [Fact]
  public async Task Run_WhenCalledAndNoFileFieldsFound_ItShouldReturnNonZeroValue2()
  {
    _processorMock.Setup(m => m.GetFileFields().Result).Returns(new List<Field>());
    _loggerMock.Setup(m => m.Warning("No file fields found."));

    var runner = new Runner(_processorMock.Object, _loggerMock.Object);
    var result = await runner.Run();

    result.Should().BeGreaterThan(0);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
  }
}