
using System.Net;

namespace OnspringAttachmentReporterTests.UnitTests;

public class OnspringServiceTests
{
  private readonly Mock<IContext> _mockContext;
  private readonly Mock<IOnspringClient> _mockClient;
  private readonly Mock<ILogger> _mockLogger;

  public OnspringServiceTests()
  {
    _mockContext = new Mock<IContext>();
    _mockClient = new Mock<IOnspringClient>();
    _mockLogger = new Mock<ILogger>();
  }

  [Fact]
  public async Task GetAllFields_WhenCalledAndNoFieldsAreFound_ShouldReturnAnEmptyList()
  {
    var apiResponse = new ApiResponse<GetPagedFieldsResponse>
    {
      StatusCode = HttpStatusCode.OK,
      Message = "OK",
      Value = new GetPagedFieldsResponse
      {
        Items = new List<Field>(),
        TotalPages = 0,
        TotalRecords = 0,
        PageNumber = 1,
      }
    };

    _mockClient
    .Setup(m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()).Result)
    .Returns(apiResponse);

    var service = new OnspringService(_mockContext.Object, _mockClient.Object, _mockLogger.Object);
    var result = await service.GetAllFields();

    result.Should().BeEmpty();
    _mockClient.Verify(
      m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()),
      Times.Once
    );
  }

  [Fact]
  public async Task GetAllFields_WhenCalledAndOnePageOfFieldsAreFound_ShouldReturnAListOfFields()
  {
    var apiResponse = new ApiResponse<GetPagedFieldsResponse>
    {
      StatusCode = HttpStatusCode.OK,
      Message = "OK",
      Value = new GetPagedFieldsResponse
      {
        Items = new List<Field>
        {
          new Field
          {
            Id = 1,
            Name = "Field 1",
            Type = FieldType.Attachment,
          },
          new Field
          {
            Id = 2,
            Name = "Field 2",
            Type = FieldType.Attachment,
          },
          new Field
          {
            Id = 3,
            Name = "Field 3",
            Type = FieldType.Attachment,
          },
        },
        TotalPages = 1,
        TotalRecords = 3,
        PageNumber = 1,
      }
    };

    _mockClient
    .Setup(m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()).Result)
    .Returns(apiResponse);

    var service = new OnspringService(
      _mockContext.Object,
      _mockClient.Object,
      _mockLogger.Object
    );

    var result = await service.GetAllFields();

    result.Should().HaveCount(3);
    result.Should().BeOfType<List<Field>>();
    _mockClient.Verify(
      m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()),
      Times.Once
    );
  }

  [Fact]
  public async Task GetAllFields_WhenCalledAndMultiplePagesOfFieldsAreFound_ShouldReturnAListOfFields()
  {
    var pageOne = new ApiResponse<GetPagedFieldsResponse>
    {
      StatusCode = HttpStatusCode.OK,
      Message = "OK",
      Value = new GetPagedFieldsResponse
      {
        Items = new List<Field>
        {
          new Field
          {
            Id = 1,
            Name = "Field 1",
            Type = FieldType.Attachment,
          },
          new Field
          {
            Id = 2,
            Name = "Field 2",
            Type = FieldType.Attachment,
          },
          new Field
          {
            Id = 3,
            Name = "Field 3",
            Type = FieldType.Attachment,
          },
        },
        TotalPages = 2,
        TotalRecords = 6,
        PageNumber = 1,
      }
    };

    var pageTwo = new ApiResponse<GetPagedFieldsResponse>
    {
      StatusCode = HttpStatusCode.OK,
      Message = "OK",
      Value = new GetPagedFieldsResponse
      {
        Items = new List<Field>
        {
          new Field
          {
            Id = 4,
            Name = "Field 4",
            Type = FieldType.Attachment,
          },
          new Field
          {
            Id = 5,
            Name = "Field 5",
            Type = FieldType.Attachment,
          },
          new Field
          {
            Id = 6,
            Name = "Field 6",
            Type = FieldType.Attachment,
          },
        },
        TotalPages = 2,
        TotalRecords = 6,
        PageNumber = 2,
      }
    };

    _mockClient
    .SetupSequence(m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()).Result)
    .Returns(pageOne)
    .Returns(pageTwo);

    var service = new OnspringService(
      _mockContext.Object,
      _mockClient.Object,
      _mockLogger.Object
    );

    var result = await service.GetAllFields();

    result.Should().HaveCount(6);
    result.Should().BeOfType<List<Field>>();
    _mockClient.Verify(
      m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()),
      Times.Exactly(2)
    );
  }

  [Fact]
  public async Task GetAllFields_WhenCalledAndMultiplePagesOfFieldsAreFoundAndOnePageReturnsAnError_ItShouldReturnAListOfFieldsAfterRetryingFailedPageThreeTimes()
  {
    var pageOne = new ApiResponse<GetPagedFieldsResponse>
    {
      StatusCode = HttpStatusCode.OK,
      Message = "OK",
      Value = new GetPagedFieldsResponse
      {
        Items = new List<Field>
        {
          new Field
          {
            Id = 1,
            Name = "Field 1",
            Type = FieldType.Attachment,
          },
          new Field
          {
            Id = 2,
            Name = "Field 2",
            Type = FieldType.Attachment,
          },
          new Field
          {
            Id = 3,
            Name = "Field 3",
            Type = FieldType.Attachment,
          },
        },
        TotalPages = 2,
        TotalRecords = 6,
        PageNumber = 1,
      }
    };

    var pageTwo = new ApiResponse<GetPagedFieldsResponse?>
    {
      StatusCode = HttpStatusCode.InternalServerError,
      Message = "Internal Server Error",
      Value = null,
    };

    _mockClient
    .SetupSequence(m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()).Result)
    .Returns(pageOne)
    .Returns(pageTwo)
    .Returns(pageTwo)
    .Returns(pageTwo);


    var service = new OnspringService(
      _mockContext.Object,
      _mockClient.Object,
      _mockLogger.Object
    );

    var result = await service.GetAllFields();

    result.Should().HaveCount(3);
    result.Should().BeOfType<List<Field>>();
    _mockClient.Verify(
      m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()),
      Times.Exactly(4)
    );
  }

  [Fact]
  public async Task GetAPageOfRecords_WhenCalledAndOnePageOfRecordsAreFound_ShouldReturnAListOfRecords()
  {
    var apiResponse = new ApiResponse<GetPagedRecordsResponse>
    {
      StatusCode = HttpStatusCode.OK,
      Message = "OK",
      Value = new GetPagedRecordsResponse
      {
        Items = new List<ResultRecord>
        {
          new ResultRecord
          {
            AppId = 1,
            RecordId = 1,
            FieldData = new List<RecordFieldValue>
            {
              new StringFieldValue
              {
                FieldId = 1,
                Value = "Field 1",
              },
            },
          },
          new ResultRecord
          {
            AppId = 1,
            RecordId = 2,
            FieldData = new List<RecordFieldValue>
            {
              new StringFieldValue
              {
                FieldId = 1,
                Value = "Field 1",
              },
            },
          },
        },
        TotalPages = 1,
        TotalRecords = 2,
        PageNumber = 1,
      }
    };

    _mockClient
    .Setup(m => m.GetRecordsForAppAsync(It.IsAny<GetRecordsByAppRequest>()).Result)
    .Returns(apiResponse);

    var service = new OnspringService(
      _mockContext.Object,
      _mockClient.Object,
      _mockLogger.Object
    );

    var result = await service.GetAPageOfRecords(It.IsAny<List<int>>(), It.IsAny<PagingRequest>());

    result.Should().NotBeNull();
    result.Should().BeOfType<GetPagedRecordsResponse>();

    if (result is null)
    {
      return;
    }

    result.TotalPages.Should().Be(1);
    result.TotalRecords.Should().Be(2);
    result.PageNumber.Should().Be(1);
    result.Items.Should().HaveCount(2);
    result.Items.Should().BeOfType<List<ResultRecord>>();
  }

  [Fact]
  public async Task GetAPageOfRecords_WhenNoRecordsAreFound_ItShouldReturnNullAfterRetryingThreeTimes()
  {
    var apiResponse = new ApiResponse<GetPagedRecordsResponse?>
    {
      StatusCode = HttpStatusCode.InternalServerError,
      Message = "Internal Server Error",
      Value = null,
    };

    _mockClient
    .SetupSequence(m => m.GetRecordsForAppAsync(It.IsAny<GetRecordsByAppRequest>()).Result)
    .Returns(apiResponse)
    .Returns(apiResponse)
    .Returns(apiResponse);

    var service = new OnspringService(
      _mockContext.Object,
      _mockClient.Object,
      _mockLogger.Object
    );

    var result = await service.GetAPageOfRecords(It.IsAny<List<int>>(), It.IsAny<PagingRequest>());

    result.Should().BeNull();
    _mockClient.Verify(
      m => m.GetRecordsForAppAsync(It.IsAny<GetRecordsByAppRequest>()),
      Times.Exactly(3)
    );
  }

  [Fact]
  public async Task GetFile_WhenCalledAndFileIsFound_ShouldReturnAFile()
  {
    var apiResponse = new ApiResponse<GetFileResponse>
    {
      StatusCode = HttpStatusCode.OK,
      Message = "OK",
      Value = new GetFileResponse
      {
        FileName = "test.txt",
        ContentLength = 10,
        ContentType = "text/plain",
        Stream = new MemoryStream(),
      }
    };

    _mockClient
    .Setup(m => m.GetFileAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()).Result)
    .Returns(apiResponse);

    var service = new OnspringService(
      _mockContext.Object,
      _mockClient.Object,
      _mockLogger.Object
    );

    var result = await service.GetFile(new FileInfoRequest(1, 1, "test", 1));

    result.Should().NotBeNull();
    result.Should().BeOfType<GetFileResponse>();

    if (result is null)
    {
      return;
    }

    result.FileName.Should().Be("test.txt");
    result.ContentLength.Should().Be(10);
    result.ContentType.Should().Be("text/plain");
    result.Stream.Should().NotBeNull();
  }

  [Fact]
  public async Task GetFile_WhenFileIsNotFound_ItShouldReturnNullAfterRetryingThreeTimes()
  {
    var apiResponse = new ApiResponse<GetFileResponse?>
    {
      StatusCode = HttpStatusCode.InternalServerError,
      Message = "Internal Server Error",
      Value = null,
    };

    _mockClient
    .SetupSequence(m => m.GetFileAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()).Result)
    .Returns(apiResponse)
    .Returns(apiResponse)
    .Returns(apiResponse);

    var service = new OnspringService(
      _mockContext.Object,
      _mockClient.Object,
      _mockLogger.Object
    );

    var result = await service.GetFile(new FileInfoRequest(1, 1, "test", 1));

    result.Should().BeNull();
    _mockClient.Verify(
      m => m.GetFileAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()),
      Times.Exactly(3)
    );
  }
}