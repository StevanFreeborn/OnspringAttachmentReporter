
using System.Net;

namespace OnspringAttachmentReporterTests.ModelsTests;

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

    _mockClient.Setup(m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()).Result)
      .Returns(apiResponse);

    var service = new OnspringService(_mockContext.Object, _mockClient.Object, _mockLogger.Object);
    var result = await service.GetAllFields();

    result.Should().BeEmpty();
    _mockClient.Verify(m => m.GetFieldsForAppAsync(It.IsAny<int>(), It.IsAny<PagingRequest>()), Times.Once);
  }
}