namespace OnspringAttachmentReporterTests.UnitTests;

public class ProcessorTests
{
  private readonly Mock<IOnspringService> _onspringServiceMock;
  private readonly Mock<IReportService> _reportServiceMock;
  private readonly Mock<ILogger> _loggerMock;

  public ProcessorTests()
  {
    _onspringServiceMock = new Mock<IOnspringService>();
    _reportServiceMock = new Mock<IReportService>();
    _loggerMock = new Mock<ILogger>();
  }

  [Fact]
  public async Task GetFileFields_WhenCalledAndNoFileFieldsFound_ItShouldReturnAnEmptyList()
  {
    var fields = new List<Field>
    {
      new Field { Id = 1, Name = "test", Type = FieldType.Text },
      new Field { Id = 2, Name = "test2", Type = FieldType.Text }
    };

    _onspringServiceMock
    .Setup(m => m.GetAllFields(It.IsAny<int>()).Result)
    .Returns(fields);

    var processor = new Processor(
      _onspringServiceMock.Object,
      _reportServiceMock.Object,
      _loggerMock.Object
    );

    var result = await processor.GetFileFields();

    result.Should().BeEmpty();
    _onspringServiceMock.Verify(m => m.GetAllFields(It.IsAny<int>()), Times.Once);
  }

  [Fact]
  public async Task GetFileFields_WhenCalledAndImageFieldIsFound_ItShouldReturnAListOfFileFields()
  {
    var fileFieldId = 2;
    var fileFieldName = "test2";
    var fileFieldType = FieldType.Image;

    var fields = new List<Field>
    {
      new Field { Id = 1, Name = "test", Type = FieldType.Text },
      new Field { Id = fileFieldId, Name = fileFieldName, Type = fileFieldType }
    };

    _onspringServiceMock
    .Setup(m => m.GetAllFields(It.IsAny<int>()).Result)
    .Returns(fields);

    var processor = new Processor(
      _onspringServiceMock.Object,
      _reportServiceMock.Object,
      _loggerMock.Object
    );

    var result = await processor.GetFileFields();

    result.Should().HaveCount(1);
    result.First().Id.Should().Be(fileFieldId);
    result.First().Name.Should().Be(fileFieldName);
    result.First().Type.Should().Be(fileFieldType);
    _onspringServiceMock.Verify(m => m.GetAllFields(It.IsAny<int>()), Times.Once);
  }

  [Fact]
  public async Task GetFileFields_WhenCalledAndAttachmentFieldIsFound_ItShouldReturnAListOfFileFields()
  {
    var fileFieldId = 2;
    var fileFieldName = "test2";
    var fileFieldType = FieldType.Attachment;

    var fields = new List<Field>
    {
      new Field { Id = 1, Name = "test", Type = FieldType.Text },
      new Field { Id = fileFieldId, Name = fileFieldName, Type = fileFieldType }
    };

    _onspringServiceMock
    .Setup(m => m.GetAllFields(It.IsAny<int>()).Result)
    .Returns(fields);

    var processor = new Processor(
      _onspringServiceMock.Object,
      _reportServiceMock.Object,
      _loggerMock.Object
    );

    var result = await processor.GetFileFields();

    result.Should().HaveCount(1);
    result.First().Id.Should().Be(fileFieldId);
    result.First().Name.Should().Be(fileFieldName);
    result.First().Type.Should().Be(fileFieldType);
    _onspringServiceMock.Verify(m => m.GetAllFields(It.IsAny<int>()), Times.Once);
  }

  [Fact]
  public async Task GetFileFields_WhenCalledAndBothImageAndAttachmentFieldsAreFound_ItShouldReturnAListOfFileFields()
  {
    var attachmentFieldId = 2;
    var attachmentFieldName = "test2";
    var attachmentFieldType = FieldType.Attachment;

    var imageFieldId = 3;
    var imageFieldName = "test3";
    var imageFieldType = FieldType.Image;

    var fields = new List<Field>
    {
      new Field { Id = 1, Name = "test", Type = FieldType.Text },
      new Field { Id = attachmentFieldId, Name = attachmentFieldName, Type = attachmentFieldType },
      new Field { Id = imageFieldId, Name = imageFieldName, Type = imageFieldType }
    };

    _onspringServiceMock
    .Setup(m => m.GetAllFields(It.IsAny<int>()).Result)
    .Returns(fields);

    var processor = new Processor(
      _onspringServiceMock.Object,
      _reportServiceMock.Object,
      _loggerMock.Object
    );

    var result = await processor.GetFileFields();

    result.Should().HaveCount(2);
    result.First().Id.Should().Be(attachmentFieldId);
    result.First().Name.Should().Be(attachmentFieldName);
    result.First().Type.Should().Be(attachmentFieldType);
    result.Last().Id.Should().Be(imageFieldId);
    result.Last().Name.Should().Be(imageFieldName);
    result.Last().Type.Should().Be(imageFieldType);
    _onspringServiceMock.Verify(m => m.GetAllFields(It.IsAny<int>()), Times.Once);
  }

  [Fact]
  public async Task GetFileRequests_WhenCalledAndNoRecordsAreFound_ItShouldReturnAnEmptyList()
  {
    _onspringServiceMock
    .Setup(m => m.GetAPageOfRecords(It.IsAny<List<int>>(), It.IsAny<PagingRequest>()).Result)
    .Returns<List<ResultRecord>>(null);

    var processor = new Processor(
      _onspringServiceMock.Object,
      _reportServiceMock.Object,
      _loggerMock.Object
    );

    var fileFields = new List<Field>
    {
      new Field { Id = 1, Name = "attachment", Type = FieldType.Attachment },
      new Field { Id = 2, Name = "image", Type = FieldType.Image }
    };

    var result = await processor.GetFileRequests(fileFields);

    result.Should().BeEmpty();
    _onspringServiceMock.Verify(m =>
      m.GetAPageOfRecords(It.IsAny<List<int>>(), It.IsAny<PagingRequest>()),
      Times.Once
    );
  }

  [Fact]
  public async Task GetFileRequests_WhenCalledAndRecordsAreFound_ItShouldReturnAListOfFileRequests()
  {
    var attachmentFieldId = 1;
    var attachmentFieldName = "attachment";
    var attachmentFieldType = FieldType.Attachment;

    var imageFieldId = 2;
    var imageFieldName = "image";
    var imageFieldType = FieldType.Image;

    var fileFields = new List<Field>
    {
      new Field { Id = attachmentFieldId, Name = attachmentFieldName, Type = attachmentFieldType },
      new Field { Id = imageFieldId, Name = imageFieldName, Type = imageFieldType }
    };

    var records = new List<ResultRecord>
    {
      new ResultRecord
      {
        AppId = 1,
        RecordId = 1,
        FieldData = new List<RecordFieldValue>
        {
          new AttachmentListFieldValue
          {
            FieldId = attachmentFieldId,
            Value = new List<AttachmentFile>
            {
              new AttachmentFile
              {
                FileId = 1,
                FileName = "attachment1",
                Notes = "notes1",
                StorageLocation = FileStorageSite.Internal,
                DownloadLink = "downloadLink1",
                QuickEditLink = "quickEditLink1",
              },
              new AttachmentFile
              {
                FileId = 2,
                FileName = "attachment1",
                Notes = "notes1",
                StorageLocation = FileStorageSite.Internal,
                DownloadLink = "downloadLink1",
                QuickEditLink = "quickEditLink1",
              }
            }
          },
          new FileListFieldValue
          {
            FieldId = imageFieldId,
            Value = new List<int> { 3, 4, }
          }
        }
      }
    };

    var res = new GetPagedRecordsResponse
    {
      PageNumber = 1,
      TotalPages = 1,
      TotalRecords = 1,
      Items = records,
    };

    _onspringServiceMock
    .Setup(m => m.GetAPageOfRecords(It.IsAny<List<int>>(), It.IsAny<PagingRequest>()).Result)
    .Returns(res);

    var processor = new Processor(
      _onspringServiceMock.Object,
      _reportServiceMock.Object,
      _loggerMock.Object
    );

    var result = await processor.GetFileRequests(fileFields);

    result.Should().HaveCount(4);
    result.Should().BeOfType<List<FileInfoRequest>>();

    result
    .Select(f => f.RecordId)
    .Distinct()
    .Should().HaveCount(1);

    result
    .Select(f => f.RecordId)
    .Distinct()
    .ToList().First().Should().Be(1);

    result
    .Select(f => f.FieldId)
    .Distinct().Should().HaveCount(2);

    result
    .Select(f => f.FieldId)
    .Distinct()
    .ToList().Should().BeEquivalentTo(new List<int> { attachmentFieldId, imageFieldId });

    result
    .Select(f => f.FileId)
    .ToList().Should().BeEquivalentTo(new List<int> { 1, 2, 3, 4 });

    _onspringServiceMock.Verify(m =>
      m.GetAPageOfRecords(It.IsAny<List<int>>(), It.IsAny<PagingRequest>()),
      Times.Once
    );
  }
}