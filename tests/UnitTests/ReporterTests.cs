using FileInfo = OnspringAttachmentReporter.Models.FileInfo;

namespace OnspringAttachmentReporterTests.UnitTests;

public class ReporterTests
{
  private readonly Mock<IContext> _contextMock;
  private readonly Mock<IProcessor> _processorMock;
  private readonly Mock<ILogger> _loggerMock;

  public ReporterTests()
  {
    _contextMock = new Mock<IContext>();
    _processorMock = new Mock<IProcessor>();
    _loggerMock = new Mock<ILogger>();
  }

  [Fact]
  public void GetContext_WhenCalledAndApiKeyIsNullAndNoConfigIsProvided_ItShouldThrowAnException()
  {

    Action action = () => Reporter.GetContext(null, 1, LogEventLevel.Information, null);
    action.Should().Throw<Exception>();
  }

  [Fact]
  public void GetContext_WhenCalledAndApiKeyIsEmptyAndNoConfigIsProvided_ItShouldThrowAnException()
  {
    Action action = () => Reporter.GetContext("", 1, LogEventLevel.Information, null);
    action.Should().Throw<Exception>();
  }

  [Fact]
  public void GetContext_WhenCalledAndApiKeyIsWhiteSpaceAndNoConfigIsProvided_ItShouldThrowAnException()
  {
    Action action = () => Reporter.GetContext(" ", 1, LogEventLevel.Information, null);
    action.Should().Throw<Exception>();
  }

  [Fact]
  public void GetContext_WhenCalledAndAppIdIsNullAndNoConfigIsProvided_ItShouldThrowAnException()
  {
    Action action = () => Reporter.GetContext("apiKey", null, LogEventLevel.Information, null);
    action.Should().Throw<Exception>();
  }

  [Fact]
  public void GetContext_WhenCalledAndApiKeyAppIdAndConfigAreNull_ItShouldThrowAnException()
  {
    Action action = () => Reporter.GetContext(null, null, LogEventLevel.Information, null);
    action.Should().Throw<Exception>();
  }

  [Fact]
  public void GetContext_WhenCalledAndApiKeyAndAppIdAreProvided_ItShouldReturnAContext()
  {
    var context = Reporter.GetContext("apiKey", 1, LogEventLevel.Information, null);
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    context.Should().NotBeNull();
    context.ApiKey.Should().Be("apiKey");
    context.AppId.Should().Be(1);
    context.LogLevel.Should().Be(LogEventLevel.Information);
    context.OutputDirectory.Should().Be(outputDirectory);
  }

  [Fact]
  public void GetContext_WhenCalledAndApiKeyAppIdAndConfigAreProvided_ItShouldReturnAContextAccordingToApiKeyAndAppId()
  {
    var context = Reporter.GetContext("apiKey", 1, LogEventLevel.Information, "config.json");
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    context.Should().NotBeNull();
    context.ApiKey.Should().Be("apiKey");
    context.AppId.Should().Be(1);
    context.LogLevel.Should().Be(LogEventLevel.Information);
    context.OutputDirectory.Should().Be(outputDirectory);
  }

  [Fact]
  public void GetContext_WhenCalledAndConfigIsProvidedWithoutAppIdAndApiKey_ItShouldReturnAContext()
  {
    var context = Reporter.GetContext(null, null, LogEventLevel.Information, "./testData/config.valid.json");
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";
    context.Should().NotBeNull();
    context.ApiKey.Should().Be("apiKey");
    context.AppId.Should().Be(1);
    context.LogLevel.Should().Be(LogEventLevel.Information);
    context.OutputDirectory.Should().Be(outputDirectory);
  }

  [Theory]
  [InlineData("./testData/config.invalid.json")]
  [InlineData("./testData/config.null.json")]
  public void GetContext_WhenCalledAndInvalidConfigIsProvided_ItShouldThrowAnException(string configFilePath)
  {
    Action action = () => Reporter.GetContext(null, null, LogEventLevel.Information, configFilePath);
    action.Should().Throw<Exception>();
  }

  [Fact]
  public async Task Run_WhenCalledAndFileInfoIsFound_ItShouldReturnZero()
  {
    var fileFields = new List<Field>
    {
      new Field
      {
        Id = 1,
        Name = "field1",
        Type = FieldType.Text,
        Status = FieldStatus.Enabled,
        IsRequired = true,
        IsUnique = false,
      }
    };

    var fileRequests = new List<FileInfoRequest>
    {
      new FileInfoRequest(1, 2, "test", 1)
    };

    var fileInfos = new List<FileInfo>
    {
      new FileInfo(1, 1, "test", 1, "test", 1000),
    };

    _processorMock
    .Setup(m => m.GetFileFields().Result)
    .Returns(fileFields);

    _processorMock
    .Setup(m => m.GetFileRequests(fileFields).Result)
    .Returns(fileRequests);

    _processorMock
    .Setup(m => m.GetFileInfos(fileRequests).Result)
    .Returns(fileInfos);

    _processorMock
    .Setup(m => m.PrintReport(fileInfos));

    _contextMock.Setup(m => m.OutputDirectory).Returns("output");

    var reporter = new Reporter(
      _contextMock.Object,
      _processorMock.Object,
      _loggerMock.Object
    );

    var result = await reporter.Run();

    result.Should().Be(0);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
    _processorMock.Verify(m => m.GetFileRequests(It.IsAny<List<Field>>()), Times.Once);
    _processorMock.Verify(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()), Times.Once);
    _processorMock.Verify(m => m.PrintReport(It.IsAny<List<FileInfo>>()), Times.Once);
  }

  [Fact]
  public async Task Run_WhenCalledAndFileInfoIsNotFound_ItShouldReturnThree()
  {
    var fileFields = new List<Field>
    {
      new Field
      {
        Id = 1,
        Name = "field1",
        Type = FieldType.Text,
        Status = FieldStatus.Enabled,
        IsRequired = true,
        IsUnique = false,
      }
    };

    var fileRequests = new List<FileInfoRequest>
    {
      new FileInfoRequest(1, 2, "test", 1)
    };

    var fileInfos = new List<FileInfo>();

    _processorMock
    .Setup(m => m.GetFileFields().Result)
    .Returns(fileFields);

    _processorMock
    .Setup(m => m.GetFileRequests(fileFields).Result)
    .Returns(fileRequests);

    _processorMock
    .Setup(m => m.GetFileInfos(fileRequests).Result)
    .Returns(fileInfos);

    var reporter = new Reporter(
      _contextMock.Object,
      _processorMock.Object,
      _loggerMock.Object
    );

    var result = await reporter.Run();

    result.Should().Be(3);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
    _processorMock.Verify(m => m.GetFileRequests(It.IsAny<List<Field>>()), Times.Once);
    _processorMock.Verify(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()), Times.Once);
    _processorMock.Verify(m => m.PrintReport(It.IsAny<List<FileInfo>>()), Times.Never);
  }

  [Fact]
  public async Task Run_WhenCalledAndFileRequestsAreNotFound_ItShouldReturnTwo()
  {
    var fileFields = new List<Field>
    {
      new Field
      {
        Id = 1,
        Name = "field1",
        Type = FieldType.Text,
        Status = FieldStatus.Enabled,
        IsRequired = true,
        IsUnique = false,
      }
    };

    var fileRequests = new List<FileInfoRequest>();

    _processorMock
    .Setup(m => m.GetFileFields().Result)
    .Returns(fileFields);

    _processorMock
    .Setup(m => m.GetFileRequests(fileFields).Result)
    .Returns(fileRequests);

    _contextMock.Setup(m => m.OutputDirectory).Returns("output");

    var reporter = new Reporter(
      _contextMock.Object,
      _processorMock.Object,
      _loggerMock.Object
    );

    var result = await reporter.Run();

    result.Should().Be(2);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
    _processorMock.Verify(m => m.GetFileRequests(It.IsAny<List<Field>>()), Times.Once);
    _processorMock.Verify(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()), Times.Never);
    _processorMock.Verify(m => m.PrintReport(It.IsAny<List<FileInfo>>()), Times.Never);
  }

  [Fact]
  public async Task Run_WhenCalledAndFileFieldsAreNotFound_ItShouldReturnOne()
  {
    var fileFields = new List<Field>();

    _processorMock
    .Setup(m => m.GetFileFields().Result)
    .Returns(fileFields);

    var reporter = new Reporter(
      _contextMock.Object,
      _processorMock.Object,
      _loggerMock.Object
    );

    var result = await reporter.Run();

    result.Should().Be(1);
    _processorMock.Verify(m => m.GetFileFields(), Times.Once);
    _processorMock.Verify(m => m.GetFileRequests(It.IsAny<List<Field>>()), Times.Never);
    _processorMock.Verify(m => m.GetFileInfos(It.IsAny<List<FileInfoRequest>>()), Times.Never);
    _processorMock.Verify(m => m.PrintReport(It.IsAny<List<FileInfo>>()), Times.Never);
  }
}