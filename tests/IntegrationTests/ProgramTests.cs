namespace OnspringAttachmentReporterTests.IntegrationTests
{
  public class ProgramTests
  {
    [Fact]
    public async Task Main_WhenCalledWithValidApiKeyAndAppId_ItShouldReturnZeroAndProduceExpectedReport()
    {
      var args = new string[]
      {
        "-c",
        "testconfig.json",
      };
      var result = await Program.Main(args);
      result.Should().Be(0);
    }
  }
}