using FileInfo = OnspringAttachmentReporter.Models.FileInfo;

namespace OnspringAttachmentReporterTests.UnitTests;

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
    _processorMock
    .Setup(m => m.GetFileFields().Result)
    .Returns(new List<Field>());

    var runner = new Runner(_processorMock.Object, _loggerMock.Object);
    var result = await runner.Run();

    result.Should().BeGreaterThan(0);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
    _processorMock.Verify(m => m.GetFileRequests(It.IsAny<List<Field>>()), Times.Never);
    _processorMock.Verify(m => m.PrintReport(It.IsAny<List<FileInfo>>()), Times.Never);
  }

  [Fact]
  public async Task Run_WhenCalledAndNoFilesFound_ItShouldReturnNonZeroValue()
  {
    _processorMock
    .Setup(m => m.GetFileFields().Result)
    .Returns(new List<Field> { new Field() });

    _processorMock
    .Setup(m => m.GetFileRequests(It.IsAny<List<Field>>()).Result)
    .Returns(new List<FileInfoRequest>());

    var runner = new Runner(_processorMock.Object, _loggerMock.Object);
    var result = await runner.Run();

    result.Should().BeGreaterThan(0);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
    _processorMock.Verify(m => m.GetFileRequests(It.IsAny<List<Field>>()), Times.Once);
    _processorMock.Verify(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()), Times.Never);
    _processorMock.Verify(m => m.PrintReport(It.IsAny<List<FileInfo>>()), Times.Never);
  }

  [Fact]
  public async Task Run_WhenCalledAndNoFilesInformationFound_ItShouldReturnNonZeroValue()
  {
    _processorMock
    .Setup(m => m.GetFileFields().Result)
    .Returns(new List<Field> { new Field() });

    _processorMock
    .Setup(m => m.GetFileRequests(It.IsAny<List<Field>>()).Result)
    .Returns(new List<FileInfoRequest> { new FileInfoRequest(1, 1, "test", 1) });

    _processorMock
    .Setup(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()).Result)
    .Returns(new List<FileInfo>());

    var runner = new Runner(_processorMock.Object, _loggerMock.Object);
    var result = await runner.Run();

    result.Should().BeGreaterThan(0);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
    _processorMock.Verify(m => m.GetFileRequests(It.IsAny<List<Field>>()), Times.Once);
    _processorMock.Verify(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()), Times.Once);
    _processorMock.Verify(m => m.PrintReport(It.IsAny<List<FileInfo>>()), Times.Never);
  }

  [Fact]
  public async Task Run_WhenCalledAndFileInformationFound_ItShouldReturnZero()
  {
    _processorMock
    .Setup(m => m.GetFileFields().Result)
    .Returns(new List<Field> { new Field() });

    _processorMock
    .Setup(m => m.GetFileRequests(It.IsAny<List<Field>>()).Result)
    .Returns(new List<FileInfoRequest> { new FileInfoRequest(1, 1, "test", 1) });

    _processorMock
    .Setup(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()).Result)
    .Returns(new List<FileInfo> { new FileInfo(1, 1, "test", 1, "test", 1) });

    var runner = new Runner(_processorMock.Object, _loggerMock.Object);
    var result = await runner.Run();

    result.Should().Be(0);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
    _processorMock.Verify(m => m.GetFileRequests(It.IsAny<List<Field>>()), Times.Once);
    _processorMock.Verify(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()), Times.Once);
    _processorMock.Verify(m => m.PrintReport(It.IsAny<List<FileInfo>>()), Times.Once);
  }
}