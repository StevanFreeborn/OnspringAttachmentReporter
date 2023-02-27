namespace OnspringAttachmentReporterTests.IntegrationTests
{
  public class ProgramTests
  {
    private readonly IConfiguration _configuration;

    public ProgramTests()
    {
      _configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
      .AddEnvironmentVariables()
      .Build();
    }

    [Fact]
    public async Task Main_WhenCalledWithNoApiKeyAppIdOrConfigOption_ItShouldReturnOne()
    {
      var args = new string[]
      {
        "-l",
        "debug"
      };

      var result = await Program.Main(args);
      result.Should().Be(1);
    }

    [Fact]
    public async Task Main_WhenCalledWithValidConfigAndLogLevel_ItShouldReturnZero()
    {
      var configFilePath = _configuration["TestConfigFilePath"];
      configFilePath.Should().NotBeNullOrEmpty();

      var args = new string[]
      {
        "-c",
        configFilePath!,
        "-l",
        "debug"
      };

      var result = await Program.Main(args);
      result.Should().Be(0);
    }

    [Fact]
    public async Task Main_WhenCalledWithValidApiKeyAppIdAndLogLevel_ItShouldReturnZero()
    {
      var apiKey = _configuration["TestApiKey"];
      var appId = _configuration["TestAppId"];

      apiKey.Should().NotBeNullOrEmpty();
      appId.Should().NotBeNullOrEmpty();

      var args = new string[]
      {
        "-k",
        apiKey!,
        "-a",
        appId!,
        "-l",
        "debug"
      };

      var result = await Program.Main(args);
      result.Should().Be(0);
    }

    [Fact]
    public async Task Main_WhenCalledWithValidConfig_ItShouldReturnZero()
    {
      var configFilePath = _configuration["TestConfigFilePath"];
      configFilePath.Should().NotBeNullOrEmpty();

      var args = new string[]
      {
        "-c",
        configFilePath!,
      };

      var result = await Program.Main(args);
      result.Should().Be(0);
    }

    [Fact]
    public async Task Main_WhenCalledWithValidApiKeyAppId_ItShouldReturnZero()
    {
      var apiKey = _configuration["TestApiKey"];
      var appId = _configuration["TestAppId"];

      apiKey.Should().NotBeNullOrEmpty();
      appId.Should().NotBeNullOrEmpty();

      var args = new string[]
      {
        "-k",
        apiKey!,
        "-a",
        appId!,
      };

      var result = await Program.Main(args);
      result.Should().Be(0);
    }
  }
}