using OnspringAttachmentReporter.Extensions;

using FileInfo = System.IO.FileInfo;

internal class Program
{
  internal static async Task<int> Main(string[] args)
  {
    var apiKeyOption = new Option<string>(
      aliases: new string[] { "--apikey", "-k" },
      description: "The API key that will be used to authenticate with Onspring."
    );

    var appIdOption = new Option<int?>(
      aliases: new string[] { "--appId", "-a" },
      description: "The ID of the Onspring app that will be reported on.",
      getDefaultValue: () => null
    );

    var configFileOption = new Option<string>(
      aliases: new string[] { "--config", "-c" },
      description: "The path to the file that specifies configuration for the reporter."
    );

    var logLevelOption = new Option<LogEventLevel>(
      aliases: new string[] { "--log", "-l" },
      description: "Set the minimum level of event that will be logged to the console.",
      getDefaultValue: () => LogEventLevel.Information
    );

    var filesFilterCsvOption = new Option<FileInfo>(
      aliases: new string[] { "--filesFilterCsv", "-ffcsv" },
      description: "The path to the file that specifies a list of files to report on."
    );

    filesFilterCsvOption.AddValidator(
      result =>
      {
        var value = result.GetValueOrDefault<FileInfo>();

        if (
          value is not null &&
          value.Exists is false
        )
        {
          result.ErrorMessage = $"The file {value.FullName} does not exist.";
        }
      }
    );

    var rootCommand = new RootCommand("An app that will report on all attachments in a given Onspring app.")
    {
      apiKeyOption,
      appIdOption,
      configFileOption,
      logLevelOption,
      filesFilterCsvOption
    };

    rootCommand.SetHandler(
      Execute,
      apiKeyOption,
      appIdOption,
      logLevelOption,
      configFileOption,
      filesFilterCsvOption
    );

    return await rootCommand.InvokeAsync(args);
  }

  private async static Task<int> Execute(
    string? apiKeyOption,
    int? appIdOption,
    LogEventLevel logLevelOption,
    string? configFileOption,
    FileInfo? filesFilterCsvOption
  )
  {
    try
    {
      return await Reporter
      .GetContext(
        apiKeyOption,
        appIdOption,
        logLevelOption,
        configFileOption,
        filesFilterCsvOption
      )
      .ConfigureServices()
      .BuildServiceProvider()
      .GetRequiredService<Reporter>()
      .Run();
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return 1;
    }
  }
}